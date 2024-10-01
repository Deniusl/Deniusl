using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameSystems.Scripts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;


public class Web3GL
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void SendContractJs(string method, string abi, string contract, string args, string value, string gasLimit, string gasPrice);

    [DllImport("__Internal")]
    private static extern string SendContractResponse();

    [DllImport("__Internal")]
    private static extern void SetContractResponse(string value);

    private static bool _contractInProgress;

    [DllImport("__Internal")]
    private static extern void SendTransactionJs(string to, string value, string gasLimit, string gasPrice);

    [DllImport("__Internal")]
    private static extern string SendTransactionResponse();

    [DllImport("__Internal")]
    private static extern void SetTransactionResponse(string value);

    private static bool _transactionInProgress;

    [DllImport("__Internal")]
    private static extern void SignMessage(string value);

    [DllImport("__Internal")]
    private static extern string SignMessageResponse();

    [DllImport("__Internal")]
    private static extern void SetSignMessageResponse(string value);

    private static bool _signMessageInProgress;

    [DllImport("__Internal")]
    private static extern int GetNetwork();
    
    [DllImport("__Internal")]
    private static extern void AddNetworkJs(string value);
    
    [DllImport("__Internal")]
    private static extern string AddNetworkResponse();
    
    [DllImport("__Internal")]
    private static extern void SetAddNetworkResponse(string value);

    private static bool _addNetworkInProgress;
    
    [DllImport("__Internal")]
    private static extern void ChangeChainIdJs(string value);
    
    [DllImport("__Internal")]
    private static extern void GetAllErc721Js(string abi, string contract);

    [DllImport("__Internal")]
    private static extern string GetAllErc721Response();

    [DllImport("__Internal")]
    private static extern void SetAllErc721Response(string value);

    private static bool _allErc721InProgress;
    
    [DllImport("__Internal")]
    private static extern void GetLatestEpochJs(string abi, string contract);

    [DllImport("__Internal")]
    private static extern string GetLatestEpochResponse();

    [DllImport("__Internal")]
    private static extern void SetLatestEpochResponse(string value);

    private static bool _latestEpochInProgress;
    
    [DllImport("__Internal")]
    private static extern void MethodCallJs(string abi, string contract, string method, string args, string value);

    [DllImport("__Internal")]
    private static extern string MethodCallResponse();

    [DllImport("__Internal")]
    private static extern void SetMethodCallResponse(string value);

    private static bool _methodCallInProgress;
    
    [DllImport("__Internal")]
    private static extern void GetTxStatusJs(string transactionHash);

    [DllImport("__Internal")]
    private static extern string GetTxStatusResponse();

    [DllImport("__Internal")]
    private static extern void SetTxStatusResponse(string value);

    private static bool _txStatusInProgress;
    
    [DllImport("__Internal")]
    private static extern void GetBalanceJs();

    [DllImport("__Internal")]
    private static extern string GetBalanceResponse();

    [DllImport("__Internal")]
    private static extern void SetBalanceResponse(string value);

    private static bool _balanceInProgress;
    
    [DllImport("__Internal")]
    private static extern void NearSendContractJs(string mainnet, string motoDexContract, string method, string args, string value);

    [DllImport("__Internal")]
    private static extern string NearSendContractResponse();

    [DllImport("__Internal")]
    private static extern void NearSetContractResponse(string value);

    private static bool _nearContractProgress;
    
    [DllImport("__Internal")]
    private static extern void NearMethodCallJs(string mainnet, string motoDexContract, string method, string args, string value);

    [DllImport("__Internal")]
    private static extern string NearMethodCallResponse();

    [DllImport("__Internal")]
    private static extern void NearSetMethodCallResponse(string value);

    private static bool _nearMethodProgress;
    
    [DllImport("__Internal")]
    private static extern void GetListNearNFTsWebJs(string mainnet, string contractAddress, string selectedAccount);

    [DllImport("__Internal")]
    private static extern string GetListNearNFTsWebResponse();

    [DllImport("__Internal")]
    private static extern void SetListNearNFTsWebResponse(string value);

    private static bool _listNearNFTsProgress;
    
    [DllImport("__Internal")]
    private static extern void GetNearLatestEpochJs(string mainnet, string contractAddress);

    [DllImport("__Internal")]
    private static extern string GetNearLatestEpochResponse();

    [DllImport("__Internal")]
    private static extern void SetNearLatestEpochResponse(string value);

    private static bool _nearLatestEpochProgress;
    
    [DllImport("__Internal")]
    private static extern void WebGLReloadJs(string value);
    
    [DllImport("__Internal")]
    private static extern void GoogleAnalyticsSendEventJs(string value);
#endif

    private const int DEFAULT_DELAY = 2000;
    private const int RETRY_MAX = 45;

    // this function will create a metamask tx for user to confirm.
    async public static UniTask<string> SendContract(string _method, string _abi, string _contract, string _args, string _value, string _gasLimit = "", string _gasPrice = "")
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        using LoadingObject loadingObject = await InProgress(_contractInProgress, () => _contractInProgress = false, nameof(SendContract));
        _contractInProgress = true;

        Debug.Log("SendContract function:'" + _method + "',params" + _args + ",value:'" + _value + "'");
        // Set response to empty
        SetContractResponse("");
        SendContractJs(_method, _abi, _contract, _args, _value, _gasLimit, _gasPrice);

        string response = await RetryWrapper(SendContractResponse, nameof(SendContractResponse));

        Debug.Log("SendContract function:'" + _method + "',params" + _args + ",value:'" + _value + "'" + " response " + response + " AvailableNetworks.CurrentNetworkData.chain " + AvailableNetworks.CurrentNetworkData.chain);
        if (AvailableNetworks.CurrentNetworkData.isEVMNotCompatible)
        {
            if (AvailableNetworks.CurrentNetworkData.chain == "aptos")
            {
                Debug.Log("aptos Response: " + response);
                return response;
            }

            if (AvailableNetworks.CurrentNetworkData.chain == "alephzero")
            {
                Debug.Log("alephzero Response: " + response);
                return response;
            }

            if (AvailableNetworks.CurrentNetworkData.chain == "sui_t" || AvailableNetworks.CurrentNetworkData.chain == "sui")
            {
                Debug.Log(AvailableNetworks.CurrentNetworkData.chain + " Response: " + response);
                return response;
            }

            if (AvailableNetworks.CurrentNetworkData.chain == "icp_t" || AvailableNetworks.CurrentNetworkData.chain == "icp")
            {
                Debug.Log(AvailableNetworks.CurrentNetworkData.chain + " Response: " + response);
                return response;
            }

            if (AvailableNetworks.CurrentNetworkData.chain == "nibiru" ||
                AvailableNetworks.CurrentNetworkData.chain == "nibiru_t")
            {
                Debug.Log(AvailableNetworks.CurrentNetworkData.chain + " Response: " + response);
                return response;
            }
        }

        JObject _contractData = null;
        if (!AvailableNetworks.CurrentNetworkData.isEVMNotCompatible)
        {
            if (AvailableNetworks.CurrentNetworkData.chain != "concordium" &&
                AvailableNetworks.CurrentNetworkData.chain != "alephzero" &&
                AvailableNetworks.CurrentNetworkData.chain != "aptos" &&
                AvailableNetworks.CurrentNetworkData.chain != "sui_t" &&
                AvailableNetworks.CurrentNetworkData.chain != "sui" &&
                AvailableNetworks.CurrentNetworkData.chain != "nibiru_t" &&
                AvailableNetworks.CurrentNetworkData.chain != "nibiru" &&
                AvailableNetworks.CurrentNetworkData.chain != "icp_t" &&
                AvailableNetworks.CurrentNetworkData.chain != "icp"
               )
            {
                Debug.Log("#debug #buy #nft Response: " + response);
                _contractData = JObject.Parse(response);
                if (_contractData.ContainsKey("transactionHash"))
                    response = _contractData["transactionHash"].ToString();
            }
        }

        SetContractResponse("");
        // check if user submmited or user rejected
        if (response.Length == 66 || (AvailableNetworks.CurrentNetworkData.chain == "concordium" && response.Length > 0)) 
        {
            if (_method == "purchase" && AvailableNetworks.CurrentNetworkData.chain != "concordium")
            {
                if (_contractData.ContainsKey("events"))
                {
                    var tokenId = _contractData["events"]["Transfer"]["returnValues"]["tokenId"];
                    Debug.Log($"#debug #buy #nft TokenId: {tokenId}");
                    Debug.Log($"#debug #buy #nft TokenId: " + tokenId);

                    response += "," + tokenId;
                }
            }
            Debug.Log($"#debug #buy #nft SendContract method: {_method} response: {response}");
            return response;
        } 
        else 
        {
        PopUpManager.Instance.PushPopUp(
   "fail_connect_sendcontract",
   "SendContract",
   response,
         "",
   false, PopupButton.ok, true, null, null);
            throw new Exception(response);
        }
#else
        PopUpManager.Instance.PushPopUp(
   "fail_connect_sendContract",
   "SendContract",
   "fail",
   "",
   false, PopupButton.ok, true, null, null);
        return "fail";
#endif
    }

    async public static UniTask<string> SendTransaction(string _to, string _value, string _gasLimit = "", string _gasPrice = "")
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        using LoadingObject loadingObject = await InProgress(_transactionInProgress, () => _transactionInProgress = false, nameof(SendTransaction));
        _transactionInProgress = true;

        // Set response to empty
        SetTransactionResponse("");
        SendTransactionJs(_to, _value, _gasLimit, _gasPrice);
        
        string response = await RetryWrapper(SendTransactionResponse, nameof(SendTransactionResponse));

        SetTransactionResponse("");
        // check if user submmited or user rejected
        if (response.Length == 66) 
        {
            return response;
        } 
        else 
        {
        PopUpManager.Instance.PushPopUp(
   "fail_connect_sendtransaction",
   "SendTransaction",
   response,
         "",

   false, PopupButton.ok, true, null, null);
            throw new Exception(response);
        }

#else
        PopUpManager.Instance.PushPopUp(
   "fail_connect_sendtransaction",
   "SendTransaction",
   "fail",
   "",
   false, PopupButton.ok, true, null, null);
        return "fail";
#endif
    }

    async public static UniTask<string> Sign(string _message)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        using LoadingObject loadingObject = await InProgress(_signMessageInProgress, () => _signMessageInProgress = false, nameof(Sign));
        _signMessageInProgress = true;

        SignMessage(_message);

        string response = await RetryWrapper(SignMessageResponse, nameof(SignMessageResponse));
        
        // Set response to empty
        SetSignMessageResponse("");
        // check if user submmited or user rejected
        if (response.Length == 132)
        {
            return response;
        } 
        else 
        {
        PopUpManager.Instance.PushPopUp(
   "fail_connect_sign",
   "Sign",
   response,
         "",

   false, PopupButton.ok, true, null, null);
        return "fail";
            throw new Exception(response);
        }

#else
        PopUpManager.Instance.PushPopUp(
   "fail_connect_sign",
   "Sign",
   "fail",
   "",
   false, PopupButton.ok, true, null, null);
        return "fail";
#endif
    }

    async public static UniTask<int> Network()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        return GetNetwork();

#else
        throw new Exception("Wrong platform");
#endif
    }
    async public static UniTask<string> AddNetwork(string _networkId)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        using LoadingObject loadingObject = await InProgress(_addNetworkInProgress, () => _addNetworkInProgress = false, nameof(AddNetwork));
        _addNetworkInProgress = true;

        AddNetworkJs(_networkId);

        string response = await RetryWrapper(AddNetworkResponse, nameof(AddNetworkResponse));
        
        // Set response to empty
        SetAddNetworkResponse("");
        // check if user submmited or user rejected
        if (response == "Success" || response == "Error")
        {
            return response;
        } 
        else 
        {
        PopUpManager.Instance.PushPopUp(
   "fail_connect_addnet",
   "AddNetwork",
   response,
         "",

   false, PopupButton.ok, true, null, null);
        return "fail";
            throw new Exception(response);
        }
#else
        PopUpManager.Instance.PushPopUp(
   "fail_connect_addnet",
   "AddNetwork",
   "fail",
      "",
   false, PopupButton.ok, true, null, null);
        return "fail";
#endif
    }
    async public static UniTask ChangeChainId(string _chainId)
    {
        Debug.Log($"Change chain id, UNITY {_chainId}");
#if UNITY_WEBGL && !UNITY_EDITOR
        ChangeChainIdJs(_chainId);
#endif
    }
    
    async public static UniTask<string> AllErc721(string _abi, string _contract)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        using LoadingObject loadingObject = await InProgress(_allErc721InProgress, () => _allErc721InProgress = false, nameof(AllErc721));
        _allErc721InProgress = true;

        // Set response to empty
        SetAllErc721Response("");
        GetAllErc721Js(_abi, _contract);

        string response = await RetryWrapper(GetAllErc721Response, nameof(GetAllErc721Response));

        SetAllErc721Response("");
        // check if user submmited or user rejected
        if (response.Length > 0) 
        {
            return response;
        } 
        else 
        {
        PopUpManager.Instance.PushPopUp(
            "fail_connect_erc",
            "Erc721 error",
            response,
                  "",

            false, PopupButton.ok, true, null, null);
            throw new Exception(response);
        }

#else
        PopUpManager.Instance.PushPopUp(
            "fail_connect_erc",
            "Erc721 error",
            "AllErc721",
               "",
            false, PopupButton.ok, true, null, null);
        return "fail";
#endif
    }
    async public static UniTask<string> MethodCall(string _abi, string _contract, string _method, string _args, string _value)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        using LoadingObject loadingObject = await InProgress(_methodCallInProgress, () => _methodCallInProgress = false, nameof(MethodCall));
        _methodCallInProgress = true;

        Debug.Log("MethodCall function:'" + _method + "',params" + _args + ",value:'" + _value + "'");
        // Set response to empty
        SetMethodCallResponse("");
        MethodCallJs(_abi, _contract, _method, _args, _value);

        string response = await RetryWrapper(MethodCallResponse, nameof(MethodCallResponse));

        Debug.Log($"#debug #webgl3 #methodcall #response - {response}");
        SetMethodCallResponse("");
        // check if user submmited or user rejected
        if (response.Length > 0) 
        {
            Debug.Log(_method + " response:" + response);
            return response;
        } 
        else 
        {
            Debug.Log($"#debug #buy #nft #error {_method} error#1");
        PopUpManager.Instance.PushPopUp(
            "fail_connect_methodcall",
            "MethodCall",
            response,
                  "",

            false, PopupButton.ok, true, null, null);
            throw new Exception(response);
        }

#else
        PopUpManager.Instance.PushPopUp(
            "fail_connect_methodcall",
            "MethodCall",
            "MethodCall",
               "",
            false, PopupButton.ok, true, null, null);
            Debug.Log($"#debug #buy #nft #error {_method} error#2");

        return "fail";
#endif
    }

    async public static UniTask<string> GetTxStatus(string _transactionHash)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        using LoadingObject loadingObject = await InProgress(_txStatusInProgress, () => _txStatusInProgress = false, nameof(GetTxStatus));
        _txStatusInProgress = true;

        // Set response to empty
        SetTxStatusResponse("");
        GetTxStatusJs(_transactionHash);

        string response = await RetryWrapper(GetTxStatusResponse, nameof(GetTxStatusResponse));

        SetTxStatusResponse("");
        // check if user submmited or user rejected
        if (response.Length > 0) 
        {
            return response;
        } 
        else 
        {
        PopUpManager.Instance.PushPopUp(
            "fail_connect_gettxstatus",
            "GetTxStatus",
            response,
                  "",

            false, PopupButton.ok, true, null, null);
            throw new Exception(response);
        }

#else
        PopUpManager.Instance.PushPopUp(
            "fail_connect_gettxstatus",
            "GetTxStatus", "",
            "fail",
            false, PopupButton.ok, true, null, null);
        return "fail";
#endif
    }
    
    async public static UniTask<string> GetBalance()
    {
#if UNITY_WEBGL && !UNITY_EDITOR        
        using LoadingObject loadingObject = await InProgress(_balanceInProgress, () => _balanceInProgress = false, nameof(GetBalance));
        _balanceInProgress = true;

        // Set response to empty
        SetBalanceResponse("");
        GetBalanceJs();

        string response = await RetryWrapper(GetBalanceResponse, nameof(GetBalanceResponse));
        
        SetBalanceResponse("");
        // check if user submmited or user rejected
        if (response.Length > 0) 
        {
            return response;
        } 
        else 
        {
        PopUpManager.Instance.PushPopUp(
            "fail_connect_getbalance",
            "GetBalance",
            response,
                  "",
            false, PopupButton.ok, true, null, null);
            throw new Exception(response);
        }

#else
        PopUpManager.Instance.PushPopUp(
            "fail_connect_getbalance",
            "GetBalance", "",
            "fail",
            false, PopupButton.ok, true, null, null);
        return "fail";
#endif
    }
    
    async public static UniTask<string> ListNearNFTs(string mainnet, string contractAddress, string selectedAccount)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        using LoadingObject loadingObject = await InProgress(_listNearNFTsProgress, () => _listNearNFTsProgress = false, nameof(ListNearNFTs));
        _listNearNFTsProgress = true;

        // Set response to empty
        SetListNearNFTsWebResponse("");
        GetListNearNFTsWebJs(mainnet, contractAddress, selectedAccount);

        string response = await RetryWrapper(GetListNearNFTsWebResponse, nameof(GetListNearNFTsWebResponse));
        
        SetListNearNFTsWebResponse("");
        // check if user submmited or user rejected
        if (response.Length > 0) 
        {
            return response;
        } 
        else 
        {
        PopUpManager.Instance.PushPopUp(
           "fail_connect_listNnft",
           "ListNearNFTs",
           response,
                 "",
           false, PopupButton.ok, true, null, null);
            throw new Exception(response);
        }

#else
        PopUpManager.Instance.PushPopUp(
           "fail_connect_listNnft",
           "ListNearNFTs",
           "fail",
              "",

           false, PopupButton.ok, true, null, null);
        return "fail";
#endif
    }
    
    async public static UniTask<string> NearLatestEpoch(string mainnet, string contractAddress)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        using LoadingObject loadingObject = await InProgress(_nearLatestEpochProgress, () => _nearLatestEpochProgress = false, nameof(NearLatestEpoch));
        _nearLatestEpochProgress = true;

        // Set response to empty
        SetNearLatestEpochResponse("");
        GetNearLatestEpochJs(mainnet, contractAddress);

        string response = await RetryWrapper(GetNearLatestEpochResponse, nameof(GetNearLatestEpochResponse));

        SetNearLatestEpochResponse("");
        // check if user submmited or user rejected
        if (response.Length > 0) 
        {
            return response;
        } 
        else 
        {
        PopUpManager.Instance.PushPopUp(
   "fail_connect_Nlatestepoch",
   "NearLatestEpoch",
   response,
         "",
   false, PopupButton.ok, true, null, null);
        return "fail";
            throw new Exception(response);
        }

#else
        PopUpManager.Instance.PushPopUp(
   "fail_connect_Nlatestepoch",
   "NearLatestEpoch",
   "fail",
      "",

   false, PopupButton.ok, true, null, null);
        return "fail";
#endif
    }
    
    async public static UniTask<string> NearSendContract(string mainnet, string method, string args, string value)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        using LoadingObject loadingObject = await InProgress(_nearContractProgress, () => _nearContractProgress = false, nameof(NearSendContract));
        _nearContractProgress = true;

        Debug.Log("NearSendContract function:'" + method + "',params" + args + ",value:'" + value + "'");
        string motoDexContract = JsonConvert.SerializeObject(AvailableNetworks.CurrentNetworkData.contracts);
        // Set response to empty
        NearSetContractResponse("");
        NearSendContractJs(mainnet, motoDexContract, method, args, value);

        string response = await RetryWrapper(NearSendContractResponse, nameof(NearSendContractResponse));

        Debug.Log("Response: "+response);
        NearSetContractResponse("");
        // check if user submmited or user rejected
        if (response.Length > 0) 
        {
            Debug.Log(method + " response:" + response);
            return response;
        }
        else 
        {
        PopUpManager.Instance.PushPopUp(
   "fail_connect_Nsendcontract",
   "NearSendContract",
   response,
         "",

   false, PopupButton.ok, true, null, null);
            throw new Exception(response);
        }

#else
        PopUpManager.Instance.PushPopUp(
   "fail_connect_Nsendcontract",
   "NearSendContract",
   "fail",
      "",

   false, PopupButton.ok, true, null, null);
        return "fail";
#endif
    }
    
    async public static UniTask<string> NearMethodCall(string mainnet, string method, string args, string value)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        using LoadingObject loadingObject = await InProgress(_nearMethodProgress, () => _nearMethodProgress = false, nameof(NearMethodCall));
        _nearMethodProgress = true;

        Debug.Log("NearMethodCall function:'" + method + "',params" + args + ",value:'" + value + "'");
        string motoDexContract = JsonConvert.SerializeObject(AvailableNetworks.CurrentNetworkData.contracts);
        // Set response to empty
        NearSetMethodCallResponse("");
        NearMethodCallJs(mainnet, motoDexContract, method, args, value);

        string response = await RetryWrapper(NearMethodCallResponse, nameof(NearMethodCallResponse));

        Debug.Log("Response: "+response);
        NearSetMethodCallResponse("");
        // check if user submmited or user rejected
        if (response.Length > 0) 
        {
            Debug.Log(method + " response:" + response);
            return response;
        }
        else 
        {
                PopUpManager.Instance.PushPopUp(
   "fail_connect_Nmethodcall",
   "NearMethodCall",
   response,
         "",

   false, PopupButton.ok, true, null, null);
            throw new Exception(response);
        }

#else
        PopUpManager.Instance.PushPopUp(
   "fail_connect_Nmethodcall",
   "NearMethodCall",
   "fail",
      "",

   false, PopupButton.ok, true, null, null);
        return "fail";
#endif
    }

    async public static UniTask<string> LatestEpoch(string _abi, string _contract)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        using LoadingObject loadingObject = await InProgress(_latestEpochInProgress, () => _latestEpochInProgress = false, nameof(LatestEpoch));
        _latestEpochInProgress = true;

        // Set response to empty
        SetLatestEpochResponse("");
        GetLatestEpochJs(_abi, _contract);

        string response = await RetryWrapper(GetLatestEpochResponse, nameof(GetLatestEpochResponse));

        SetLatestEpochResponse("");
        // check if user submmited or user rejected
        if (response.Length > 0)
        {
            return response;
        }
        else
        {
            PopUpManager.Instance.PushPopUp(
       "fail_connect_latestEpoch",
       "LatestEpoch",
       response,
             "",
       false, PopupButton.ok, true, null, null);
            throw new Exception(response);
        }
#else
        PopUpManager.Instance.PushPopUp(
   "fail_connect_latestEpoch",
   "LatestEpoch",
   "fail",
      "",

   false, PopupButton.ok, true, null, null);
        return "fail";
#endif
    }

    private static async Task<LoadingObject> InProgress(bool inProgress, Action complete, string methodName)
    {
        Debug.Log($"#debug #inProgress start {methodName}");
        while (inProgress)
            await UniTask.Delay(DEFAULT_DELAY);

        var loadingObject = new LoadingObject(complete);
        return loadingObject;
    }

    private static async UniTask<string> RetryWrapper(Func<string> responseCallback, string methodName)
    {
        int currentTry = 0;

        string response = responseCallback();
        Debug.Log($"Check for empty - {string.IsNullOrEmpty(response)}");
        
        currentTry++;

        Debug.Log($"#debug #retry {methodName} first {currentTry} response {response}");

        while (response == "" && currentTry < RETRY_MAX)
        {
            await UniTask.Delay(DEFAULT_DELAY);
            response = responseCallback();
            Debug.Log($"#debug #web3gl {methodName} retry {currentTry} response {response}");
            currentTry++;
        }
        
        if (response == "")
            response = "fail";

        Debug.Log($"#debug #webgl3 #retryWrapper #response - {response}");
        return response;
    }
    
    async public static UniTask WebGLReload(bool clearCookies)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        WebGLReloadJs(clearCookies.ToString());
#endif
    }

    async public static UniTask GoogleAnalyticsSendEvent(string eventName)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        GoogleAnalyticsSendEventJs(eventName);
#endif
    }
   
}