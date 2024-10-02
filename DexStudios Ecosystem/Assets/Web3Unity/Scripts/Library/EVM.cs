using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameSystems.Scripts;
//using GameSystems.Scripts.Constants;
using Nethereum.Signer;
using Nethereum.Util;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;  
using Web3Unity.Scripts.Library.Ethers.Contracts.Builders;
using Web3Unity.Scripts.Library.Ethers.Providers;
//using Network = GameSystems.Scripts.Network;
using Contract = Web3Unity.Scripts.Library.Ethers.Contracts.Contract;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.ABI.Model;
using Nethereum.Hex.HexConvertors.Extensions;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using Web3Unity.Scripts.Library.Ethers.Transactions;
using Web3Unity.Scripts.Library.Web3Wallet;

public class EVM
{
public class Response<T> { public T response; }

  private readonly static string host = "https://api.gaming.chainsafe.io/evm";

  public static async UniTask<string> BalanceOf(string _account, string _rpc = "")
  {
        Debug.Log($"EVM BalanceOf invoked with parameters:" +
            $"\naccount={_account}," +
            $"\nrpc={_rpc}");
#if UNITY_EDITOR
        var cashResult =  MockCashResponseSystem.Instance.GetResponseFromCashe(AvailableNetworks.CurrentNetworkData.network, "getBalance", string.Empty);
        if (cashResult.exist)
        {
            Debug.Log($"EVM BalanceOf cashResult exists and equals: cashResult={cashResult.response}");
            return cashResult.response;
        }
#endif
    var provider = _rpc == "" ? new JsonRpcProvider(AvailableNetworks.CurrentNetworkData.networkUrl) : new JsonRpcProvider(_rpc);
    var getBalance = await provider.GetBalance(_account);
    Debug.Log("Account Balance: " + getBalance);

#if UNITY_EDITOR
        MockCashResponseSystem.Instance.CashValue(AvailableNetworks.CurrentNetworkData.network, new CashPriceData
        {
            Method = "getBalance",
            TokenId = string.Empty,
            Response = getBalance.ToString()
        });
#endif

    return getBalance.ToString();
  }

  public static async UniTask<string> Verify(string _message, string _signature)
  {
    WWWForm form = new WWWForm();
    form.AddField("message", _message);
    form.AddField("signature", _signature);
    string url = host + "/verify";
    UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
    await webRequest.SendWebRequest();
    Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
    return data.response;
  }
  
  public static string SignVerifySignature(string signatureString, string originalMessage)
  {
    var msg = "\x19" + "Ethereum Signed Message:\n" + originalMessage.Length + originalMessage;
    var msgHash = new Sha3Keccack().CalculateHash(Encoding.UTF8.GetBytes(msg));
    var signature = Nethereum.Signer.MessageSigner.ExtractEcdsaSignature(signatureString);
    var key = EthECKey.RecoverFromSignature(signature, msgHash);
    return key.GetPublicAddress();
  }

  public static async UniTask<string> Call(string _chain, string _network, string _contract, string _abi, string _method, string _args, string _rpc = "")
  {
      string url = string.IsNullOrEmpty(_rpc) 
          ? AvailableNetworks.CurrentNetworkData.networkUrl 
          : _rpc;
      
      Debug.Log($"Using URL: {url}");
      
      try
      {
          var provider = new JsonRpcProvider(url);
          var contract = new Contract(_abi, _contract, provider);
          var args = ContractHelper.ParseAndValidateParameters(_args, _abi, _contract, _method);

          Debug.Log(
              "\n_chain: " + _chain +
              "\nfunction: " + _method +
              "\nparams: " + _args +
              "\nprovider: " + provider +
              "\ncontract: " + _contract +
              "\nnew contract: " + contract +
              "\nabi: " + _abi +
              "\nargs: " + args);

          var calldata = await contract.Call(_method, args);
          Debug.Log("Received calldata: " + calldata);

          var response = ContractHelper.ProcessCalldata(calldata, _method);
          return response;
      }
      catch (JsonSerializationException jsonEx)
      {
          Debug.LogError("JSON Serialization Error: " + jsonEx.Message);
          Debug.LogError("Response might be missing 'jsonrpc' field. Check if URL returns correct JSON-RPC format.");
      }
      catch (Exception ex)
      {
          Debug.LogError("Error calling contract: " + ex.Message);
          Debug.LogError("Stack Trace: " + ex.StackTrace);
      }

      return null;
  }

  public static async UniTask<string> SendContract(string _method, string _abi, string _contract, string _args, string _value, string _gasLimit, string _gasPrice)
    {
        var provider = new JsonRpcProvider(AvailableNetworks.CurrentNetworkData.networkUrl);
        _value = _value == "" ? "0" : _value;
        Debug.Log("SendContract function:'" + _method + "',value:'" + _value + "',params" + _args);
        var contract = new Contract(_abi, _contract, provider);

        var args = ContractHelper.ParseAndValidateParameters(_args, _abi, _contract, _method);

        var data = contract.Calldata(_method, args);

        try
        {
            string response = await Web3Wallet.SendTransaction(AvailableNetworks.CurrentNetworkData.networkChainId, _contract, _value, data, _gasLimit, _gasPrice);
            Debug.Log("Successful acception, response: " + response);
            if (AvailableNetworks.CurrentNetworkData.isEVMNotCompatible == false)
            {
                // for non evm no need check receipt
                var txReceipt = await TxStatus(response, provider);
                while (txReceipt == null)
                {
                    txReceipt = await TxStatus(response, provider);
                }
                if (_method == "purchase" && txReceipt.Status.ToString() == "1")
                    return "success," + txReceipt.Logs[0]["topics"][3].ToString().HexToBigInteger(false);
                string result = txReceipt.Status.ToString() == "1" ? "success" : "fail";
                if (result == "fail")
                    PopUpManager.Instance.PushPopUp(
                        "fail_connect_sendcontract",
                        "Contract error",
                        "SendContract",
                           "",
                        false, PopupButton.ok, true, null, null);
            } else
            {


                return response;
                //if (_method == "purchase")
                //{
                //    // non evm send token_id
                //    var splited = response.Split(",");
                //    var tokenId = splited[0].Replace("[", "");
                //    var hash = splited[1].Replace("]", "");
                //    return "success," + tokenId;
                //}

            }





            return "success";

        }
        catch (Exception e)
        {
            PopUpManager.Instance.PushPopUp(
                      "fail_connect_sendcontract",
                      "Transaction error",
                      "SendContract",
                       "",
                      false, PopupButton.ok, true, null, null);
            Debug.Log("There was issue with the transaction!");
            Debug.LogException(e);
            return "fail";
        }
    }

    public static async UniTask<string> MultiCall(string _chain, string _network, string _contract, string _abi, string _method, string _args, string _multicall = "", string _rpc = "")
    {
        WWWForm form = new WWWForm();
        form.AddField("chain", _chain);
        form.AddField("network", _network);
        form.AddField("contract", _contract);
        form.AddField("abi", _abi);
        form.AddField("method", _method);
        form.AddField("args", _args);
        form.AddField("multicall", _multicall);
        form.AddField("rpc", _rpc);
        string url = host + "/multicall";
        UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
        await webRequest.SendWebRequest();
        Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
        return data.response;
    }

    public static async UniTask<TransactionReceipt> TxStatus(string transactionResult, JsonRpcProvider provider = default)
    {
        provider ??= new JsonRpcProvider(AvailableNetworks.CurrentNetworkData.networkUrl);
        
        var transactionReceipt = await provider.GetTransactionReceipt(transactionResult);
        return transactionReceipt;
    }

  public static async UniTask<int> BlockNumber(string _chain, string _network, string _rpc = "")
  {
    WWWForm form = new WWWForm();
    form.AddField("chain", _chain);
    form.AddField("network", _network);
    form.AddField("rpc", _rpc);
    string url = host + "/blockNumber";
    UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
    await webRequest.SendWebRequest();
    Response<int> data = JsonUtility.FromJson<Response<int>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
    return data.response;
  }

  public static async UniTask<string> Nonce(string _chain, string _network, string _account, string _rpc = "")
  {
    WWWForm form = new WWWForm();
    form.AddField("chain", _chain);
    form.AddField("network", _network);
    form.AddField("account", _account);
    form.AddField("rpc", _rpc);
    string url = host + "/nonce";
    UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
    await webRequest.SendWebRequest();
    Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
    return data.response;
  }

  public static async UniTask<string> CreateContractData(string _abi, string _method, string _args)
  {
    WWWForm form = new WWWForm();
    form.AddField("abi", _abi);
    form.AddField("method", _method);
    form.AddField("args", _args);
    string url = host + "/createContractData";
    UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
    await webRequest.SendWebRequest();
    Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
    return data.response;
  }

  public static async UniTask<string> AllErc721(string _chain, string _network, string _account, string _contract)
  {
    try
    {
      //var balance = await ERC721.BalanceOf(_chain, _network, _contract, _account);
      var balance = await ERC721.BalanceOf(_chain, _network, _contract, _account);
      //Debug.Log("Balance of contract " + _contract + ": " + balance);
      var data = new List<object>();
      while (balance > 0)
      {
        balance -= 1;
        var tokenID = await ERC721.OwnerOfByIndex(_chain, _network, _contract, _account, balance.ToString());
        var tokenURI = await ERC721.URI(_chain, _network, _contract, tokenID);
        data.Add(new
        {
          contract = _contract,
          tokenId = tokenID,
          uri = tokenURI,
          balance = "1"
        });
      }

      string json = JsonConvert.SerializeObject(data);
      return json;
    }
    catch(Exception e)
    {
            PopUpManager.Instance.PushPopUp(
            "fail_connect_erc",
            "AllErc721", 
            "AllErc721",
            "",
            false, PopupButton.ok, true, null, null);

            Debug.LogException(e);
      return "fail";
    }
  }

  public static async UniTask<string> AllErc1155(string _chain, string _network, string _account, string _contract = "")
  {
    WWWForm form = new WWWForm();
    form.AddField("chain", _chain);
    form.AddField("network", _network);
    form.AddField("account", _account);
    form.AddField("contract", _contract);
    string url = host + "/all1155";
    UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
    await webRequest.SendWebRequest();
    Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
    return data.response;
  }

  public static async UniTask<string> GasPrice(string _chain, string _network, string _rpc = "")
  {
    WWWForm form = new WWWForm();
    form.AddField("chain", _chain);
    form.AddField("network", _network);
    form.AddField("rpc", _rpc);
    string url = host + "/gasPrice";
    UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
    await webRequest.SendWebRequest();
    Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
    return data.response;
  }

  public static async UniTask<string> GasLimit(string _chain, string _network, string _to, string _value, string _data, string _rpc = "")
  {
    WWWForm form = new WWWForm();
    form.AddField("chain", _chain);
    form.AddField("network", _network);
    form.AddField("to", _to);
    form.AddField("value", _value);
    form.AddField("data", _data);
    form.AddField("rpc", _rpc);
    string url = host + "/gasLimit";
    UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
    await webRequest.SendWebRequest();
    Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
    return data.response;
  }

  public static async UniTask<string> ChainId (string _chain, string _network, string _rpc = "")
  {
    WWWForm form = new WWWForm();
    form.AddField("chain", _chain);
    form.AddField("network", _network);
    form.AddField("rpc", _rpc);
    string url = host + "/chainId";
    UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
    await webRequest.SendWebRequest();
    Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
    return data.response;
  }

    public static async UniTask<string> CreateTransaction(string _chain, string _network, string _account, string _to, string _value, string _data, string _gasPrice = "", string _gasLimit = "", string _rpc = "")
  {
    WWWForm form = new WWWForm();
    form.AddField("chain", _chain);
    form.AddField("network", _network);
    form.AddField("account", _account);
    form.AddField("to", _to);
    form.AddField("value", _value);
    form.AddField("data", _data);
    form.AddField("gasPrice", _gasPrice);
    form.AddField("gasLimit", _gasLimit);
    form.AddField("rpc", _rpc);
    string url = host + "/createTransaction";
    UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
    await webRequest.SendWebRequest();
    Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
    return data.response;
  }

    public static async UniTask<string> BroadcastTransaction(string _chain, string _network, string _account, string _to, string _value, string _data, string _signature, string _gasPrice, string _gasLimit, string _rpc)
    {
        WWWForm form = new WWWForm();
        form.AddField("chain", _chain);
        form.AddField("network", _network);
        form.AddField("account", _account);
        form.AddField("to", _to);
        form.AddField("value", _value);
        form.AddField("data", _data);
        form.AddField("signature", _signature);
        form.AddField("gasPrice", _gasPrice);
        form.AddField("gasLimit", _gasLimit);
        form.AddField("rpc", _rpc);
        string url = host + "/broadcastTransaction";
        UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
        await webRequest.SendWebRequest();
        Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
        return data.response;
    }
  
    public static async UniTask<string> RaceResults(string _motoId, string _trackId, string _health, string _time, string _account, string _chainName)
    {
        Debug.Log($"EVM RaceResults invoked");

        TimeSpan timeout = TimeSpan.FromSeconds(15);
        ushort attempts = 5;

        RaceResults raceRes = new RaceResults();
        raceRes.timestamp = DateTime.Now.Ticks.ToString();
        raceRes.motoId = _motoId;
        raceRes.trackId = _trackId;

        if (AvailableNetworks.CurrentNetworkData.chain == "near")
            raceRes.health = (double.Parse(_health) * Const.NearHealth.nearHpDiff).ToString("0");
        else if (AvailableNetworks.CurrentNetworkData.chain == "concordium")
            raceRes.health = (double.Parse(_health) / Const.ConcordiumHealth.concordiumHpDiff).ToString("0");
        else
            raceRes.health = _health;

        raceRes.time = _time;
        raceRes.account = _account;
        raceRes.chainName = _chainName;

        string jsonStringTrial = JsonUtility.ToJson(raceRes);
#if UNITY_EDITOR
        Debug.Log($"EVM RaceResults: jsonStringTrial={jsonStringTrial}");
#endif

        var request = await PostRaceResultsAsync(jsonStringTrial, timeout, attempts);

        Debug.Log($"#debug #health #damage #race RaceResult response is valid {request.downloadHandler.data != null}");
        var response = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
        Debug.Log($"#debug #health #damage #race RaceResult response {response}");
        return response;
    }

    private static async Task<UnityWebRequest> PostRaceResultsAsync(string jsonStringTrial, TimeSpan timeout)
    {
        Debug.Log($"EVM PostRaceResultsAsync invoked with parameters:\njson={jsonStringTrial},\ntimeout={timeout}");

        string url = MotoDexApi.Api + EndPoints.Motodex;
        var request = new UnityWebRequest(url);
        request.timeout = (int)timeout.TotalSeconds;
        request.method = "POST";
        request.uploadHandler = new UploadHandlerRaw(RSACryptoProvider.EncryptData(jsonStringTrial, Application.streamingAssetsPath + "/public_key.pem")); // postData is Json file as a string
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Cache-Control", "no-cache");

        await request.SendWebRequest();

        if(request.result != UnityWebRequest.Result.Success)
        {
            throw new UnityWebRequestException(request);
        }

        return request;
    }

    private static async Task<UnityWebRequest> PostRaceResultsAsync(string jsonStringTrial, TimeSpan timeout, ushort attempts)
    {
        Debug.Log($"EVM PostRaceResultsAsync invoked with parameters:\njson={jsonStringTrial},\ntimeout={timeout},\nattempts={attempts}");

        UnityWebRequest request;
        while(true)
        {
            try
            {
                request = await PostRaceResultsAsync(jsonStringTrial, timeout);
                break;
            }
            catch (UnityWebRequestException e)
            {
                if (e.Error != "Request timeout") throw e;
                if (attempts == 0 || attempts - 1 == 0) throw e;

                attempts--;
                Debug.Log($"EVM PostRaceResultAsync request timed out, remaining attempts: {attempts}");
            }
        }
        Debug.LogWarning($"EVM PostRaceResultsAsync finished with result={request.result}");
        return request;
    }

    public static async UniTask<string> SendCurrHealthWei(string _motoId, string _healthWei)
    {
        Debug.Log($"EVM SendCurrHealthWei invoked with parameters:\nmotoID={_motoId},\nhealthWei={_healthWei}");
        TimeSpan timeout = TimeSpan.FromSeconds(15);
        ushort attempts = 5;

        CurrHealthWei currHealthWei = new CurrHealthWei();
        currHealthWei.timestamp = DateTime.Now.Ticks.ToString();
        currHealthWei.motoId = _motoId;

        if (AvailableNetworks.CurrentNetworkData.chain == "near")
            currHealthWei.healthWei = (double.Parse(_healthWei) * Const.NearHealth.nearHpDiff).ToString("0");
        else if (AvailableNetworks.CurrentNetworkData.chain == "concordium")
            currHealthWei.healthWei = (double.Parse(_healthWei) / Const.ConcordiumHealth.concordiumHpDiff).ToString("0");
        else
            currHealthWei.healthWei = _healthWei;

        currHealthWei.chainName = AvailableNetworks.CurrentNetworkData.uriPathName;
        string jsonStringTrial = JsonUtility.ToJson(currHealthWei);

#if UNITY_EDITOR
        Debug.Log(jsonStringTrial);
#endif

        UnityWebRequest webRequest = await PostHealthWeiAsync(jsonStringTrial, timeout, attempts);

        if (webRequest.downloadHandler.data != null)
        {
            var response = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);
            Debug.Log($"#debug SendCurrHealthWei response {response}");
            return response;
        }

        return string.Empty;
    }

    private static async Task<UnityWebRequest> PostHealthWeiAsync(string jsonStringTrial, TimeSpan timeout)
    {
        Debug.Log($"EVM PoseCurrHealthWeiAsync invoked with parameters:\njson={jsonStringTrial},\ntimeout={timeout}");
        string url = MotoDexApi.Api + EndPoints.GetHealth;
        var request = new UnityWebRequest(url);
        request.timeout = (int)timeout.TotalSeconds;
        request.method = "POST";
        request.uploadHandler = new UploadHandlerRaw(RSACryptoProvider.EncryptData(jsonStringTrial, Application.streamingAssetsPath + "/public_key.pem")); // postData is Json file as a string
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Cache-Control", "no-cache");

        await request.SendWebRequest();

        if(request.result != UnityWebRequest.Result.Success)
        {
            throw new UnityWebRequestException(request);
        }

        Debug.Log($"#debug SendCurrHealthWei response is valid {request.downloadHandler.data != null}");
        return request;
    }

    private static async Task<UnityWebRequest> PostHealthWeiAsync(string jsonStringTrial, TimeSpan timeout, ushort attempts)
    {
        Debug.Log($"EVM PoseCurrHealthWeiAsync invoked with parameters:\njson={jsonStringTrial},\ntimeout={timeout},\nattempts={attempts}");
        UnityWebRequest request;
        while (true)
        {
            try
            {
                request = await PostHealthWeiAsync(jsonStringTrial, timeout);
                break;
            }
            catch (UnityWebRequestException e)
            {
                if (e.Error != "Request timeout") throw e;
                if (attempts == 0 || attempts - 1 == 0) throw e;

                attempts--;
                Debug.LogWarning($"EVM PostHealthWeiAsync request timed out, remaining attempts: {attempts}");
            }
        }
        Debug.Log($"EVM PostHealthWeiAsync finished with result={request.result}");
        return request;
    }

    public static async UniTask<string> GetCurrHealthWei(string _motoId)
    {
        Debug.Log($"EVM GetCurrHealthWei invoked with parameters: motoID={_motoId}");
        TimeSpan timeout = TimeSpan.FromSeconds(15);
        ushort attempts = 5;

        string url = MotoDexApi.Api + EndPoints.GetHealth + "/" + AvailableNetworks.CurrentNetworkData.uriPathName + "/" + _motoId;
        string response = await GetHealthWeiAsync(url, timeout, attempts);

        return response;
    }

    private static async Task<string> GetHealthWeiAsync(string url, TimeSpan timeout)
    {
        Debug.Log($"EVM GetHealthWeiAsync invoked with parameters:\nurl={url},\ntimeout={timeout}");
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.timeout = (int)timeout.TotalSeconds;
        request.SetRequestHeader("Cache-Control", "no-cache");

        await request.SendWebRequest();

        if(request.result != UnityWebRequest.Result.Success)
        {
            throw new UnityWebRequestException(request);
        }

        Debug.Log(request.downloadHandler.text);

        Debug.Log($"#debug #health GetCurrHealthWei response is valid {request.downloadHandler.data != null}");
        Debug.Log($"#debug #health GetHealthValues #aptos chainUsed: {AvailableNetworks.CurrentNetworkData.uriPathName} response is valid {request.downloadHandler.data != null}");

        var response = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
        Debug.Log($"#debug #health GetCurrHealthWei response {response}");
        return response;
    }

    private static async Task<string> GetHealthWeiAsync(string url, TimeSpan timeout, ushort attempts)
    {
        Debug.Log($"EVM GetHealthWeiAsync invoked with parameters:\nurl={url},\ntimeout={timeout},\nattempts={attempts}");
        string response;
        while (true)
        {
            try
            {
                response = await GetHealthWeiAsync(url, timeout);
                break;
            }
            catch (UnityWebRequestException e)
            {
                if (e.Error != "Request timeout") throw e;
                if (attempts == 0 || attempts - 1 == 0) throw e;

                attempts--;
                Debug.LogWarning($"EVM GetHealthWeiAsync request timed out, remaining attempts: {attempts}");
            }
        }
        Debug.Log($"EVM GetHealthWeiAsync finished with result={response}");
        return response;
    }
}
