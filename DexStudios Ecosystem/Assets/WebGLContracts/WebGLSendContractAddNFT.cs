using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameSystems.Scripts;
using Newtonsoft.Json;
using UnityEngine;
using Network = GameSystems.Scripts.Network;
using System.Numerics;
using GameSystems.Scripts.Constants;

public class WebGLSendContractAddNft : MonoBehaviour
{
    static async public UniTask<string> AddNFT(string motoDEXAdress, string args, string method)
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

        string[] listForSerialize = {ownerAccount, _tokenId};
        string tokenIdArgs = JsonConvert.SerializeObject(listForSerialize);
        
        // gas limit OPTIONAL
        string gasLimit = "";
        // gas price OPTIONAL
        string gasPrice = "";

        if (AvailableNetworks.CurrentNetworkData.chain == "octopus")
            gasLimit = "7920027";
        // connects to user's browser wallet (metamask) to update contract state
        try {
            Web3GL.GoogleAnalyticsSendEvent("add_nft_clicks");
            
            // string[] obj = {motoDEXAdress, _tokenId};
            // args = JsonConvert.SerializeObject(obj);
            
            string[] tokenIdsForSerialize = {_tokenId};
            string serializedTokenId = JsonConvert.SerializeObject(tokenIdsForSerialize);
            
            // string approve = await Web3GL.SendContract("approve", motoDEXnftAbi, motoDEXnftContract, args, "", gasLimit, gasPrice);
            // Debug.Log(approve);
            if (AvailableNetworks.CurrentNetworkData.chain != "near")
            {
                string valueInMainCoin = "0";
                if (AvailableNetworks.CurrentNetworkData.isEVMNotCompatible == false)
                {
                    string minimalFeeInUSD = await ContractHandler.MotodDexCallWithoutSerialize("minimalFeeInUSD",
                        motoDEXAbi, chain, network, motoDEXContract, "[]");
                    Debug.Log($"#debug #minimalFeeInUSD - {minimalFeeInUSD}");
                    valueInMainCoin =  await ContractHandler.MotodDexCall("valueInMainCoin", motoDEXAbi, chain,network, motoDEXContract, minimalFeeInUSD);; // here must be convert in minimalFee main coin for some chains
                    //valueInMainCoin = (int.Parse(valueInMainCoin) * 10e18).ToString();
                    Debug.Log($"#debug #valueInMainCoin - {valueInMainCoin}");
                }
                
#if UNITY_WEBGL && !UNITY_EDITOR
                string response = await Web3GL.SendContract(method, motoDEXAbi, motoDEXContract, serializedTokenId,
                    valueInMainCoin, gasLimit, gasPrice);
#else
                string response = await EVM.SendContract(method, motoDEXAbi, motoDEXContract, serializedTokenId,
                    valueInMainCoin, gasLimit, gasPrice);
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