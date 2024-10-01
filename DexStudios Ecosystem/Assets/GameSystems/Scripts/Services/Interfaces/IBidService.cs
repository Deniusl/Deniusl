using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UI;
using UnityEngine;

namespace GameSystems.Scripts.Services.Interfaces
{
    public interface IBidService
    {
        UniTask Bid(string trackId, string motoId, 
                Action<bool> setTxStatusActive,
                Action<bool> setTxStatusButtonActive, 
                Action<bool> setBidButtonActive, 
                Action<bool> setInputFieldActive,
                Action setTxStatusButton,
                SelectLevelWindow selectLevelWindow, 
                TMP_InputField inputField, GameObject txStatus);
        
        
        string SetBidStatus(string txStatus, NftItemComponent nftItemComponent, NFTBid NFTBid);
    }
}