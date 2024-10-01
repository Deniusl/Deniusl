using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using BuyFlow;
using Cysharp.Threading.Tasks;
using GameSystems.Scripts;
using TMPro;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.UI;
using DIContainerLib;
using GameSystems.Scripts.Services;
using Services;
using Assets.GameSystems.Scripts.UI;
using Assets.GameSystems.Scripts.Utils;
using BuyFlow.IAP;
using GameSystems.Scripts.Constants;
using GameSystems.Scripts.Providers;
using GameSystems.Scripts.Services.Interfaces;
using GameSystems.Scripts.Services.Providers;

#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace UI
{
    public class SelectLevelWindow : MonoBehaviour
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")] private static extern void ClearLocalStorage();
        [DllImport("__Internal")] private static extern void ClearAllCookies();
#endif
        [SerializeField] private ConnectChain connectChain;
        [SerializeField] private ConnectWalletPanel connectWalletPanel;
        [SerializeField] private DisconnectWalletPanel disconnectWalletPanel;
        [SerializeField] private MainMenuController _mainMenuController;
        [SerializeField] private RequestSystem _requestSystem;

        [SerializeField] private ChooseNftComponent _healthTabNft;
        [SerializeField] private ChooseNftComponent _chooseNft;
        [SerializeField] private ChooseNftComponent _chooseLevel;
        [SerializeField] private ChooseNftComponent _chooseLevelWallet;
        [SerializeField] private ChooseNftComponent _chooseLevelInvested;
        [SerializeField] private SetupNftBetting _setupNftBetting;
        [SerializeField] private SelectingStatusNew selectingMarksPlay;
        [SerializeField] private SelectingStatus selectingMarksShare;
        [SerializeField] private HealthTab _healthTab;

        [SerializeField] private Button[] menuBtns;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _tutorialMobile;
        [SerializeField] private Button _selectedResetButton;
        [SerializeField] private Button shareButton;
        [SerializeField] private Button _tutorialWebButton;
        [SerializeField] private Button poapButton;
        [SerializeField] private Button _supportButton;
        [SerializeField] private TMP_Text _balanceMobileView;
        [SerializeField] private TMP_Text txtVersion;
        [SerializeField] private GameObject _walletConnectedGroup;
        [SerializeField] private GameObject _nftComponentsTab;
        [SerializeField] private GameObject _betting;
        [SerializeField] private GameObject _tabButtons;
        [SerializeField] private TabViewContainer _tabViewContainerMobile;
        [SerializeField] private TabViewContainer _tabViewContainer;
        [SerializeField] private TrackShareWindow _trackShareWindow;
        [SerializeField] private CustomDropdown dropdown;

        public IAddHealthService AddHealthService { get; private set; }
        public ChooseNftComponent MotoChooseNftComponent => _chooseNft;
        public Button SelectResetButton => _selectedResetButton;
        public BalanceMobileService BalanceMobileService => _balanceMobileService;
        public PurchaseService PurchaseService { get; private set; }

        private IMainMenuService _mainMenuService;
        private IGetUniqueIDService _getUniqueIDService;
        private BalanceMobileService _balanceMobileService;

        private void GetAllPrices() => PurchaseService.GetAllPrices();

        public string GetAccount
        {
            get
            {
#if UNITY_ANDROID || UNITY_EDITOR
                return _getUniqueIDService.UniqueID;
#elif UNITY_IOS
                return _getUniqueIDService.UniqueID;
#elif UNITY_WEBGL
                return connectChain.account;
#endif
            }
        }

        public async UniTask Init()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            HandleCookies();
#endif
            PurchaseService = await InitializePurchaseService();
            var diServiceCollection = new DIServiceCollection();

            await RegisterServices(diServiceCollection);
            await InitializeMainMenuService(diServiceCollection);
            
            HandleSupportActions();
            HandleNetworkSelection();
        }

        private async UniTask<PurchaseService> InitializePurchaseService()
        {
            PurchaseService purchaseService = new PurchaseService();
#if UNITY_IOS || UNITY_ANDROID
            const string environment = "sandbox";
            Debug.Log("Init purchase service before unity InitializeAsync");
            var options = new InitializationOptions().SetEnvironmentName(environment);
            await UnityServices.InitializeAsync(options);
            Debug.Log("Init purchase service after unity InitializeAsync");
    
            purchaseService.Init();
#endif
            return purchaseService;
        }

        private async UniTask RegisterPlatformSpecificServices(DIServiceCollection diServiceCollection)
        {
#if UNITY_ANDROID || UNITY_EDITOR || UNITY_WEBGL
            await RegisterAndroidOrEditorOrWebGLServices(diServiceCollection);
#elif UNITY_IOS
            await RegisterIOSServices(diServiceCollection);
#endif
        }

        private async UniTask RegisterAndroidOrEditorOrWebGLServices(DIServiceCollection diServiceCollection)
        {
            _getUniqueIDService = new UDIDService();
            await _getUniqueIDService.GetUniqueID();
            diServiceCollection.RegisterSingleton<IGetUniqueIDService, UDIDService>(_getUniqueIDService);
        }

        private async UniTask RegisterIOSServices(DIServiceCollection diServiceCollection)
        {
            _getUniqueIDService = new IDFAService();
            await _getUniqueIDService.GetUniqueID();
            diServiceCollection.RegisterSingleton<IGetUniqueIDService, IDFAService>(_getUniqueIDService);
        }

        private void RegisterCommonServices(DIServiceCollection diServiceCollection)
        {
            diServiceCollection.RegisterSingleton<IPurchaseConsumeService, ConsumePurchaseService>();
            diServiceCollection.RegisterSingleton<ILevelService, LevelService>();
            diServiceCollection.RegisterSingleton<IPlayerService, PlayerService>();
            diServiceCollection.RegisterSingleton<IPurchaseService, PurchaseService>(PurchaseService);
            diServiceCollection.RegisterSingleton<IAddHealthService, AddHealthService>(
                new AddHealthService(_healthTab, _healthTabNft, GetTabPerPlatform(), _tabButtons, selectingMarksPlay,
                    _tutorialWebButton));
            diServiceCollection.RegisterSingleton<IRequestSystem, RequestSystem>(_requestSystem);
            diServiceCollection.RegisterSingleton<ConnectChain>(connectChain);
            diServiceCollection.RegisterSingleton<IPurchaseIAPService, PurchaseIAPService>();
            diServiceCollection.RegisterSingleton<ITokenService, TokenService>();
            diServiceCollection.RegisterSingleton<ISetupContractsService, SetupContractsService>();
            diServiceCollection.RegisterSingleton<IContractService, ContractService>();
            diServiceCollection.RegisterSingleton<IPoapService, POAPService>();
            diServiceCollection.RegisterSingleton<IConnectionService, ConnectionService>();
            diServiceCollection.RegisterSingleton<INetworkService, NetworkService>();
            diServiceCollection.RegisterSingleton<IMainMenuService, MainMenuService>();
        }

        private void RegisterPlatformSpecificPurchaseServices(DIServiceCollection diServiceCollection)
        {
#if UNITY_EDITOR || UNITY_WEBGL
            diServiceCollection.RegisterSingleton<IPurchaseable, EditorPurchase>();
#elif UNITY_IOS
            diServiceCollection.RegisterSingleton<IPurchaseable, IOSPurchase>();
#elif UNITY_ANDROID
            diServiceCollection.RegisterSingleton<IPurchaseable, AndroidPurchase>();
#endif
        }

        private void RegisterPlatformSpecificAnalyticsServices(DIServiceCollection diServiceCollection)
        {
#if UNITY_IOS || UNITY_ANDROID
            RegisterMobileAnalyticsServices(diServiceCollection);
#elif UNITY_WEBGL
            RegisterWebGLAnalyticsServices(diServiceCollection);
#endif
        }

        private void RegisterMobileAnalyticsServices(DIServiceCollection diServiceCollection)
        {
            _balanceMobileService = new BalanceMobileService(_balanceMobileView);
            diServiceCollection.RegisterSingleton<IBalanceService, BalanceMobileService>(_balanceMobileService);
            diServiceCollection.RegisterSingleton<IAnalyticsService, MobileAnalyticsService>();
            diServiceCollection.RegisterSingleton<IBuyNFTService, BuyMobileService>();
            diServiceCollection.RegisterSingleton<IAddNFTService, AddMobileService>();
            diServiceCollection.RegisterSingleton<IReturnNFTService, ReturnMobileService>();
            diServiceCollection.RegisterSingleton<IApplyHealthNFTService, ApplyHealthNFTMobile>();
            diServiceCollection.RegisterSingleton<IBidService, BidServiceMobile>();
        }

        private void RegisterWebGLAnalyticsServices(DIServiceCollection diServiceCollection)
        {
            diServiceCollection.RegisterSingleton<IBalanceService, BalanceWebService>(
                new BalanceWebService(disconnectWalletPanel, _requestSystem));
            diServiceCollection.RegisterSingleton<IAnalyticsService, WebAnalyticsService>();
            diServiceCollection.RegisterSingleton<IBuyNFTService, BuyWebService>();
            diServiceCollection.RegisterSingleton<IAddNFTService, AddWebService>();
            diServiceCollection.RegisterSingleton<IReturnNFTService, ReturnWebService>();
            diServiceCollection.RegisterSingleton<IApplyHealthNFTService, ApplyHealthNFTWebService>();
            diServiceCollection.RegisterSingleton<IBidService, BidServiceWeb>();
            diServiceCollection.RegisterSingleton<IWebGLHandler, WebGLHandler>();
        }

        private async UniTask RegisterServices(DIServiceCollection diServiceCollection)
        {
            await RegisterPlatformSpecificServices(diServiceCollection);
            RegisterCommonServices(diServiceCollection);
            RegisterPlatformSpecificPurchaseServices(diServiceCollection);
            RegisterPlatformSpecificAnalyticsServices(diServiceCollection);
        }

        private async UniTask InitializeMainMenuService(DIServiceCollection diServiceCollection)
        {
            await InitializeServices(diServiceCollection);
            AddComponentsToMainMenuService();
            AddButtonsToMainMenuService();
            AddOthersToMainMenuService();
            _mainMenuService.Init();
        }

        private async UniTask InitializeServices(DIServiceCollection diServiceCollection)
        {
            var container = diServiceCollection.GenerateContainer();
            _mainMenuService = container.GetService<IMainMenuService>();
            AddHealthService = container.GetService<IAddHealthService>();
            LevelServiceProvider.LevelService = container.GetService<ILevelService>();
            BidServiceProvider.BidService = container.GetService<IBidService>();
            container.GetService<IAnalyticsService>().Init();
        }

        private void AddComponentsToMainMenuService()
        {
            _mainMenuService.AddNftComponents(_healthTabNft, _chooseNft, _chooseLevel, _chooseLevelInvested,
                _chooseLevelWallet);
            _mainMenuService.AddComponents(GetTabPerPlatform(), dropdown, _healthTab, _setupNftBetting,
                connectWalletPanel, disconnectWalletPanel, _mainMenuController, selectingMarksPlay, selectingMarksShare,
                _trackShareWindow);
        }

        private void AddButtonsToMainMenuService()
        {
            _mainMenuService.AddButtons(menuBtns, _selectedResetButton, GetPlatformTutorialButton(), _playButton,
                shareButton, poapButton);
        }

        private void AddOthersToMainMenuService()
        {
            _mainMenuService.AddOthers(txtVersion, _tabButtons);
        }

        private void HandleSupportActions()
        {
#if UNITY_IOS || UNITY_ANDROID
            SupportActions(_getUniqueIDService.UniqueID);
#elif UNITY_WEBGL
            SupportActions(connectChain.account);
#endif
        }

        private void HandleNetworkSelection()
        {
#if UNITY_IOS || UNITY_ANDROID
            HandleMobileNetworkSelection();
#elif UNITY_WEBGL
            HandleWebGLNetworkSelection();
#endif
        }

        private void HandleMobileNetworkSelection()
        {
            var allnetworks = AvailableNetworks.networksData.ToList();
            _mainMenuService.SelectNetwork(allnetworks.IndexOf(allnetworks.First(x => x.chain == "skale")));
            _mainMenuService.OnConnected();
        }

        private void HandleWebGLNetworkSelection()
        {
            var url = GetApplicationUrl();
            Debug.Log($"SelectLevelWindow Init url is {url}");
            var validator = new URLValidator();
            var success = validator.Validate(url);

            if (success) HandleSuccessfulValidation(validator);
            else HandleFailedValidation();

            SetInteractableElements(true);
        }

        private string GetApplicationUrl()
        {
#if UNITY_EDITOR
            return "https://app.motodex.dexstudios.games";
#else
            return Application.absoluteURL;
#endif
        }

        private void HandleSuccessfulValidation(URLValidator validator)
        {
            if (validator.InvitationParsedData != null)
            {
                Debug.Log($"#debug #parsed #data - {validator.InvitationParsedData.Network}");
                Debug.Log($"#debug #parsed #data - {validator.InvitationParsedData.TokenId}");
                Debug.Log($"#debug #parsed #data - {validator.InvitationParsedData.IsOwner}");
                SetupInvitationalNetwork(validator);
                HandleInvitedTrack(validator);
            }
        }

        private void HandleFailedValidation()
        {
            bool isNetworkChosenBefore = PlayerPrefs.HasKey(PlayerPrefsKeys.NetworkIndex);
            if (isNetworkChosenBefore &&
                int.TryParse(PlayerPrefs.GetString(PlayerPrefsKeys.NetworkIndex), out var networkId))
            {
                _mainMenuService.SelectNetwork(networkId);
            }
            else
            {
                _mainMenuService.SelectNetwork(dropdown.SelectedItem.Index);
            }

            if (isNetworkChosenBefore)
                _mainMenuService.GetPrices().Forget();
        }

        private void SetInteractableElements(bool interactable)
        {
            _chooseNft.SetInteractable(interactable);
            _chooseLevel.SetInteractable(interactable);
        }


        private void HandleInvitedTrack(URLValidator validator)
        {
            var invitationData = validator.InvitationParsedData;
            if (invitationData != null)
            {
                var allNetworks = AvailableNetworks.networksData.ToList();
                int networkIndex = allNetworks.FindIndex(x =>
                    x.chain.Equals(invitationData.Network, StringComparison.OrdinalIgnoreCase));
                if (networkIndex != -1)
                {
                    _mainMenuService.SelectNetwork(networkIndex);
                }
                else
                {
                    Debug.LogWarning($"Network {invitationData.Network} not found in available networks.");
                }

                SetupInvitationalComponent(invitationData);
            }
        }

        private void AddCoins() => _balanceMobileService.Balance += 1000;

        private void SetupInvitationalNetwork(URLValidator validator)
        {
            var invitationData = validator.InvitationParsedData;
            if (invitationData != null)
            {
                var network = AvailableNetworks.allNetworksData.FirstOrDefault(n =>
                    n.chain.Equals(invitationData.Network, StringComparison.OrdinalIgnoreCase));
                if (network != null)
                {
                    var networkIdx = Array.IndexOf(AvailableNetworks.allNetworksData, network);
                    _mainMenuService.ForceLogin(networkIdx);
                }
                else
                {
                    Debug.LogError($"Network {invitationData.Network} not found in available networks.");
                }
            }
        }

        private void HandleCookies()
        {
            if (Application.version != PlayerPrefs.GetString(PlayerPrefsKeys.Version, ""))
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                ClearLocalStorage();
                ClearAllCookies();
#endif
                PlayerPrefs.DeleteKey(PlayerPrefsKeys.NetworkIndex);

                PlayerPrefs.SetString(PlayerPrefsKeys.Version, Application.version);
            }
        }

        private void SetupInvitationalComponent(InvitationData invitationData)
        {
            _chooseLevel.SetInvitationalComponent(invitationData.TokenId, invitationData.IsOwner);
        }

        private void SupportActions(string id)
        {
            _supportButton.onClick.AddListener(() =>
            {
                string email = "margarita@openbisea.com";
                string subject = EscapeURL("motoDEX support need");
                string body = EscapeURL($"Hi. I'm user with advertisementIdentifier {id}\nPlease help me in case");

                Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
            });
        }

        private string EscapeURL(string url)
        {
            return WWW.EscapeURL(url).Replace("+", "%20");
        }

        private Button GetPlatformTutorialButton()
        {
#if UNITY_IOS || UNITY_ANDROID
            return _tutorialMobile;
#elif UNITY_WEBGL
            return _tutorialWebButton;
#endif
        }


        private TabViewContainer GetTabPerPlatform()
        {
#if UNITY_IOS || UNITY_ANDROID
            return _tabViewContainerMobile;
#elif UNITY_WEBGL
            return _tabViewContainer;
#endif
        }

        public void ShowFullBalance()
        {
            _balanceMobileService?.ShowFull();
        }

        public void ResetBalance()
        {
            _balanceMobileService?.Reset();
        }

        public void ApplyHealthClicked(NftItemAction itemAction)
        {
            _mainMenuService.ApplyHealthClicked(itemAction);
        }

        public void EnableNFTInteractbale()
        {
            _mainMenuService.EnableNFTInteractbale();
        }

        public void DisableNFTInteractbale()
        {
            _mainMenuService.DisableNFTInteractbale();
        }

        public void EnablePreLoadingSpinner()
        {
            _chooseNft.EnablePreLoadingSpinner();
            _chooseLevel.EnablePreLoadingSpinner();
        }

        [Obsolete("ADD iOS, Move To Requests Service")]
        public async void HealthPurchaseClicked()
        {
            _mainMenuService.HealthPurchaseClicked();
        }

        public void OpenHealthBar(NftItemAction itemAction)
        {
            _mainMenuService.OpenHealthBar(itemAction);
        }

        public void CloseHealthBar()
        {
            _mainMenuService.CloseHealthBar();
        }

        public void HealthBarSwapToChanged()
        {
            _mainMenuService.HealthBarSwapToChanged();
        }

        public void HealthBarAnimationStatus(bool active)
        {
            _mainMenuService.HealthBarAnimationStatus(active);
        }

        public async void UpdateHealthBarPurchaseHealth()
        {
            _mainMenuService.UpdateHealthBarPurchaseHealth();
        }

        [Obsolete(
            "Make update only one nft component, not all contracts, if need change order, just change order by algoritm")]
        public void UpdateHealthByToken(string tokenId, BigInteger healthAmountWei)
        {
            _mainMenuService.UpdateHealthByToken(tokenId, healthAmountWei);
        }

#if UNITY_ANDROID || UNITY_IOS
        public void OnEnable()
        {
            if (_mainMenuService != null)
            {
                Debug.Log($"#debug #android #balance on enable");
                _mainMenuService.SetBalanceValue();
            }
        }
#endif
    }
}