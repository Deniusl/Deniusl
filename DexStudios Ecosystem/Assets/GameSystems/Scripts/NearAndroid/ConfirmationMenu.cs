using System;
using System.Numerics;
using System.Threading.Tasks;
using GameSystems.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationMenu : MonoBehaviour
{
    public GameObject confirmationPanel;
    public TMP_Text confirmationText;
    public Button confirmButton;
    public Button cancelButton;
    private TaskCompletionSource<bool> confirmationTask;

    private void Start()
    {
        confirmButton.onClick.AddListener(ConfirmAction);
        cancelButton.onClick.AddListener(CancelAction);
    }

    public Task<bool> ShowConfirmationMenu(double value)
    {
        confirmationTask = new TaskCompletionSource<bool>();
        string valueString = value == 1E-24 ? "Yocto" : Math.Round((value / Const.Wallet.NEARToHuman), 6).ToString();
        confirmationText.text = "This action will deduct " + valueString + " NEAR from your account.";
        confirmationPanel.SetActive(true);
        return confirmationTask.Task;
    }

    private void ConfirmAction()
    {
        confirmationPanel.SetActive(false);
        confirmationTask.SetResult(true);
    }

    private void CancelAction()
    {
        confirmationPanel.SetActive(false);
        confirmationTask.SetResult(false);
    }
}