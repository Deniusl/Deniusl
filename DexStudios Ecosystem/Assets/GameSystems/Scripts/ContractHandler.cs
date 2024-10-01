using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameSystems.Scripts;
using Newtonsoft.Json;
using UnityEngine;
using Types = GameSystems.Scripts.Types;

public class ContractHandler
{
    public static async UniTask<string> MotodDexCall(string method, string abi, string _chain, string _network, string _contract, string _tokenId, string _rpc="")
    {
        if (method == "getPercentForTrack")
            return "30";

#if UNITY_EDITOR
        var cashResult =  MockCashResponseSystem.Instance.GetResponseFromCashe(_network, method, _tokenId);
        if (cashResult.exist)
        {
            return cashResult.response;
        }
#endif

        string response;
        if (AvailableNetworks.CurrentNetworkData.chain == "skale" && method == "valueInMainCoin")
            method = "getPriceForType";

        if (method == "getPriceForType")
        {
            string[] objs = { _tokenId };
            string argss = JsonConvert.SerializeObject(objs);
            response = PriceForTypeManager.GetPriceForType((Types) int.Parse(_tokenId));
            if (response != "")
                return response;
        }
        string[] obj = { _tokenId };
        string args = JsonConvert.SerializeObject(obj);
#if UNITY_WEBGL && !UNITY_EDITOR
        if (AvailableNetworks.CurrentNetworkData.chain != "near")
            response = await Web3GL.MethodCall(abi, _contract, method, args, "");
        else
            response = await Web3GL.NearMethodCall(AvailableNetworks.CurrentNetworkData.network, method, args, "");
#elif UNITY_ANDROID  
        if (AvailableNetworks.CurrentNetworkData.chain == "near")
            response = await AndroidNear.View(method, _tokenId);
        else
            response = await EVM.Call(_chain, _network, _contract, abi, method, args, _rpc);
#else
		response = await EVM.Call(_chain, _network, _contract, abi, method, args, _rpc);
#endif
        if (method == "getPriceForType")
        {
            try
            {
                 BigInteger.Parse(response);
                 PriceForTypeManager.SetPriceForType((Types) int.Parse(_tokenId), response);
            }
            catch
            {
                Debug.Log(response);
            }
        }
//#if UNITY_EDITOR
//        MockCashResponseSystem.Instance.CashValue(_network, new CashPriceData
//        {
//            Method = method,
//            TokenId = _tokenId,
//            Response = response
//        });
//#endif

        try
        {
            Debug.Log($"#debug MotodDexCall chain {_chain} tokenId {_tokenId} method {method} response {response}");
            return response;
        } 
        catch 
        {
            Debug.LogError("MotodDexCall method " + method + " response " + response);
            throw;
        }
    }

    public static async UniTask<string> MotodDexCallWithoutSerialize(string method, string abi, string _chain, string _network, string _contract, string _tokenId, string _rpc = "")
    {
        string response;
        if (AvailableNetworks.CurrentNetworkData.chain == "skale" && method == "valueInMainCoin")
            method = "getPriceForType";
        if (method == "getPriceForType")
        {
            response = PriceForTypeManager.GetPriceForType((Types) int.Parse(_tokenId.Contains("[") ? _tokenId.TrimStart('[').TrimEnd(']').Trim('"') : _tokenId));
            if (response != "")
                return response;
        }
        string args = _tokenId;
#if UNITY_WEBGL && !UNITY_EDITOR
        if (AvailableNetworks.CurrentNetworkData.chain != "near")
            response = await Web3GL.MethodCall(abi, _contract, method, args, "");
        else
            response = await Web3GL.NearMethodCall(AvailableNetworks.CurrentNetworkData.network, method, args, "");
#elif UNITY_ANDROID
        if (AvailableNetworks.CurrentNetworkData.chain == "near")
            response = await AndroidNear.View(method, args);
        else
            response = await EVM.Call(_chain, _network, _contract, abi, method, args, _rpc);
#else
        response = await EVM.Call(_chain, _network, _contract, abi, method, args, _rpc);
#endif
        
        Debug.Log("Response from server: " + response);
        
        if (method == "getPriceForType")
        {
            try
            {
                BigInteger.Parse(response);
                PriceForTypeManager.SetPriceForType((Types) int.Parse(_tokenId), response);
            }
            catch
            {
                Debug.Log(response);
            }
        }

        try
        {
            return response;
        }
        catch
        {
            Debug.LogError(response);
            throw;
        }
    }
}

//tokenURI : tokenId