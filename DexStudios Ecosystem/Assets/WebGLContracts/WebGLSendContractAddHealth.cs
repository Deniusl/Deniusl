using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameSystems.Scripts;
using Newtonsoft.Json;
using UnityEngine;
using Network = GameSystems.Scripts.Network;

public class WebGLSendContractAddHealth : MonoBehaviour
{
    static async public UniTask<string> AddHealthMoney(string contract, string args, string value)
    {
        string chain = Chain.InUse;
        string network = Network.InUse;
        
        string[] argsForSerialize = {args};
        args = JsonConvert.SerializeObject(argsForSerialize);
        // abi in json format
        string abi = Abis.MotoDEXnftABI;
        // gas limit OPTIONAL
        string gasLimit = "";
        // gas price OPTIONAL
        string gasPrice = "";
        // connects to user's browser wallet (metamask) to update contract state
        try {
            if (AvailableNetworks.CurrentNetworkData.chain != "near")
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                string response =
                    await Web3GL.SendContract("addHealthMoney", abi, contract, args, value, gasLimit, gasPrice);
#else
                string response =
                    await EVM.SendContract("addHealthMoney", abi, contract, args, value, gasLimit, gasPrice);
#endif
                Debug.Log(response);
                return response;
            }
            else
            {
                string response = await Web3GL.NearSendContract(AvailableNetworks.CurrentNetworkData.network, "addHealthMoney", args, value);
                Debug.Log(response);
                return response;
            }
        } catch (Exception e) {
            Debug.LogException(e);
            return "fail";
        }
    }
}