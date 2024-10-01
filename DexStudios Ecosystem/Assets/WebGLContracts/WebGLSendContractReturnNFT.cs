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


public class WebGLSendContractReturnNft : MonoBehaviour
{
    static async public UniTask<string> ReturnNft(string motoDEXAdress, string args, string method)
    {
        string ownerAccount = "";
        string chain = Chain.InUse;
        string network = Network.InUse;
        string _tokenId = args;
        string motoDEXnftContract = Contracts.MotoDEXnft;
        string motoDEXContract = Contracts.MotoDEX;

        string motoDEXnftAbi = Abis.MotoDEXnftABI;
        string motoDEXAbi = Abis.MotoDEXABI;

#if UNITY_EDITOR
        return "success";
#endif

        if (PlayerPrefs.HasKey(PlayerPrefsKeys.Account))
        {
            ownerAccount = PlayerPrefs.GetString(PlayerPrefsKeys.Account);
            Debug.Log("ownerAccount: " + ownerAccount);
        }

        // gas limit OPTIONAL
        string gasLimit = "";
        // gas price OPTIONAL
        string gasPrice = "";

        // connects to user's browser wallet (metamask) to update contract state
        try {
            Web3GL.GoogleAnalyticsSendEvent("return_nft_clicks");
            
            string[] tokenIdsForSerialize = {_tokenId};
            string serializedTokenId = JsonConvert.SerializeObject(tokenIdsForSerialize);

            if (AvailableNetworks.CurrentNetworkData.chain != "near")
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                string response = await Web3GL.SendContract(method, motoDEXAbi, motoDEXContract, serializedTokenId, "",
                    gasLimit, gasPrice);
                Debug.Log(response);
#else
                string response = await EVM.SendContract(method, motoDEXAbi, motoDEXContract, serializedTokenId, "",
                    gasLimit, gasPrice);
#endif
                Debug.Log(response);
                return response;
            }
            else
            {
                string response = await Web3GL.NearSendContract(AvailableNetworks.CurrentNetworkData.network, method, serializedTokenId, "");
                Debug.Log(response);
                return response;
            }
        } catch (Exception e) {
            Debug.LogException(e);
            return "fail";
        }
    }
}