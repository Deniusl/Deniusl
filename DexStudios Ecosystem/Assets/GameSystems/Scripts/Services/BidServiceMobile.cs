using System;
using System.Globalization;
using Cysharp.Threading.Tasks;
using GameSystems.Scripts.Services.Interfaces;
using Newtonsoft.Json;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.Scripts.Services
{
    public class BidServiceMobile: IBidService
    {
        public async UniTask Bid(string trackId, string motoId, 
                Action<bool> setTxStatusActive,
                Action<bool> setTxStatusButtonActive, 
                Action<bool> setBidButtonActive, 
                Action<bool> setInputFieldActive,
                Action setTxStatusButton,
                SelectLevelWindow selectLevelWindow, 
                TMP_InputField inputField, GameObject txStatus
            )
        {
            if (inputField.text == "") return;

            string value = inputField.text;
            value = ChainRecalculate(value);
            selectLevelWindow.DisableNFTInteractbale();

            setTxStatusButton();
            
            setTxStatusActive(true);
            setTxStatusButtonActive(true);
            setBidButtonActive(false);
            setInputFieldActive(false);
            
            string[] obj = {trackId, motoId};
            var args = JsonConvert.SerializeObject(obj);
            string response = "";
            

            if (AvailableNetworks.CurrentNetworkData.chain != "near")
                response = await WebGLSendContractBid.Bid(Contracts.MotoDEX, args, value);
            // else
                // response = await AndroidNear.Change("bidFor", args, value, confirmationMenu:_selectLevelWindow._confirmationMenu);

            await txStatus.GetComponent<TxStatus>().SetStatus(Chain.InUse, Network.InUse, response, TxStatusType.BID);
            
            selectLevelWindow.EnableNFTInteractbale();
            Debug.Log(response);
        }

        public string SetBidStatus(string txStatus, NftItemComponent nftItemComponent, NFTBid NFTBid)
        {
            if (nftItemComponent != null) nftItemComponent.IdleChoiceBorder.SetActive(true);

            if (NFTBid != null && NFTBid.TxStatusButton == NFTBid.bidButton)
            {
                Debug.Log($"#debug #txstatus #bid");

                NFTBid.bidButton.gameObject.SetActive(true);
                if (txStatus == "success")
                {
                    NFTBid._totalBids.text =
                        (double.Parse(NFTBid._totalBids.text, NumberStyles.Any, CultureInfo.InvariantCulture) +
                         double.Parse(NFTBid.inputField.text)).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
                    NFTBid.SetPlayerBid(NFTBid.playerBidValue + double.Parse(NFTBid.inputField.text));
                }

                NFTBid.inputField.text = "";
                NFTBid.inputField.gameObject.SetActive(true);
                if (nftItemComponent != null)
                    nftItemComponent.checkMark.gameObject.SetActive(false);
                return txStatus;
            }

            return "pending";
        }

        private string ChainRecalculate(string value)
        {
            var currentChain = AvailableNetworks.CurrentNetworkData.chain;
            
            double tempValue = double.Parse(value);
            if (currentChain == "near")
            {
                tempValue *= Const.Wallet.NEARToHuman;
            }
            else if (currentChain == "concordium" || currentChain == "skale")
            {
                tempValue *= Const.Wallet.CCDToHuman;
            }
            else
            {
                tempValue *= Const.Wallet.BNBToHuman;
            }

            value = tempValue.ToString("0");
            return value;
        }
    }
}