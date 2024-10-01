using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameSystems.Scripts;
using GameSystems.Scripts.Constants;
using Newtonsoft.Json;
using UnityEngine;
using Network = GameSystems.Scripts.Network;


public class WebGLSendContractHealthApply : MonoBehaviour
{
    static async public UniTask<string> HealthApply(string motoDEXAdress, string _tokenId, string _healthPillTokenId)
    {
        string ownerAccount = "";
        string chain = Chain.InUse;
        string network = Network.InUse;
        string motoDEXnftContract = Contracts.MotoDEXnft;
        string motoDEXContract = Contracts.MotoDEX;

        string motoDEXnftAbi = Abis.MotoDEXnftABI;
        string motoDEXAbi = Abis.MotoDEXABI;

        if (PlayerPrefs.HasKey(PlayerPrefsKeys.Account))
        {
            ownerAccount = PlayerPrefs.GetString(PlayerPrefsKeys.Account);
            Debug.Log("ownerAccount: " + ownerAccount);
        }

        // gas limit OPTIONAL
        string gasLimit = "";
        // gas price OPTIONAL
        string gasPrice = "";
        string method;

        // connects to user's browser wallet (metamask) to update contract state
        try {
            
            string[] argsForSerialize = {_tokenId, _healthPillTokenId};
            string serializedArgs = JsonConvert.SerializeObject(argsForSerialize);
            if (AvailableNetworks.CurrentNetworkData.chain != "near")
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                string response = await Web3GL.SendContract(method="addHealthNFT", motoDEXnftAbi, motoDEXAdress, serializedArgs, "", gasLimit, gasPrice);
#else
                string response = await EVM.SendContract(method="addHealthNFT", motoDEXnftAbi, motoDEXAdress, serializedArgs, "", gasLimit, gasPrice);
#endif
                Debug.Log(response);
                return response;
            }
            else
            {
                string response = await Web3GL.NearSendContract(AvailableNetworks.CurrentNetworkData.network, method="addHealthNFT", serializedArgs, "1");
                Debug.Log(response);
                return response;
            }
        } catch (Exception e) {
            Debug.LogException(e);
            return "fail";
        }
    }
}