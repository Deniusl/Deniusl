using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

// using UnityEngine.EventSystems;

public class PopUp : MonoBehaviour
{
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _messageText;
    [SerializeField] private TMP_InputField _wallet;

    [SerializeField] private Button _approveButton;
    [SerializeField] private Button _rejectButton;


    [SerializeField] private Image _blackout;
    [SerializeField] private Image _bgImage;
    [SerializeField] private Color _positive;
    [SerializeField] private Color _negative;

    [SerializeField] private Color _positiveText;
    [SerializeField] private Color _negativeText;

    // private GameObject _prevSelectedGameObject;
    public string ID { private set; get; }

    public Action<PopUp> onPopupClosed;

    private Button mainButton;

    public void Initialize(string id, string title, string message, string wallet, bool needApprove, PopupButton popupMainButton,
        bool negative = false, Action onReject = null, Action onApprove = null)
    {
        ID = id;
        _bgImage.color = negative ? _negative : _positive;
        _messageText.color = negative ? _negativeText : _positiveText;
        // _prevSelectedGameObject = EventSystem.current.currentSelectedGameObject;

        _title.text = title;
        _messageText.text = message;
        _wallet.text = wallet;
        if (wallet.Length == 0) _wallet.gameObject.SetActive(false);

        _approveButton.onClick.AddListener(() => onApprove?.Invoke());
        _rejectButton.onClick.AddListener(() => onReject?.Invoke());

        _approveButton.onClick.AddListener(ClosePopup);
        _rejectButton.onClick.AddListener(ClosePopup);

        mainButton = popupMainButton switch
        {
            PopupButton.cancel => _rejectButton,
            PopupButton.ok => _approveButton,
            _ => _approveButton,
        };
        
        if (popupMainButton is PopupButton.None)
        {
            _rejectButton.gameObject.SetActive(false);
            _approveButton.gameObject.SetActive(false);
            mainButton.gameObject.SetActive(false);
            mainButton = null;
            _blackout.color = Color.black;
            return;
        }

        if (!needApprove)
        {
            _rejectButton.gameObject.SetActive(false);
            _approveButton.gameObject.SetActive(false);
            mainButton.gameObject.SetActive(true);
        }

        // EventSystem.current.SetSelectedGameObject(mainButton.gameObject);
    }

    public void Initialize(PopUpSource popUpSource, bool negative = false, Action onReject = null,
        Action onApprove = null)
    {
        Initialize(popUpSource.id, popUpSource.title, popUpSource.message, popUpSource.wallet, popUpSource.needApprove,
            popUpSource.mainButton, negative, onReject, onApprove);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (mainButton != null && mainButton.interactable)
            {
                mainButton.onClick.Invoke();
            }
        }
    }

    public void ClosePopup()
    {
        // EventSystem.current.SetSelectedGameObject(_prevSelectedGameObject);
        onPopupClosed?.Invoke(this);
        Destroy(gameObject);
    }
}