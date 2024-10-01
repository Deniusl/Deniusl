using System;
using Cysharp.Threading.Tasks;
using GameSystems.Scripts.Constants;
using GameSystems.Scripts.UI;
using UnityEngine;

namespace GameSystems.Scripts.Services
{
    public class ConnectionService: IConnectionService
    {
        private IContractService _contractService;
        private IRequestSystem _requestSystem;
        private ISetupContractsService _setupContractsService;

        public ConnectionService(IContractService contractService, IRequestSystem requestSystem, ISetupContractsService setupContractsService)
        {
            _contractService = contractService;
            _requestSystem = requestSystem;
            _setupContractsService = setupContractsService;
        }
        
        public async UniTask OnConnected()
        {
            
            Debug.Log($"MainMenuService OnConnected invoked");
    #if UNITY_WEBGL
            try
            {
                await MinimalFee.SetBidMinimalFee();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
    #endif
            await _setupContractsService.GetPrices();
            await _setupContractsService.SetupAvailabeContracts();
            
            Debug.Log($"#debug SetupAvailabeContracts done");

            Debug.Log($"#debug #accont #switchScene ThirdStep PlayerPrefs.SetString NetworkIndex to: {UIManager.Instance.CustomDropdown.SelectedItem.Index}");
            PlayerPrefs.SetString(PlayerPrefsKeys.NetworkIndex, UIManager.Instance.CustomDropdown.SelectedItem.Index.ToString());

            Debug.Log("SelectLevelWindow AvailableNetworks.currNetworkData.chain " + AvailableNetworks.CurrentNetworkData.chain);

            await UIManager.Instance.ChooseNft.SetPriceForUnExistContracts(_contractService.Contracts, _requestSystem.CashPrice);
            await UIManager.Instance.ChooseLevel.SetPriceForUnExistContracts(_contractService.Contracts, _requestSystem.CashPrice);

            UIManager.Instance.ConnectWalletPanel.HidePanel();
            UIManager.Instance.DisconnectWalletPanel.ShowPanel();
            
            UIManager.Instance.ChooseNft.SetInteractable(true);
            UIManager.Instance.ChooseNft.DisablePreLoadingSpinner();
            UIManager.Instance.ChooseLevel.SetInteractable(true);
            UIManager.Instance.ChooseLevel.DisablePreLoadingSpinner();
            
            UIManager.Instance.EnableNFTInteractbale();

            MockCashResponseSystem.Instance.DebugToJsonString();

            ApplicationVersion.TxtVersion.SetText(Application.version + "  " + AvailableNetworks.CurrentNetworkData.chain);
        }

        public void OnConnectionFail()
        {
            UIManager.Instance.ConnectWalletPanel.ConnectingFail();
        }
    }
}

public interface IConnectionService
{
    UniTask OnConnected();

    void OnConnectionFail();
}