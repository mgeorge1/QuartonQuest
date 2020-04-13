using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JoinPanelController : MonoBehaviour
{
    public delegate IEnumerator CancelHandler();
    public event CancelHandler OnCancel;
    public GameObject JoinPanel = null;
    public Button cancelButton;
    public TextMeshProUGUI joinGameText;

    private void Start()
    {
        ResetStatus();
    }

    public void SetJoiningStatus()
    {
        joinGameText.text = "Joining Game...";
    }

    public void ResetStatus()
    {
        joinGameText.text = "Join Game";
    }

    public void DisableCancelButton()
    {
        cancelButton.interactable = false;
    }

    public void EnableCancelButton()
    {
        cancelButton.interactable = true;
    }

    public void OnCancelButtonClicked()
    {
        StartCoroutine(Cancel_Async());
    }

    public IEnumerator Cancel_Async()
    {
        cancelButton.interactable = false;
        yield return OnCancel?.Invoke();
        JoinPanel.SetActive(false);
        cancelButton.interactable = true;
    }
}
