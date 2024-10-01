using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Cysharp.Threading.Tasks;
using GameSystems.Scripts.Constants;
using GameSystems.Scripts.UI;
using Newtonsoft.Json;
using UnityEngine;

namespace GameSystems.Scripts.Services
{
    public class SetupContractsService: ISetupContractsService
    {
        private IBalanceService _balanceService;
        private ITokenService _tokenService;
        private IContractService _contractService;
        private IRequestSystem _requestSystem;

        public SetupContractsService(IBalanceService balanceService, ITokenService tokenService, IContractService contractService, IRequestSystem requestSystem)
        {
            _balanceService = balanceService;
            _tokenService = tokenService;
            _contractService = contractService;
            _requestSystem = requestSystem;
        }
        
        public async UniTask GetPrices()
        {
            if (_balanceService.IsGotCashedPrices)
                return;

            var network = AvailableNetworks.CurrentNetworkData;

            Debug.Log($"#debug #nft GetPrices chain {network.chain}"); 

            _balanceService.IsGotCashedPrices = true;

            Debug.Log($"#debug #nft Cash Prices Setuped {network.uriPathName}");

            try
            {
                var response = await _requestSystem.GetPricesRequest(network.uriPathName);
                var priceServer = response.Value;
                if(!response.Success)
                    Debug.LogError($"#debug #nft Error GetPrices from server: {response.Exception.Message}");

                UIManager.Instance.ChooseNft.SetupPriceServer(priceServer);
                UIManager.Instance.ChooseLevelInvested.SetupPriceServer(priceServer);
                UIManager.Instance.HealthTabNft.SetupPriceServer(priceServer);
            }
            catch(Exception ex)
            {
                Debug.LogError($"#debug #nft Error GetPrices from server: {ex.Message}");
            }

            Debug.Log("#debug #nft PrcesRequest Prices Setuped");
        }

        public async UniTask SetupAvailabeContracts()
        {
            await SetAddedContracts();

            UIManager.Instance.EnablePreLoadingSpinner();
        
        if (AvailableNetworks.CurrentNetworkData.chain != "near" &&
            AvailableNetworks.CurrentNetworkData.chain != "concordium" &&
            AvailableNetworks.CurrentNetworkData.chain != "sui" &&
            AvailableNetworks.CurrentNetworkData.chain != "sui_t" &&
            AvailableNetworks.CurrentNetworkData.chain != "icp" &&
            AvailableNetworks.CurrentNetworkData.chain != "icp_t")
        {
    #if !UNITY_IOS && !UNITY_ANDROID
            var balance = await _balanceService.RequestBalance();
            
            if (balance <= 0)
            {
                return;
            }
    #else
                var balance = await _requestSystem.GetBalance();
    #endif
    #if UNITY_WEBGL
                int[] limitsAndCounters =
     JsonConvert.DeserializeObject<int[]>(await ContractHandler.MotodDexCallWithoutSerialize("getLimitsAndCounters", Abis.MotoDEXnftABI, Chain.InUse, Network.InUse, Contracts.MotoDEXnft, "[]"));
                for (int i = 0; i < LimitedTypes.limitedTypes.Count; i++)
                {
                    var limitedTypeKey = LimitedTypes.limitedTypes.ElementAt(i).Key;
                    LimitedTypes.limitedTypes[limitedTypeKey] =
     limitsAndCounters[limitedTypeKey - 70] - limitsAndCounters[limitedTypeKey - 80];
                }
    #endif
        }
        
        
        if (AvailableNetworks.CurrentNetworkData.chain is "icp" or "icp_t")
        {
            var balance = await UIManager.Instance.ConnectChain.GetBalanceByContract();
            Debug.Log($"#debug: SetupAvailabeContracts PlayerPrefs.GetString(Account); " + PlayerPrefs.GetString(PlayerPrefsKeys.Account) + " balance " + balance);
            if (balance == 0)
            {
                string message = "Please top up your balance and re-enter the game. Wallet address was copied to clipboard. Your wallet is:";
            
                PopUpManager.Instance.PushPopUp("zero_balnce", "Balance error", message, PlayerPrefs.GetString(PlayerPrefsKeys.Account), false, PopupButton.ok, true);
            }
        }


        Debug.Log($"#debug: SetupAvailabeContracts PlayerPrefs.GetString(Account); " + PlayerPrefs.GetString(PlayerPrefsKeys.Account) + " chain " + AvailableNetworks.CurrentNetworkData.chain);

        Debug.Log($"#debug: SetupAvailabeContracts Invites.bid�ombination.Length {Invites.bidCombination.Length}");
        
        await SetDefaultContracts();
        }
        
        public async UniTask SetDefaultContracts()
        {
            var balance = 0.0;
        
            float minimumAllowableBalance = -1;
        
            balance = await _balanceService.RequestBalance();

#if UNITY_WEBGL
            Debug.Log($"#debug #contract SetDefaultContracts balanceService.Balance {_balanceService.Balance}");
            if(balance > minimumAllowableBalance)
            {
                Debug.Log($"#debug #contract SetDefaultContracts after request balanceService.Balance {_balanceService.Balance}");
             
                UIManager.Instance.BlockMainUIButtons();
                await _contractService.RequestContracts();

                SetupContracts();
                UIManager.Instance.UnBlockMainUIButtons();
            }
#else
        if (_balanceService.Balance > minimumAllowableBalance)
        {
            UIManager.Instance.BlockMainUIButtons();
            await _contractService.RequestContracts();
            SetupContracts();
            UIManager.Instance.UnBlockMainUIButtons();
        }
#endif

            Debug.Log($"#debug: SetDefaultContracts.DisablePreLoadingSpinner, contractsBalance");
            UIManager.Instance.DisablePreLoadingSpinner();
        }
        
        public async UniTask SetAddedContracts()
        {
            Debug.Log($"MainMenuService SetAddedContracts invoked");
            await _tokenService.RequestAddedToken();
            
            if (AvailableNetworks.CurrentNetworkData.chain 
                is "binance"   or "aurora" 
                or "mantle"    or "skale" 
                or "manta"     or "mint" 
                or "alephzero" or "eosevm" 
                or "q"         or "kroma" 
                or "viction"   or "zeta" 
                or "sui_t"     or "sui"      
                or "icp_t"     or "icp" 
                or "motodex")
            {
                Debug.Log(_tokenService.AddedToken.tokenId.Length);
                UIManager.Instance.PoapButton.gameObject.SetActive(true);
            }

            Debug.Log($"#debug Invites.tokenId != null {Invites.tokenId != null}");
            
            if (Invites.tokenId != null)
                AddedTokensRemoveEverytingExceptTokenIdAndOwned(Invites.tokenId);

            SetupContracts();
        }
        
        public void SetupContracts()
        {
            Debug.Log("#debug #addtoken LEVEL CONTRACTS: " + JsonConvert.SerializeObject(_contractService.Contracts));
            Debug.Log("#debug #addtoken LEVEL ADDED TOKENS: " + JsonConvert.SerializeObject(_tokenService.AddedToken));
            
            UIManager.Instance.HealthTabNft.Setup(_contractService.Contracts,_tokenService.AddedToken);
            UIManager.Instance.ChooseNft.Setup(_contractService.Contracts, _tokenService.AddedToken);
            UIManager.Instance.ChooseLevel.Setup(_contractService.Contracts, _tokenService.AddedToken);
            UIManager.Instance.ChooseLevelInvested.Setup(_contractService.Contracts, _tokenService.AddedToken);
            UIManager.Instance.ChooseLevelWallet.Setup(_contractService.Contracts, _tokenService.AddedToken);
        }
        
        public void AddedTokensRemoveEverytingExceptTokenIdAndOwned(string tokenId)
        {
            int length = _tokenService.AddedToken.tokenId.Length;
            List<string> tokenIdList = new List<string>();
            List<string> typeIdList = new List<string>();
            List<BigInteger> currHealthWeiList = new List<BigInteger>();
            List<BigInteger> maxHealthWeiList = new List<BigInteger>();
            List<int> maxHealthList = new List<int>();
            List<float> profitList = new List<float>();
            List<string> ownedTokensList = new List<string>();
            for (int i = 0; i < length; i++)
            {
                if (_tokenService.AddedToken.tokenId[i] == tokenId || _tokenService.AddedToken.ownedTokens[i] == "owned")
                {
                    tokenIdList.Add(_tokenService.AddedToken.tokenId[i]);
                    typeIdList.Add(_tokenService.AddedToken.typeId[i]);
                    currHealthWeiList.Add(_tokenService.AddedToken.currHealthWei[i]);
                    maxHealthWeiList.Add(_tokenService.AddedToken.maxHealthWei[i]);
                    maxHealthList.Add(_tokenService.AddedToken.maxHealth[i]);
                    profitList.Add(_tokenService.AddedToken.profit[i]);
                    ownedTokensList.Add(_tokenService.AddedToken.ownedTokens[i]);
                }
            }
            Debug.Log("ADDED TOCKENS");
            _tokenService.AddedToken.tokenId = tokenIdList.ToArray();
            _tokenService.AddedToken.typeId = typeIdList.ToArray();
            _tokenService.AddedToken.currHealthWei = currHealthWeiList.ToArray();
            _tokenService.AddedToken.maxHealthWei = maxHealthWeiList.ToArray();
            _tokenService.AddedToken.maxHealth = maxHealthList.ToArray();
            _tokenService.AddedToken.profit = profitList.ToArray();
            _tokenService.AddedToken.ownedTokens = ownedTokensList.ToArray();
        }
    }
}

public interface ISetupContractsService
{
    UniTask GetPrices();
    UniTask SetupAvailabeContracts();
    void SetupContracts();
}