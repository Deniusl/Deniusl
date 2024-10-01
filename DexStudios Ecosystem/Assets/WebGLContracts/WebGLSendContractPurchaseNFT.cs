using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameSystems.Scripts;
using Newtonsoft.Json;
using UnityEngine;
using Network = GameSystems.Scripts.Network;


public class WebGLSendContractPurchaseNFT : MonoBehaviour
{
    static async public UniTask<string> PurchaseNFT(string contract, string args)
    {
        string chain = Chain.InUse;
        string network = Network.InUse;
        string _typeId = args;

#if UNITY_EDITOR
        return "0xeee85f8344af80b998d7994f464b5b3c8c611c7a7a5e265065092a28ccb696ca,999";
#endif

        string[] obj = {args, "0x0000000000000000000000000000000000000000"};
        args = JsonConvert.SerializeObject(obj);

        // abi in json format
        string abi = Abis.MotoDEXnftABI;
        // gas limit OPTIONAL
        string gasLimit = "";
        // gas price OPTIONAL
        string gasPrice = "";
        // connects to user's browser wallet (metamask) to update contract state
        string method;

        Web3GL.GoogleAnalyticsSendEvent("purchase_nft_clicks");

        if (AvailableNetworks.CurrentNetworkData.chain != "near")
        {
            string valueInMainCoin = "0";
            if (AvailableNetworks.CurrentNetworkData.chain != "concordium")
                valueInMainCoin = await ContractHandler.MotodDexCall(method = "valueInMainCoin", abi, chain, network,
                    contract, _typeId);
            Debug.Log(valueInMainCoin);
#if UNITY_WEBGL && !UNITY_EDITOR
                string response = await Web3GL.SendContract(method =
 "purchase", abi, contract, args, valueInMainCoin, gasLimit, gasPrice);

#else
            string response = await EVM.SendContract(method = "purchase", abi, contract, args, valueInMainCoin,
                gasLimit, gasPrice);

#endif
            Debug.Log("#debug #buy #nft WebGLSendContractPurchaseNFT.PurchaseNFT" + response);
            return response;
        }
        else
        {
            string response = await Web3GL.NearSendContract(AvailableNetworks.CurrentNetworkData.network,
                method = "purchase", args, "");
            Debug.Log(response);
            return response;
        }
    }
}