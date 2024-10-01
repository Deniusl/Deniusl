using System;
using GameSystems.Scripts;
using GameSystems.Scripts.Services.Interfaces;
using GameSystems.Scripts.Services.Providers;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Tls;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Network = GameSystems.Scripts.Network;
using Types = GameSystems.Scripts.Types;

namespace UI
{
    public class NFTBid : MonoBehaviour
    {
        [SerializeField] private Image _motoImg;
        [SerializeField] private Image _trackImg;
        [SerializeField] private Types _motoType;
        [SerializeField] private Types _trackType;
        [SerializeField] private TextMeshProUGUI _trackName;
        [SerializeField] private TextMeshProUGUI _trackIdLabel;
        [SerializeField] private TextMeshProUGUI _motoName;
        [SerializeField] private TextMeshProUGUI _motoIdLabel;
        [SerializeField] private TextMeshProUGUI _attempts;
        [SerializeField] private TextMeshProUGUI _time;
        [SerializeField] private TextMeshProUGUI _health;
        [SerializeField] public TextMeshProUGUI _totalBids;
        [SerializeField] private TextMeshProUGUI _playerBid; 
        [SerializeField] public Button bidButton;
        [SerializeField] private GameObject borders;
        [SerializeField] public TMP_InputField inputField;
        [SerializeField] private GameObject _txStatus;
        [SerializeField] private GameObject _cursorBid;
        [SerializeField] private Scrollbar _bettingScrollbar;

        private string _trackId;
        private string _motoId;
        public double playerBidValue;
        private string _currentChain;
        public int MotoType => (int) _motoType;
        public int TrackType => (int) _trackType;
        public Button TxStatusButton { get; set; }
        private SelectLevelWindow _selectLevelWindow;
        private bool _scrolledUp;

        private void Awake()
        {
            AddListeners();   
            _selectLevelWindow = GameObject.FindGameObjectWithTag("SelectWindow").GetComponent<SelectLevelWindow>();
        }

        private void OnEnable()
        {
            if (playerBidValue > 0 && GameObject.FindGameObjectsWithTag("CursorBid").Length == 0)
                _cursorBid.SetActive(true);
        }

        public void Setup(string trackId, string trackType, string motoId, string motoType, string time, string motoHealth, string totalBids, double playerBid)
        {
            _currentChain = AvailableNetworks.CurrentNetworkData.chain;
            SetType(trackType, motoType);
            SetImage();
            SetPlayerBid(playerBid);
            SetChainCurrency();
            _trackId = trackId;
            _motoId = motoId;
            _trackName.text = GetNameFromType(_trackType); 
            _motoName.text = GetNameFromType(_motoType);
            _trackIdLabel.text = trackId;
            _motoIdLabel.text = motoId;
            _time.text = time;
            _health.text = motoHealth;
            _totalBids.text = trackId;
            inputField.text = motoId;

        }

        private string GetNameFromType(Types typeName)
        {
            var name = typeName.ToString().Replace("_", " ");
            if (typeName.ToString().Contains("TRACK"))
            {
                name = name.Replace("TRACK ", "");
            }
            
            return name;
        }

        private void SetImage()
        {
            var motoPath = "BidSprites/Moto/" + _motoType.ToString().Replace("_", " ");
            var trackPath = "BidSprites/Cities/" + _trackType.ToString().Replace("_", " ").Replace("TRACK ", "");
            _motoImg.sprite = Resources.Load<Sprite>(motoPath);
            _trackImg.sprite = Resources.Load<Sprite>(trackPath);
        }

        private void SetType(string trackType, string motoType)
        {
            var tempIntMotoType = int.Parse(motoType);
            var tempIntTrackType = int.Parse(trackType);
            _motoType = (Types) tempIntMotoType;
            _trackType = (Types) tempIntTrackType;
            
        }
        
        public void SetPlayerBid(double playerBid)
        {
            playerBidValue = playerBid;
            if (playerBidValue > 0)
            {
                _totalBids.color = Color.green;
                _totalBids.GetComponent<Button>().interactable = true;
                _playerBid.text = playerBidValue + " " + AvailableNetworks.CurrentNetworkData.balanceCurrency;
            }
        }
        
        public void OnInviteButtonClick()
        {
            string inviteUrl = Hosts.currHost;
            if (Application.absoluteURL.Contains("/motoDEXweb"))
                inviteUrl += "motoDEXweb/";
            inviteUrl += "?chain=" + AvailableNetworks.CurrentNetworkData.uriPathName + "?bidСombination=" + _motoId + "," + _trackId;
            GUIUtility.systemCopyBuffer = inviteUrl;
#if UNITY_WEBGL && !UNITY_EDITOR
            Application.OpenURL(inviteUrl);
#endif
        }
        
        public void AddListeners()
        {   
            inputField.onEndEdit.AddListener(delegate { OnEndEdit(inputField); });
            bidButton.onClick.AddListener(OnClick);
        }

        public void RemoveListeners()
        {
            inputField.onEndEdit.RemoveListener(delegate { OnEndEdit(inputField); });
            bidButton.onClick.RemoveListener(OnClick);
        }

        private void SetChainCurrency()
        {
            inputField.placeholder.GetComponent<TMP_Text>().text = '(' + AvailableNetworks.CurrentNetworkData.balanceCurrency + ')';
        }
        
        private async void OnClick()
        {
            await BidServiceProvider.BidService.Bid(_trackId,
                                            _motoId,
                                            (active) => _txStatus.gameObject.SetActive(active),
                                            (active) => TxStatusButton.gameObject.SetActive(active),
                                            (active) => bidButton.gameObject.SetActive(active),
                                            (active) => inputField.gameObject.SetActive(active),
                                            () => TxStatusButton = bidButton,
                                            _selectLevelWindow,
                                            inputField, 
                                            _txStatus);
        }
        
        private void OnEndEdit(TMP_InputField input)
        {
            if (input.text == "") return;
            double value = double.Parse(input.text);
            if (value < MinimalFee.bidMinimalFee)
            {
                value = MinimalFee.bidMinimalFee;
                input.text = value.ToString();
            }
        }

        public void SetInteractable(bool isInteractable)
        {
            inputField.interactable = isInteractable;
            bidButton.interactable = isInteractable;
        }
    }
}