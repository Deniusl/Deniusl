using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Cysharp.Threading.Tasks;
using GameSystems.Scripts;
using UnityEngine;
using Types = GameSystems.Scripts.Types;

namespace UI
{
    public class ChooseNftComponent : MonoBehaviour
    {
        [SerializeField] 
        private NftItemComponent[] _nftItemComponents;

        public IEnumerable<NftItemComponent> NftItemComponents => _nftItemComponents;

        private (string id, bool isOwner)? _invitationalNft;

        
    public void Setup(Contract[] contracts, AddedToken _addedTokens = null)
    {
        ResetAll();
        
        for (int i = 0; i < contracts?.Length; i++)
        {
            foreach (var nftItemComponent in _nftItemComponents)
            {
                if (contracts[i].ItemData?.type == 106 && contracts[i].contract != "free_contract")
                {
                    Debug.Log(111111111111111111);
                    Debug.Log(i);
                    continue;
                }
                Debug.Log(111111111111111111);
                Debug.Log(contracts[i].ItemData?.type);
                Debug.Log(contracts[i].contract);
                
                if (nftItemComponent.Type == contracts[i].ItemData?.type)
                {
                    nftItemComponent.Setup(contracts[i]);
                    nftItemComponent.SetAvailable(true);
                }
            }
        }
        
        #if UNITY_WEBGL || UNITY_EDITOR
        for (int i = 0; i < _addedTokens?.typeId?.Length; i++)
        {
            foreach (var nftItemComponent in _nftItemComponents)
            {
                if (nftItemComponent.Type == 106) continue;
                
                if (nftItemComponent.Type.ToString() == _addedTokens.typeId[i])
                {
                    nftItemComponent._selectIsActive = true;
                    nftItemComponent.SetupForAdded(_addedTokens, i);
                    nftItemComponent.SetAvailable(true);
                }
            }
        }
        #endif
        
        SortAndSetSiblingIndices();

        if (_invitationalNft.HasValue)
            SetupInvitationalComponents();
    }

    private void ResetAll()
    {
        foreach (var nftItemComponent in _nftItemComponents)
        {
            if (nftItemComponent != null) 
                nftItemComponent.Reset();
        }
    }
    
    private void SortAndSetSiblingIndices()
    {
        var motoComponents = _nftItemComponents.Where(x => x.TypeEnum.IsMoto()).ToList();
        var pillComponents = _nftItemComponents.Where(x => x.TypeEnum.IsPill()).ToList();
        var trackComponents = _nftItemComponents.Where(x => x.TypeEnum.IsTrack()).ToList();

        var sortedMotoComponents = SortComponents(motoComponents);
        var sortedPillComponents = SortComponents(pillComponents);
        var sortedTrackComponents = SortComponents(trackComponents);

        var sortedComponents = sortedMotoComponents
            .Concat(sortedPillComponents)
            .Concat(sortedTrackComponents)
            .ToList();

        for (int i = 0; i < sortedComponents.Count; i++)
        {
            sortedComponents[i].transform.SetSiblingIndex(i);
        }
    }

    private List<NftItemComponent> SortComponents(List<NftItemComponent> components)
    {
        return components
            .OrderBy(x => x.PriceInCoins == 0 ? 0 : 1) // Price 0 goes first
            .ThenByDescending(x => x.Contracts?.Count > 0) // Then those with contracts
            .ThenByDescending(x => x.Contracts?.Count > 0 ? x.PriceInCoins : int.MinValue) // Sort by price high to low for those with contracts
            .ThenBy(x => x.PriceInCoins ?? int.MaxValue) // Sort remaining by price low to high
            .ToList();
    }


        private void SetupInvitationalComponents()
        {
            var pair = _invitationalNft.Value;
            
            if (pair.isOwner)
            {
                SetActiveComponents(false);
                SetActiveComponent(pair.id, true);
            }
            else
            {
                SetComponentsNoOwner(pair.id);
            }
        }

        public async UniTask SetPriceForUnExistContracts(Contract[] items, IPriceCash priceCash)
        {
            var itemList = items == null ? new List<Contract>() : items.ToList();

            var dontExistContractsForNft = _nftItemComponents.Where(x => itemList.Exists(y => y.ItemData?.type == x.Type) is false);
            
            Debug.Log($"#debug #nft #price SetPriceForUnExistContracts count {dontExistContractsForNft.Count()}");
            
            foreach(var nftComponent in dontExistContractsForNft)
            {
                nftComponent.EnablePreLoadingSpinner();
                nftComponent.SetInteractable(false);

                try
                {
                    var priceFromCash = priceCash.GetCashedPrice(nftComponent.Type.ToString());
                    if (priceFromCash.exist)
                    {
                        nftComponent.SetupPriceServer(priceFromCash.value);
                    }
                    // else
                    // {
                    //     var priceResponse = AvailableNetworks.currNetworkData.isPricesInUSD ? await ContractHandler.MotodDexCall("getPriceForType", Abis.MotoDEXnftABI, Chain.InUse, GameSystems.Scripts.Network.InUse, AvailableNetworks.currNetworkData.contracts[0], nftComponent.Type.ToString()) 
                    //         : await ContractHandler.MotodDexCall("valueInMainCoin", Abis.MotoDEXnftABI, Chain.InUse, GameSystems.Scripts.Network.InUse, AvailableNetworks.currNetworkData.contracts[0], nftComponent.Type.ToString());
                    
                    //     Debug.Log($"#debug #nft #price SetPriceForUnExistContracts isPricesInUSD {AvailableNetworks.currNetworkData.isPricesInUSD} response: {priceResponse}");

                    //     nftComponent.SetupPrice(priceResponse);
                    //     nftComponent.SetName(nftComponent.TypeEnum.ToString());
                    // }

                }
                catch (Exception ex)
                {
                    Debug.LogError($"#debug #nft #price SetPriceForUnExistContracts for type {nftComponent.Type} exception {ex}");
                }
                finally
                {
                    nftComponent.DisablePreLoadingSpinner();
                    nftComponent.SetInteractable(true);
                }
            }
        }

        public void SetupCashedPrices(List<CashPriceData> prices)
        {
            foreach (var price in prices) foreach (var nftItemComponent in _nftItemComponents)
            {
                if (nftItemComponent.Type.ToString() == price.TokenId)
                {
                    Debug.Log($"#debug #nft #price SetupCashedPrices price.Response {price.Response}");

                    nftItemComponent.SetupPrice(price.Response);
                    nftItemComponent.SetName(nftItemComponent.TypeEnum.ToString());
                }
            }
        }

        public void SetupPriceServer(List<PriceServer> prices)
        {
            foreach (var nftItemComponent in _nftItemComponents)
            {
                nftItemComponent.SetupPriceServerFromList(prices);
            }
        }

        public void ActiveSelectWitchCheck(bool value)
        {
            foreach (var nftItemComponent in _nftItemComponents)
            {
                nftItemComponent.ActiveSelectWitchCheck(value);
            }
        }

        public void ActiveAllExceptSelectWithCheck(bool value)
        {
            foreach (var nftItemComponent in _nftItemComponents)
            {
                nftItemComponent.ActiveAllExceptSelectWithCheck(value);
            }
        }

        public void SetInteractable(bool isInteractable)
        {
            foreach(var nftItemComponent in _nftItemComponents)
            {
                nftItemComponent.SetInteractable(isInteractable);
            }
        }

        public void SetInteractable(bool value, NftItemComponent component)
        {
            component.SetInteractable(value);
        }

        public void Reselect(int type)
        {
            foreach (var nftItemComponent in _nftItemComponents)
            {
                if (nftItemComponent.Type == type)
                {
                    nftItemComponent.checkMark.SetActive(true);
                    nftItemComponent.checkMarkOutline.enabled = true;
                }
                else
                {
                    nftItemComponent.checkMark.SetActive(false);
                    nftItemComponent.checkMarkOutline.enabled = false;
                }
            }
        }
        
        public void Unselect()
        {
            foreach (var nftItemComponent in _nftItemComponents)
            {
                nftItemComponent.checkMark.SetActive(false);
                nftItemComponent.checkMarkOutline.enabled = false;
            }
        }

        public BigInteger ReturnCurHealthWei(string tokenId)
        {
            foreach (var nftItemComponent in _nftItemComponents)
            {
                Debug.Log($"debug #health ReturnCurHealthWei tokenId {nftItemComponent._tokenId} == {tokenId} {nftItemComponent._tokenId == tokenId} nftItemComponent._currHealthWei {nftItemComponent._currHealthWei}");
                if (nftItemComponent._tokenId == tokenId)
                {
                    return nftItemComponent._currHealthWei;
                }
            }
            Debug.Log($"debug #health ReturnCurHealthWei not found for tokenId {tokenId} reuturn 0");
            return 0;
        }
        
        public BigInteger ReturnMaxHealthWei(string tokenId)
        {
            foreach (var nftItemComponent in _nftItemComponents)
            {
                Debug.Log($"debug #health ReturnMaxHealthWei tokenId {nftItemComponent._tokenId} == {tokenId} {nftItemComponent._tokenId == tokenId} nftItemComponent._maxHealthWei {nftItemComponent._maxHealthWei}");
                if (nftItemComponent._tokenId == tokenId)
                {
                    return nftItemComponent._maxHealthWei;
                }
            }
            Debug.Log($"debug #health ReturnMaxHealthWei not found for tokenId {tokenId} return 0");
            return 0;
        }

        public BigInteger ReturnMaxHealthWeiInMainCoin(string tokenId)
        {
            foreach (var nftItemComponent in _nftItemComponents)
            {
                Debug.Log($"debug #health ReturnMaxHealthWeiInMainCoin tokenId {nftItemComponent._tokenId} == {tokenId} {nftItemComponent._tokenId == tokenId} nftItemComponent._maxHealthWei {nftItemComponent._maxHealthWeiInMainCoin}");
                if (nftItemComponent._tokenId == tokenId)
                {
                    return nftItemComponent._maxHealthWeiInMainCoin;
                }
            }
            Debug.Log($"debug #health ReturnMaxHealthWeiInMainCoin not found for tokenId {tokenId} return 0");
            return 0;
        }
        
        public int ReturnMaxHealth(string tokenId)
        {
            foreach (var nftItemComponent in _nftItemComponents)
            {
                Debug.Log($"debug #health ReturnMaxHealth tokenId {nftItemComponent._tokenId} == {tokenId} {nftItemComponent._tokenId == tokenId} nftItemComponent._maxHealth {nftItemComponent._maxHealth}");

                if (nftItemComponent._tokenId == tokenId)
                {
                    return nftItemComponent._maxHealth;
                }
            }
            Debug.Log($"debug #health ReturnMaxHealth tokenId  not found for tokenId {tokenId} return 0");
            return 0;
        }
        
        public ChooseNftComponent SubscribeBuyButtonClick(Action<NftItemAction> onClick)
        {
            foreach (var nftItemComponent in _nftItemComponents)
            {
                nftItemComponent.SubscribeBuyButton(onClick);
            }
            return this;
        }

        public ChooseNftComponent SubscribeAddButtonClick(Action<NftItemAction> onClick)
        {
            foreach (var nftItemComponent in _nftItemComponents)
            {
                nftItemComponent.SubscribeAddButton(onClick);
            }
            return this;
        }

        public ChooseNftComponent SubscribeReturnButtonClick(Action<NftItemAction> onClick)
        {
            foreach (var nftItemComponent in _nftItemComponents)
            {
                nftItemComponent.SubscribeReturnButton(onClick);
            }
            return this;
        }

        public ChooseNftComponent SubscribeChooseButtonClick(Action<NftItemAction> onClick)
        {
            foreach (var nftItemComponent in _nftItemComponents)
            {
                nftItemComponent.SubscribeChooseButton(onClick);
            }
            return this;
        }

        public ChooseNftComponent SubscribeAddHealthButtonClick(Action<NftItemAction> onClick)
        {
            foreach (var nftItemComponent in _nftItemComponents)
            {
                nftItemComponent.SubscribeAddHealthButton(onClick);
            }
            return this;
        }

        public ChooseNftComponent SubscribeInviteButtonClick(Action<NftItemAction> onClick)
        {
            foreach (var nftItemComponent in _nftItemComponents)
            {
                nftItemComponent.SubscribeInviteButton(onClick);
            }
            return this;
        }

        public void ItemsRemoveListener()
        {
            foreach (var nftItemComponent in _nftItemComponents)
            {
                nftItemComponent.RemoveAllListeners();
            }
        }
        
        public void EnablePreLoadingSpinner()
        {
            foreach (var nftItemComponent in _nftItemComponents)
            {
                nftItemComponent.EnablePreLoadingSpinner();
            }
        }
        
        public void DisablePreLoadingSpinner()
        {
            foreach (var nftItemComponent in _nftItemComponents)
            {
                nftItemComponent.DisablePreLoadingSpinner();
            }
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetActiveElements(bool active)
        {
            foreach(var nftItemComponent in _nftItemComponents)
            {
                nftItemComponent.SetActiveElements(active);
            }
        }

        public void SetActiveComponents(bool isActive)
        {
            foreach( var nftItemComponent in _nftItemComponents)
            {
                nftItemComponent.gameObject.SetActive(isActive);
            }
        }

        public void SetActiveComponent(string id, bool isActive)
        {
            foreach(var nftItemComponent in _nftItemComponents)
            {
                if (nftItemComponent._tokenId != id) continue;
                nftItemComponent.gameObject.SetActive(isActive);
                break;
            }
        }

        public void SetInvitationalComponent(string id, bool isOwner)
        {
            _invitationalNft = (id, isOwner);
        }

        public void SetComponentIndex(string id, int index)
        {
            foreach (var nftItemComponent in _nftItemComponents)
            {
                if (nftItemComponent._tokenId != id) continue;
                nftItemComponent.transform.SetSiblingIndex(index);
                break;
            }
        }
        
        public void SetComponentsNoOwner( string id)
        {
            foreach (var nftItemComponent in _nftItemComponents)
            {
                nftItemComponent.SetupAndOrderNFTs(id);
            }
        }
    }
}