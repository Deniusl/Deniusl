using System.Numerics;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class HealthTab : MonoBehaviour
{
    [SerializeField] public Image _playerImage;
    [SerializeField] public TMP_Text _playerName;
    [SerializeField] public Button applyHealthButton;
    [SerializeField] public Image _healthBarFillImg;
    [SerializeField] public Image _healthBarPillFillImg;
    [SerializeField] public Text _healthBarTxt;
    [SerializeField] public Text _healthBarTxtChanged;
    [SerializeField] public Button _purchaseHealthButton;
    [SerializeField] public TMP_Text _purchaseHealthTxt;
    [SerializeField] public TxStatus _purchaseHealthTxStatus;
    
    public NftItemAction ItemAction { get; set; }
    public BigInteger _purchaseHealthValue;
    private SelectLevelWindow _selectLevelWindow;
    
    public void OnApplyBtnClicked()
    {
        applyHealthButton.interactable = false;
        // ???
        //applyHealthButton.GetComponent<Animator>().enabled = false;
        _selectLevelWindow.ApplyHealthClicked(ItemAction);
    }
    public void OnHealthPurchaseButtonClick()
    {
        _selectLevelWindow.HealthPurchaseClicked();
    }
    
    private void Awake()
    {
        _selectLevelWindow = GameObject.FindGameObjectWithTag("SelectWindow").GetComponent<SelectLevelWindow>();
        OnSelectedActivateAnimation(false);
    }

    public void OnSelectedActivateAnimation(bool active)
    {
        if (active)
        {
            _healthBarPillFillImg.GetComponent<Animator>().Play("ColorAChange", 0, 0);
            _healthBarTxt.GetComponent<Animator>().Play("TxtColorAChange", 0, 0);
            _healthBarTxtChanged.GetComponent<Animator>().Play("ChangedTxtColorAChange", 0, 0);
        }
        var color = _healthBarTxt.GetComponent<Text>().color;
        color.a = 1;
        _healthBarTxt.GetComponent<Text>().color = color;
        _healthBarTxt.GetComponent<Animator>().enabled = active;

        color.a = 0;
        _healthBarTxtChanged.GetComponent<Text>().color = color;
        _healthBarTxtChanged.gameObject.SetActive(active);
        _healthBarPillFillImg.GetComponent<Image>().color = color;
        _healthBarPillFillImg.gameObject.SetActive(active);
            
    }
    
}
