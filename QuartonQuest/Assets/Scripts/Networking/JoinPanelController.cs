using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinPanelController : MonoBehaviour
{
    public delegate void CancelHandler();
    public event CancelHandler OnCancel;
    public GameObject JoinPanel = null;

    public void OnCancelButtonClicked()
    {
        JoinPanel.SetActive(false);
        OnCancel?.Invoke();
    }
}
