using Assets.GameSystems.Scripts.UI;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GameSystems.Scripts.UI
{
    public class UIManager: MonoBehaviour
    {
        private static UIManager _instance;
        
        public static UIManager Instance 
        { 
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<UIManager>();
                    
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<UIManager>();
                    }
                }
                return _instance;
            }
        }
        
        public ChooseNftComponent HealthTabNft { get; set; }
        public ChooseNftComponent ChooseNft { get; set; }
        public ChooseNftComponent ChooseLevel { get; set; }
        public ChooseNftComponent ChooseLevelInvested { get; set; }
        public ChooseNftComponent ChooseLevelWallet { get; set; }
        public SetupNftBetting SetupNftBetting { get; set; }
        public CustomDropdown CustomDropdown { get; set; }
        public TabViewContainer TabViewContainer { get; set; }
        public HealthTab HealthTab { get; set; }
        public ConnectWalletPanel ConnectWalletPanel { get; set; }
        public DisconnectWalletPanel DisconnectWalletPanel { get; set; }
        public MainMenuController MainMenuController { get; set; }
        public SelectingStatusNew SelectingMarksPlay { get; set; }
        public SelectingStatus SelectingMarksShare { get; set; }
        public TrackShareWindow TrackShareWindow { get; set; }
        public ConnectChain ConnectChain { get; set; }
        
        public Button PoapButton { get; set; }

        public bool MenuButtonsActive { get; set; }
        
        public Button[] MenuButtons { get; set; }
        
        public void SetUIComponents(
            ChooseNftComponent healthTabNft,
            ChooseNftComponent chooseNft,
            ChooseNftComponent chooseLevel,
            ChooseNftComponent chooseLevelInvested,
            ChooseNftComponent chooseLevelWallet,
            SetupNftBetting setupNftBetting,
            CustomDropdown customDropdown,
            TabViewContainer tabViewContainer,
            HealthTab healthTab,
            ConnectWalletPanel connectWalletPanel,
            DisconnectWalletPanel disconnectWalletPanel,
            MainMenuController mainMenuController,
            SelectingStatusNew selectingMarksPlay,
            SelectingStatus selectingMarksShare,
            TrackShareWindow trackShareWindow,
            ConnectChain connectChain)
        {
            HealthTabNft = healthTabNft;
            ChooseNft = chooseNft;
            ChooseLevel = chooseLevel;
            ChooseLevelInvested = chooseLevelInvested;
            ChooseLevelWallet = chooseLevelWallet;
            SetupNftBetting = setupNftBetting;
            CustomDropdown = customDropdown;
            TabViewContainer = tabViewContainer;
            HealthTab = healthTab;
            ConnectWalletPanel = connectWalletPanel;
            DisconnectWalletPanel = disconnectWalletPanel;
            MainMenuController = mainMenuController;
            SelectingMarksPlay = selectingMarksPlay;
            SelectingMarksShare = selectingMarksShare;
            TrackShareWindow = trackShareWindow;
            ConnectChain = connectChain;
        }

        public void SetButtons(Button poapButton, Button[] menuButtons)
        {
            PoapButton = poapButton;
            MenuButtons = menuButtons;
        }
        
        public void EnableNFTInteractbale()
        {
            SetupNftBetting.SetInteractable(true);
            HealthTabNft.SetInteractable(true);
        
            ChooseLevel.SetInteractable(true);
            ChooseNft.SetInteractable(true);
            ChooseLevelInvested.SetInteractable(true);
            ChooseLevelWallet.SetInteractable(true);
        }
        
        public void BlockMainUIButtons()
        {
            MenuButtonsActive = false;
            for (int i = 0; i < MenuButtons.Length; i++)
            {
                MenuButtons[i].interactable = false;
            }
        }
        
        public void UnBlockMainUIButtons()
        {
            MenuButtonsActive = true;
            for (int i = 0; i < MenuButtons.Length; i++)
            {
                MenuButtons[i].interactable = true;
            }
        }
        
        public void EnablePreLoadingSpinner()
        {
            ChooseNft.EnablePreLoadingSpinner();
            ChooseLevel.EnablePreLoadingSpinner();
        }
    
        public void DisablePreLoadingSpinner()
        {
            ChooseNft.DisablePreLoadingSpinner();
            ChooseLevel.DisablePreLoadingSpinner();
        }
    }
}