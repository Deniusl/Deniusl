using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameSystems.Scripts;
using Newtonsoft.Json;
using UnityEngine;
using Network = GameSystems.Scripts.Network;

public class WebGLSendContractBid : MonoBehaviour
{
    static async public UniTask<string> Bid(string contract, string args, string value)
    {
        string chain = Chain.InUse;
        string network = Network.InUse;

        //value = Convert.ToInt64(Convert.ToDouble(value) * Math.Pow(10,18)).ToString();
        // abi in json format
        string abi = Abis.MotoDEXABI;
        // gas limit OPTIONAL
        string gasLimit = "";
        // gas price OPTIONAL
        string gasPrice = "";
        // connects to user's browser wallet (metamask) to update contract state
        try
        {
            string response;
            if (AvailableNetworks.CurrentNetworkData.chain != "near")
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                response = await Web3GL.SendContract("bidFor", abi, contract, args, value, gasLimit, gasPrice);
#else
                response = await EVM.SendContract("bidFor", abi, contract, args, value, gasLimit, gasPrice);
#endif
                
            }
            else
            {
                response = await Web3GL.NearSendContract(AvailableNetworks.CurrentNetworkData.network, "bidFor", args, value);
            }

            Debug.Log(response);
            return response;
            
        } catch (Exception e) {
            Debug.LogException(e);
            return "fail";
        }
    }
}