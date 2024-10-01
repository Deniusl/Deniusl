using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisconnectWalletPanel : MonoBehaviour
{
    [SerializeField] private Button _disconectButton;
    [SerializeField] private TMP_Text _balanceText;
    [SerializeField] private GameObject _balanceHolder;
    public Button DisconnectButton => _disconectButton;

    public void SetBalance(string balance)
    {
        _balanceHolder.SetActive(true);
        _balanceText.text = balance;
    }

    public void HidePanel()
    {
        _balanceHolder.SetActive(false);
        gameObject.SetActive(false);
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }
}
