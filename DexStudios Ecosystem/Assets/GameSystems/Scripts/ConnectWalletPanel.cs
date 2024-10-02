using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System.Collections.Generic;
using GameSystems.Scripts;

public class ConnectWalletPanel : MonoBehaviour
{
    [SerializeField] private Button _connectWalletButton;
    [SerializeField] private TMP_Text _buttonText;
    [SerializeField] private string _textConnectWallet,_textWalletConnecting;
    [SerializeField] private Slider _connectionProgressSlider;

    public Button ConnectWalletButton => _connectWalletButton;

    [SerializeField] private GameObject[] _objectsToDeactivateOnConnecting;

    [SerializeField] private Animation _tipAnimation;
    [SerializeField] private Animation _connectingAnimation;
    [SerializeField] private GameObject _connectionProggressGameobject;

    [SerializeField] private CustomDropdown _networkDropdown;

    public Button ConnectButton => _connectWalletButton;

    private void Awake()
    {
        _connectionProggressGameobject.SetActive(false);
    }

    private void Start()
    {
        InitializeNetworkDropdown();

        _connectWalletButton.onClick.AddListener(OnConnectWalletButtonClicked);
    } 

    private void InitializeNetworkDropdown()
    {
        List<CustomDropdownItemOption> options = new List<CustomDropdownItemOption>();

        foreach (var network in AvailableNetworks.networksData)
        {
            CustomDropdownItemOption option = new CustomDropdownItemOption(
                network.networkName,
                () => OnNetworkSelected(network),
                null
            );
            options.Add(option);
        }

        _networkDropdown.Initialize(options);
    }

    public void StarConnection()
    {
        _tipAnimation.Stop();
        _buttonText.text = _textWalletConnecting;
        _connectWalletButton.interactable = false;

        foreach (GameObject obj in _objectsToDeactivateOnConnecting)
            obj.SetActive(false);

        _connectionProggressGameobject.SetActive(true);
        _connectingAnimation.Play();
    }

    public void ConnectingFail()
    {
        _buttonText.text = _textConnectWallet;
        _connectWalletButton.interactable = true;

        foreach (GameObject obj in _objectsToDeactivateOnConnecting)
            obj.SetActive(true);

        _tipAnimation.Play();

        _connectionProggressGameobject.SetActive(false);
        _connectingAnimation.Stop();
    }

    public void HidePanel()
    {
        _connectingAnimation.Stop();
        _tipAnimation.Stop();

        gameObject.SetActive(false);
    }

    public void ShowPanel()
    {
        _tipAnimation.Play();
        _buttonText.text = _textConnectWallet;
        _connectWalletButton.interactable = true;

        foreach (GameObject obj in _objectsToDeactivateOnConnecting)
            obj.SetActive(true);

        gameObject.SetActive(true);
        _connectionProgressSlider.value = 0;
        _connectionProggressGameobject.SetActive(false);
    }
    private void OnNetworkSelected(NetworkData selectedNetwork)
    {
        Debug.Log("Selected network: " + selectedNetwork.networkName);
        AvailableNetworks.CurrentNetworkData = selectedNetwork;
        _networkDropdown.Hide();
        StarConnection();
    }

    private void OnConnectWalletButtonClicked()
    {
        foreach (GameObject obj in _objectsToDeactivateOnConnecting)
        {
            obj.SetActive(false);
        }

        _networkDropdown.Show();
    }

}
