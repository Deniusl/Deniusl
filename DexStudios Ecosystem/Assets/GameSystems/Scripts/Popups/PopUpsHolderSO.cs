using UnityEngine;
using System.Collections.Generic;
using System;
[CreateAssetMenu(fileName = "PopupsHolderSO", menuName = "OpenBiSea/PopupsHolderSO", order = 0)]
public class PopUpsHolderSO : ScriptableObject 
{
    [field: SerializeField] public List<PopUpSource> PopupsSources {private set;get;}
    [field: SerializeField] public PopUp PopupTemplate {private set;get;}
    public PopUpSource GetPopUpSource(PopupType popupType) => PopupsSources.Find(popUpSource => popUpSource.popupType == popupType);
}

[Serializable]
public class PopUpSource
{
    public string id;
    public string title;
    public string message;
    public string wallet;
    public PopupType popupType;
    public bool needApprove;
    public bool negative;


    [Header("If we dont need approve, select which button will be shown" /*\nalso it will be used as main selected object on popup open"*/)]
    public PopupButton mainButton;
}

public enum PopupType 
{
    none = 0
}
public enum PopupButton
{
    ok = 0,
    cancel = 1,
    None = 2
}