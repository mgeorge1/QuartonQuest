using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostPanelController : MonoBehaviour
{
    public delegate void CancelHandler();
    public event CancelHandler OnCancel;
    public GameObject HostPanel = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCancelButtonClicked()
    {
        HostPanel.SetActive(false);
        OnCancel?.Invoke();
    }
}
