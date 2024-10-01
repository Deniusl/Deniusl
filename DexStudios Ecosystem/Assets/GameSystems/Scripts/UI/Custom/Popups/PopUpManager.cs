using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;


public class PopUpManager : MonoBehaviour
{
    [SerializeField] private PopUpsHolderSO _popupsHolderSO;
    public static PopUpManager Instance {private set; get;}

    private List<PopUp> _pushedMessagesIDs = new List<PopUp>();
    
    public static bool IsSomePopUpShown = false;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(Instance != this)
            Destroy(gameObject);
    }

    public bool PushPopUpWithType(PopupType popupType, out string popupID, Action onReject = null, Action onApprove = null)
    {
        PopUpSource popUpSource = _popupsHolderSO.GetPopUpSource(popupType);
        popupID = popUpSource.id;
        return PushPopUp(popupID, popUpSource.title, popUpSource.message, popUpSource.wallet, popUpSource.needApprove, popUpSource.mainButton, popUpSource.negative, onReject, onApprove);
    }

    public bool PushPopUp(string id, string title, string message, string wallet, bool withApprove, PopupButton popupButton, bool negative = false, Action onReject = null, Action onApprove = null)
    {
        if(_pushedMessagesIDs.Find(popUp=>popUp.ID == id))
            return false;

        Debug.Log($"#debug: PushPopUp, id: {id}, title: {title}, message: {message}, wallet: {wallet}, withApprove: {withApprove}, popupButton: {popupButton}, negative: {negative}");

        IsSomePopUpShown = true;
        PopUp popUp = Instantiate(_popupsHolderSO.PopupTemplate);
        popUp.Initialize(id,title,message, wallet, withApprove, popupButton, negative, onReject, onApprove);
        popUp.onPopupClosed += OnPopUpClosed;

        _pushedMessagesIDs.Add(popUp);
        return true;
    }

    public void ClosePopUp(string popupID)
    {
        if(string.IsNullOrEmpty(popupID))
            return;

        IsSomePopUpShown = false;
        PopUp popUp = _pushedMessagesIDs.Find(popUp=>popUp.ID == popupID);
        popUp?.ClosePopup();
    }
    
    private void OnPopUpClosed(PopUp popUp)
    {
        popUp.onPopupClosed -= OnPopUpClosed;
        _pushedMessagesIDs.Remove(popUp);
    }
}