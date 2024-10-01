using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using BuyFlow;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameSystems.Scripts;
using TMPro;
using UI;
using Types = GameSystems.Scripts.Types;


public class NftItemAction
{
    private string tokenId;

    public string TokenId
    {
        get => tokenId;
        set
        {
            Debug.Log($"#debug #debug #buy TokenId = {value}");
            tokenId = value;
        }
    }

    public int Type { get; set; }
    public string Name { get; set; }
    public double PriceParsed { get; set; }

    public double PriceWei { get; set; }
    public float Profit { get; set; }
    public BigInteger CurrentHealthWei { get; set; }
    public BigInteger MaxHealthWei { get; set; }
    public BigInteger MaxHealthWeiInMainCoin { get; set; }
    public int MaxHealth { get; set; }
    public Button ActionButton { get; set; }
    public TxStatus TxStatus { get; set; }
    public ISetAvailableComponent AvailableComponent { get; set; }
}

public class NftItemComponent : MonoBehaviour, ISetAvailableComponent
{
    [SerializeField] private GameObject[] interactableElements;

    [SerializeField] private Types _type;
    [SerializeField] private Image image;
    [SerializeField] private Sprite _availableSprite;
    [Tooltip("In %")] [SerializeField] private float _opacity = 0.5f;
    [SerializeField] public TMP_Text _nameView;
    [SerializeField] private TMP_Text _tokenIdLabel;
    [SerializeField] public TMP_Text _countText;
    [SerializeField] public Button _buyButton;
    [SerializeField] public Button _addButton;
    [SerializeField] public GameObject _changeNFT;
    [SerializeField] public GameObject _healthBar;
    [SerializeField] public GameObject _trackStatisticsActive;
    [SerializeField] public Button _addHealthButton;
    [SerializeField] public Button _returnButton;
    [SerializeField] private string _openUrl = "https://app.openbisea.com/metaverse";
    [SerializeField] private Button _chooseButton;
    [SerializeField] private TxStatus _txStatus;
    [SerializeField] private GameObject _idleChoiceBorder;
    [SerializeField] public GameObject _disableState;
    [SerializeField] public Button _inviteButton;
    [SerializeField] public GameObject checkMark;
    [SerializeField] public ImageColorCycler checkMarkOutline;

    private Contract _contract;
    private List<Contract> _contracts;
    private AddedToken _addedTokens;
    private int _addedTokensIndex;
    private List<int> _addedTokensIndexes;
    public bool changingNFT = false;
    private const int TotalOpacity = 1;
    public Types TypeEnum => _type;
    public int Type => (int)_type;
    public string Name { get; private set; }

    [Obsolete("Recognize flow and simplify")]
    public bool _selectIsActive;

    public string _tokenId;
    public float _profit;
    public string _price;
    private double _priceParsed;
    private double _priceWei;
    private SelectLevelWindow _selectLevelWindow;

    public BigInteger _currHealthWei { get; private set; } = -1;
    public BigInteger _maxHealthWei { get; private set; } = -1;
    public BigInteger _maxHealthWeiInMainCoin { get; private set; } = -1;
    public int _maxHealth { get; private set; } = -1;

    public List<Contract> Contracts => _contracts;

    public GameObject IdleChoiceBorder => _idleChoiceBorder;

    private bool _activeSelectFlow;
    public bool _isAvailable;
    private bool _isPriceInitialized;
    private bool _isFirstLaunch = true;

    private int _count;

    public int? PriceInCoins => PurchaseIAPService.TypeProductInfoDictionary[_type].priceInCoins;

    private void Awake()
    {
        _selectLevelWindow = GameObject.FindGameObjectWithTag("SelectWindow").GetComponent<SelectLevelWindow>();
    }

    [Obsolete("Review and simplify, remove circle dependencies to SelectLevelWindow")]
    public void SetAvailable(bool isAvailable, bool moveOrder = true)
    {
        Debug.Log($"NftItemComponent SetAvailable invoked with parameters:" + $"\nisAvailable={isAvailable}," +
                  $"\nmoveOrder={moveOrder}");

        _isAvailable = SetupType(_type, isAvailable);


#if UNITY_IOS || UNITY_ANDROID
        _addButton.gameObject.SetActive(false);
        _returnButton.gameObject.SetActive(false);

        if (_isFirstLaunch)
        {
            _isFirstLaunch = false;
            return;
        }

        if (_tokenId == "" && (_healthBar.activeInHierarchy 
                               || (_tokenIdLabel.transform.parent.gameObject.activeInHierarchy && _type.IsPill())
                               || (_tokenIdLabel.transform.parent.gameObject.activeInHierarchy && _type.IsTrack())))
        {
            Debug.Log($"#debug #nftItemAction #type - {_type} #tokenId - {_tokenId} #available - {_isAvailable}");
            EnableSpinnerOnEmptyTokenId();
            _buyButton.gameObject.SetActive(false);
            SetInteractable(false);
        }
        else
        {
            Debug.Log($"#debug #nftItemAction #type - {_type} #tokenId - {_tokenId} #available - {_isAvailable}");
            DisableSpinnerOnFillTokenId();
            SetInteractable(true);
        }

        if (!_disableState.activeSelf && _type.IsMoto())
        {
            _chooseButton.gameObject.SetActive(_contracts.Exists(x => x.tokenId == _tokenId));
        }

#endif
    }

    public bool SetupType(Types type, bool isAvailable)
    {
        if (_contracts == null)
            return false;

        isAvailable = isAvailable || SetHealthBar();

        ConfigureButtons(isAvailable);
        SetProfitStatistic();
        ConfigureChangeNFT();
        ConfigureImage(isAvailable);

        if (isAvailable && _selectIsActive)
        {
            ConfigureSelectActive(isAvailable);
        }

        ConfigureTypeSpecificSettings(type, isAvailable);

        SetName(_type.ToString().Replace("_", " "));
        _countText.gameObject.SetActive(isAvailable);

        return isAvailable;
    }

    private void ConfigureButtons(bool isAvailable)
    {
        _addButton.gameObject.SetActive(isAvailable);
        _returnButton.gameObject.SetActive(false);
    }

    private void ConfigureChangeNFT()
    {
        _changeNFT.SetActive(_contracts.Count + _addedTokensIndexes.Count > 1);
    }

    private void ConfigureImage(bool isAvailable)
    {
        image.sprite = _availableSprite;
        image.DOFadeInstantly(isAvailable ? TotalOpacity : _opacity);
    }

    private void ConfigureSelectActive(bool isAvailable)
    {
        if (_addedTokens.ownedTokens[_addedTokensIndex] == "owned")
        {
            _addButton.gameObject.SetActive(!isAvailable);
            if (_disableState.activeInHierarchy)
            {
                _chooseButton.gameObject.SetActive(!isAvailable);
                _returnButton.gameObject.SetActive(!isAvailable);
            }
            else
            {
                _chooseButton.gameObject.SetActive(!string.IsNullOrEmpty(_tokenId));
                _returnButton.gameObject.SetActive(isAvailable);
            }
        }
        else
        {
            _addButton.gameObject.SetActive(false);
            _returnButton.gameObject.SetActive(false);
        }
    }

    private void ConfigureTypeSpecificSettings(Types type, bool isAvailable)
    {
        if (type.IsTrack())
        {
            ConfigureTrackType(isAvailable);
        }
        else if (type.IsPill())
        {
            ConfigurePillType();
        }
        else if (type.IsMoto())
        {
            ConfigureMotoType(isAvailable);
        }
    }

    private void ConfigureTrackType(bool isAvailable)
    {
        Debug.Log("NftItemComponent SetupType type switch TRACK case");

        if (!string.IsNullOrEmpty(_tokenId))
        {
            _inviteButton.transform.parent.gameObject.SetActive(false);
        }

        _chooseButton.gameObject.SetActive(isAvailable);

        if (_selectLevelWindow != null && _selectLevelWindow.SelectResetButton != null &&
            !_selectLevelWindow.SelectResetButton.gameObject.activeInHierarchy && !string.IsNullOrEmpty(_tokenId))
        {
            _chooseButton.gameObject.SetActive(false);
        }
        else if (string.IsNullOrEmpty(_tokenId))
        {
            _chooseButton.gameObject.SetActive(false);
        }

        if (_selectIsActive && _addedTokens.ownedTokens[_addedTokensIndex] != "owned")
        {
            //TODO NOT REMOVE
            //_trackStatisticsActive.SetActive(!isAvailable);
        }

        _healthBar.SetActive(false);
    }

    private void ConfigurePillType()
    {
        _healthBar.SetActive(false);
        _addButton.gameObject.SetActive(false);
        _returnButton.gameObject.SetActive(false);

        Debug.Log(
            $"#HealthTab parentname {transform.parent.name != "HealthPills"} counter0 {!_countText.text.Contains("0")} _countText.text {_countText.text}");
        if (transform.parent.name != "HealthPills")
        {
            _chooseButton.gameObject.SetActive(false);
        }
        else if (!_countText.text.Contains("0"))
        {
            _chooseButton.gameObject.SetActive(_selectLevelWindow?.AddHealthService.HealthTabMissingHealth() > 0);
        }
        else if (_countText.text.Contains("0"))
        {
            _chooseButton.gameObject.SetActive(false);
        }
    }

    private void ConfigureMotoType(bool isAvailable)
    {
        _healthBar.SetActive(isAvailable);

        if (_addButton.gameObject.activeSelf)
            _chooseButton.gameObject.SetActive(false);
    }


    public void ActiveSelectWitchCheck(bool value)
    {
        if (value)
        {
            SetAvailable(_isAvailable, false);
        }
        else
        {
            _chooseButton.gameObject.SetActive(false);
        }

        _activeSelectFlow = value;
    }

    public void ActiveAllExceptSelectWithCheck(bool value)
    {
        if (value)
        {
            HandleActiveState();
        }
        else
        {
            DeactivateAllButtons();
        }

        if (_type == Types.TRACK_NYC)
        {
            _addButton.gameObject.SetActive(false);
        }
    }

    private void HandleActiveState()
    {
        if (string.IsNullOrEmpty(_tokenId) || _tokenId == "0")
        {
            return;
        }

        bool hasEmptyTokenId = CheckForEmptyTokenId();

        foreach (var tokenId in _addedTokens.tokenId)
        {
            if (((int)_type).ToString() == tokenId)
            {
                if (hasEmptyTokenId)
                {
                    DeactivateInviteAndBuyButtons();
                }
                else
                {
                    SetBuyButtonState();
                }
            }
        }

        SetAvailable(_isAvailable, false);
        _chooseButton.gameObject.SetActive(false);
    }

    private bool CheckForEmptyTokenId()
    {
        if (_addedTokens == null) return false;

        foreach (var tokenId in _addedTokens.tokenId)
        {
            if (((int)_type).ToString() == tokenId && string.IsNullOrEmpty(tokenId))
            {
                return true;
            }
        }

        return false;
    }

    private void DeactivateInviteAndBuyButtons()
    {
        _inviteButton.transform.parent.gameObject.SetActive(false);
        _buyButton.gameObject.SetActive(false);
    }

    private void SetBuyButtonState()
    {
        if (PriceInCoins <= 0)
        {
            _buyButton.gameObject.SetActive(false);
        }
        else
        {
            _buyButton.gameObject.SetActive(true);
        }
    }

    private void DeactivateAllButtons()
    {
        _buyButton.gameObject.SetActive(false);
        _addButton.gameObject.SetActive(false);
        _addHealthButton.gameObject.SetActive(false);
        _returnButton.gameObject.SetActive(false);
        _inviteButton.transform.parent.gameObject.SetActive(false);
    }

    public void SetInteractable(bool active)
    {
        _buyButton.interactable = !string.IsNullOrEmpty(_tokenId) && !_healthBar.activeInHierarchy &&
                                  PriceInCoins > 0 && active;
        _chooseButton.interactable = active;
        _addButton.interactable = active;
        _returnButton.interactable = active;
        _inviteButton.interactable = false; // active; This button is not working
        _addHealthButton.interactable = active;

        if (_type == Types.TRACK_NYC)
            _addButton.gameObject.SetActive(false);
    }

    public void SetActiveElements(bool active)
    {
        foreach (var element in interactableElements)
        {
            element.SetActive(active);
        }
    }

    public void Setup(Contract contract)
    {
        if (!_contracts.Any(c => c.tokenId == contract.tokenId))
        {
            _contracts.Add(contract);
            IncrementCount();
        }

        _contract = contract;
        _tokenId = _contract.tokenId;
        SetProfit(false);
        SetHealth(false);
        SetHealthBar();
        SetupPrice();
        SetName(_contract.ItemData.name.ToUpper());
        _chooseButton.interactable = true;
        SetTokenIdView(_tokenId);

        if (_type == Types.TRACK_NYC)
        {
            _addButton.gameObject.SetActive(false);
            _returnButton.gameObject.SetActive(false);
            _addButton.onClick.Invoke();
        }
    }

    public void SetupForAdded(AddedToken addedTokens, int addedTokensIndex)
    {
        if (!_addedTokensIndexes.Contains(addedTokensIndex))
        {
            _addedTokensIndexes.Add(addedTokensIndex);
            _addedTokensIndexes = _addedTokensIndexes.OrderByDescending(d => addedTokens.currHealthWei[d]).ToList();

            Debug.Log(
                $"#debug #addtoken SetupForAdded {_addedTokensIndexes[0]} for {Type}  tokenId {addedTokens.tokenId} Invites.tokenId {Invites.tokenId}");

            if (Invites.tokenId != "")
            {
                for (int i = 0; i < _addedTokensIndexes.Count; i++)
                {
                    if (addedTokens.tokenId[_addedTokensIndexes[i]] == Invites.tokenId)
                    {
                        var tempAddedTokensIndex = _addedTokensIndexes[i];
                        _addedTokensIndexes.RemoveAt(i);
                        _addedTokensIndexes.Insert(0, tempAddedTokensIndex);
                        break;
                    }
                }
            }

            addedTokensIndex = _addedTokensIndexes[0];
        }

        _addedTokens = addedTokens;
        _addedTokensIndex = addedTokensIndex;
        _tokenId = _addedTokens.tokenId[_addedTokensIndex];
        SetProfit(true);
        SetHealth(true);
        Debug.Log($"#debug #setTokenId #setupAdded - {_tokenId}");
        SetTokenIdView(_tokenId);
    }

    public void SetupAndOrderNFTs(string targetId)
    {
        if (_addedTokens == null || _addedTokens.tokenId == null || _addedTokens.tokenId.Length == 0)
        {
            return;
        }

        int targetIndex = Array.IndexOf(_addedTokens.tokenId, targetId);

        if (targetIndex != -1)
        {
            MoveElementToFront(_addedTokens.typeId, targetIndex);
            MoveElementToFront(_addedTokens.tokenId, targetIndex);
            MoveElementToFront(_addedTokens.ownedTokens, targetIndex);
            MoveElementToFront(_addedTokens.currHealthWei, targetIndex);
            MoveElementToFront(_addedTokens.maxHealthWei, targetIndex);
            MoveElementToFront(_addedTokens.maxHealthWeiInMainCoin, targetIndex);
            MoveElementToFront(_addedTokens.maxHealth, targetIndex);
            MoveElementToFront(_addedTokens.profit, targetIndex);
        }
    }

    private void MoveElementToFront<T>(T[] array, int index)
    {
        if (index <= 0 || index >= array.Length) return;

        T item = array[index];
        Array.Copy(array, 0, array, 1, index);
        array[0] = item;
    }

    public void Reset()
    {
        _contracts = new List<Contract>();
        _addedTokensIndexes = new List<int>();

        _selectIsActive = false;
        SetCountAndUpdateView(0);
        SetAvailable(false);
    }

    public void NextNFT()
    {
        changingNFT = true;
        int currIndx;
        if (_addedTokens != null && _addedTokens.tokenId.Contains(_tokenId))
        {
            currIndx = _addedTokensIndexes.IndexOf(_addedTokensIndex);
            if (currIndx != _addedTokensIndexes.Count - 1)
            {
                _selectIsActive = true;
                SetupForAdded(_addedTokens, _addedTokensIndexes[currIndx + 1]);
            }
            else if (_contracts.Count > 0)
            {
                _selectIsActive = false;
                Setup(_contracts[0]);
            }
            else
            {
                _selectIsActive = true;
                SetupForAdded(_addedTokens, _addedTokensIndexes[0]);
            }
        }
        else
        {
            currIndx = _contracts.IndexOf(_contract);
            if (currIndx != _contracts.Count - 1)
            {
                _selectIsActive = false;
                Setup(_contracts[currIndx + 1]);
            }
            else if (_addedTokensIndexes.Count > 0)
            {
                _selectIsActive = true;
                SetupForAdded(_addedTokens, _addedTokensIndexes[0]);
            }
            else
            {
                _selectIsActive = false;
                Setup(_contracts[0]);
            }
        }

        SetHealthBar();
        SetAvailable(true);
        FlowSelection();

        changingNFT = false;
    }

    public void PreviousNFT()
    {
        changingNFT = true;
        int currIndx;
        if (_addedTokens != null && _addedTokens.tokenId.Contains(_tokenId))
        {
            currIndx = _addedTokensIndexes.IndexOf(_addedTokensIndex);
            if (currIndx != 0)
            {
                _selectIsActive = true;
                SetupForAdded(_addedTokens, _addedTokensIndexes[currIndx - 1]);
            }
            else if (_contracts.Count > 0)
            {
                _selectIsActive = false;
                Setup(_contracts[^1]);
            }
            else
            {
                _selectIsActive = true;
                SetupForAdded(_addedTokens, _addedTokensIndexes.Last());
            }
        }
        else
        {
            currIndx = _contracts.IndexOf(_contract);
            if (currIndx != 0)
            {
                _selectIsActive = false;
                Setup(_contracts[currIndx - 1]);
            }
            else if (_addedTokensIndexes.Count > 0)
            {
                _selectIsActive = true;
                SetupForAdded(_addedTokens, _addedTokensIndexes.Last());
            }
            else
            {
                _selectIsActive = false;
                Setup(_contracts[^1]);
            }
        }

        SetHealthBar();
        SetAvailable(true);
        FlowSelection();

        changingNFT = false;
    }

    public void FlowSelection()
    {
        if (_type.IsTrack())
        {
            if (_activeSelectFlow)
            {
                ActiveSelectWitchCheck(true);
                ActiveAllExceptSelectWithCheck(false);
            }
            else
            {
                ActiveSelectWitchCheck(false);
                ActiveAllExceptSelectWithCheck(true);
            }
        }
    }

    public void SetProfitStatistic()
    {
        _trackStatisticsActive.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = _profit + "$";
    }

    public void SetProfit(bool added)
    {
        if (added && _addedTokens != null)
        {
            _profit = _addedTokens.profit[_addedTokensIndex];
        }
        else if (!added && _contract != null)
        {
            _profit = _contract.profit;
        }
        else
        {
            Debug.Log("Contracts not assigned. Can't set profit.");
        }
    }

    [Obsolete("Super Hardcode for visual health")]
    public void SetHealth(bool added)
    {
        if (added && _addedTokens != null)
        {
            _currHealthWei = _addedTokens.currHealthWei[_addedTokensIndex];
            _maxHealthWei = _addedTokens.maxHealthWei[_addedTokensIndex];
            _maxHealthWeiInMainCoin = _addedTokens.maxHealthWeiInMainCoin[_addedTokensIndex];
            _maxHealth = _addedTokens.maxHealth[_addedTokensIndex];
        }
        else if (!added && _contract != null)
        {
            _currHealthWei = _contract.currHealthWei;
            _maxHealthWei = _contract.maxHealthWei;
            _maxHealthWeiInMainCoin = _contract.maxHealthWeiInMainCoin;
            _maxHealth = _contract.maxHealth;
        }
        else
        {
            return;
        }

        SetHealthBar();
    }

    private void SetTokenIdView(string value)
    {
        _tokenIdLabel.transform.parent.gameObject.SetActive(true);
        _tokenIdLabel.text = value.GetLastSymbol(4);
    }

    private void SetCountAndUpdateView(int value)
    {
        Debug.Log($"#debug Type {Type} Count {value}");

        _count = value;

        if (_countText == null) return;

        _countText.gameObject.SetActive(true);
        _countText.text = $"{value}";
    }

    private void IncrementCount()
    {
        SetCountAndUpdateView(_count + 1);
    }

    public void SetName(string value)
    {
        Name = value;
        UpdateNameView();
    }

    private void UpdateNameView()
    {
        var priceView = string.Empty;

#if UNITY_ANDROID || UNITY_IOS
        var productId = PurchaseIAPService.TypeProductInfoDictionary[_type].id;
        
        if (_selectLevelWindow != null && _selectLevelWindow.PurchaseService != null)
            priceView = _selectLevelWindow.PurchaseService.GetProductPrice(productId);

        if (PriceInCoins.HasValue)
        {
            if (PriceInCoins <= 0)
            {
                _nameView.text = $"{Name} (FREE)";
                return;
            }
                
            _nameView.text = $"{Name} ({priceView} / {PriceInCoins} Coins)";
        }
        else
        {
            _nameView.text = $"{Name} ({priceView})";
        }
#else
        if (!string.IsNullOrEmpty(_price))
        {
            var currency = !AvailableNetworks.CurrentNetworkData.isPricesInUSD
                ? AvailableNetworks.CurrentNetworkData.balanceCurrency
                : "$";
            priceView = $" ({_price} {currency})";
            // Debug.Log($"PriceView: {priceView}");
        }

        _nameView.text = $"{Name}{priceView}";
#endif
    }

    private bool SetHealthBar()
    {
        if (_currHealthWei < 0)
        {
            _chooseButton.gameObject.SetActive(false);
            return false;
        }

        if (_maxHealthWei == 0)
        {
            SetHealthBarFillAmount(0);
            _chooseButton.gameObject.SetActive(false);
            return false;
        }

        SetHealthBarFillAmount(CalculateHealthDelta());
        HandleDisableState();

        return true;
    }

    private void SetHealthBarFillAmount(float amount)
    {
        _healthBar.transform.GetChild(1).GetComponent<Image>().fillAmount = Mathf.Clamp(amount, 0f, 1f);
    }

    private float CalculateHealthDelta()
    {
        return (float)Math.Round((float)_currHealthWei / (float)_maxHealthWeiInMainCoin, 2);
    }

    private void HandleDisableState()
    {
        if (_disableState == null) return;

        if (ShouldDisableState())
        {
            EnableMotoState();
        }
        else
        {
            EnableTrackState();
        }
    }

    private bool ShouldDisableState()
    {
        return Type < 11 && _currHealthWei == 0;
    }

    private void EnableMotoState()
    {
        _disableState.SetActive(true);
        _returnButton.gameObject.SetActive(false);
        _chooseButton.gameObject.SetActive(false);
        EnableHealthBarAnimation();
    }

    private void EnableTrackState()
    {
        _disableState.SetActive(false);
        _chooseButton.gameObject.SetActive(false);
        DisableHealthBarAnimation();
        _addHealthButton.gameObject.SetActive(true);
        SetHealthBarAddButtonAlpha(0);
    }

    private void EnableHealthBarAnimation()
    {
        _healthBar.transform.GetChild(3).GetComponent<Animation>().enabled = true;
    }

    private void DisableHealthBarAnimation()
    {
        _healthBar.transform.GetChild(3).GetComponent<Animation>().enabled = false;
    }

    private void SetHealthBarAddButtonAlpha(float alpha)
    {
        var healthBarAddBtnColor = _healthBar.transform.GetChild(3).GetComponent<Image>().color;
        healthBarAddBtnColor.a = alpha;
        _healthBar.transform.GetChild(3).GetComponent<Image>().color = healthBarAddBtnColor;
    }

    [Obsolete("Set Invite Flow from all refferences")]
    public void OnInviteButtonClick()
    {
        string inviteUrl = Hosts.currHost;
        if (Application.absoluteURL.Contains("/motoDEXweb"))
            inviteUrl += "motoDEXweb/";
        inviteUrl += "?chain=" + AvailableNetworks.CurrentNetworkData.uriPathName + "?tokenId=" + _tokenId;
        GUIUtility.systemCopyBuffer = inviteUrl;
#if UNITY_WEBGL && !UNITY_EDITOR
        Application.OpenURL(inviteUrl);
#endif
    }

    public void EnablePreLoadingSpinner()
    {
        if (!_selectIsActive && _txStatus != null)
            _txStatus.gameObject.SetActive(true);
    }

    private void EnableSpinnerOnEmptyTokenId()
    {
        if (_txStatus != null)
        {
            _txStatus.gameObject.SetActive(true);
            _txStatus.EnableSpinner(true);
        }
    }

    private void DisableSpinnerOnFillTokenId()
    {
        if (_txStatus != null)
        {
            _txStatus.gameObject.SetActive(false);
            _txStatus.EnableSpinner(false);
        }

        _idleChoiceBorder.SetActive(true);
    }

    [Obsolete("Check does we need execute status logic")]
    public void DisablePreLoadingSpinner()
    {
        if (_tokenId == "" && (_healthBar.activeInHierarchy
                               || (_tokenIdLabel.transform.parent.gameObject.activeInHierarchy && _type.IsPill())))
        {
            return;
        }

        _txStatus.gameObject.SetActive(false);
    }

    [Obsolete("Remove ContractGetHealthValues call on upper level, only set values")]
    // Need setup using maxHealthWeiInMainCoin only for hear
    public async UniTask SetupPrice(string setPrice = default)
    {
        if (setPrice != default)
        {
            if (double.TryParse(setPrice, out var priceParsed))
            {
                double priceCorrection = 0;
                if (AvailableNetworks.CurrentNetworkData.chain == "aptos")
                {
                    priceCorrection = priceParsed / AvailableNetworks.CurrentNetworkData.DecimalToHuman;
                    _price = FormatNumber.FormatToFirstThreeNonZeroDecimal(priceCorrection, true);
                }
                else
                {
                    priceCorrection = priceParsed / AvailableNetworks.CurrentNetworkData.DecimalToHuman;

                    _price = FormatNumber.FormatToFirstThreeNonZeroDecimal(priceCorrection, true);
                }

                _priceParsed = priceCorrection;
                Debug.Log(
                    $"#debug #nft SetupPrice from param {setPrice}, to double {priceParsed} result: _price {_price}");
            }

            return;
        }

        //var getCashedPrice = RequestSystem.Instance.CashPrice.GetCashedPrice(Type.ToString());
        //if (getCashedPrice.exist)
        //
        //    SetupPriceServer(getCashedPrice.value);
        //}
    }

    public void SetupPriceServerFromList(List<PriceServer> prices)
    {
        foreach (PriceServer p in prices)
        {
            if (p.type == (int)_type)
            {
                SetupPriceServer(p);
            }
        }
    }

    public void SetupPriceServer(PriceServer priceServer)
    {
        Debug.Log($"Price type = {priceServer.type}");

        if (AvailableNetworks.CurrentNetworkData.isPricesInUSD)
        {
            _price = FormatNumber.FormatToFirstThreeNonZeroDecimal(priceServer.price, true);
            SetName(_type.ToString().Replace("_", " "));

            Debug.Log($"#debug #nft SetupPriceServer {_nameView.text} price {priceServer.price}");
        }
        else
        {
            _priceWei = priceServer.priceWei;
            _priceParsed = priceServer.priceMain;
            _price = FormatNumber.FormatToFirstThreeNonZeroDecimal(_priceParsed, true);
            SetName(priceServer.name.ToUpper());

            Debug.Log($"#debug #priceMain {priceServer.priceMain}  #priceParsed {_priceParsed}");
            Debug.Log($"#debug #nft SetupPriceServer {_nameView.text} priceMain {priceServer.priceMain}");
        }

        _isPriceInitialized = true;
    }


    public void SubscribeBuyButton(Action<NftItemAction> onBuyClick)
    {
        _buyButton.onClick.AddListener(
            () => onBuyClick?.Invoke(GetItemData()));
    }

    public void SubscribeChooseButton(Action<NftItemAction> onChooseClick)
    {
        _chooseButton.onClick.AddListener(
            () => onChooseClick?.Invoke(GetItemData()));
    }

    public void SubscribeAddButton(Action<NftItemAction> onAddClick)
    {
        _addButton.onClick.AddListener(
            () => onAddClick?.Invoke(GetItemData()));
    }

    public void SubscribeReturnButton(Action<NftItemAction> onReturnClick)
    {
        _returnButton.onClick.AddListener(
            () => onReturnClick?.Invoke(GetItemData()));
    }

    public void SubscribeAddHealthButton(Action<NftItemAction> onAddHealthClick)
    {
        _addHealthButton.onClick.AddListener(
            () => onAddHealthClick?.Invoke(GetItemData()));
    }

    public void SubscribeInviteButton(Action<NftItemAction> onInviteClick)
    {
        _inviteButton.onClick.AddListener(
            () => onInviteClick?.Invoke(GetItemData()));
    }

    private NftItemAction GetItemData() => new()
    {
        TokenId = _tokenId,
        Type = Type,
        Name = Name,
        PriceParsed = _priceParsed,
        PriceWei = _priceWei,
        Profit = _profit,
        CurrentHealthWei = _currHealthWei,
        MaxHealthWei = _maxHealthWei,
        MaxHealthWeiInMainCoin = _maxHealthWeiInMainCoin,
        MaxHealth = _maxHealth,
        ActionButton = _addHealthButton,
        TxStatus = _txStatus,
        AvailableComponent = this
    };

    public void RemoveAllListeners()
    {
        _buyButton.onClick.RemoveAllListeners();
        _chooseButton.onClick.RemoveAllListeners();
        _addButton.onClick.RemoveAllListeners();
        _returnButton.onClick.RemoveAllListeners();
        _addHealthButton.onClick.RemoveAllListeners();
    }
}

public interface ISetAvailableComponent
{
    void SetAvailable(bool isAvailable, bool moveOrder = true);
}