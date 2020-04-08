using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostPanelController : MonoBehaviour
{
    public delegate IEnumerator CancelHandler();
    public event CancelHandler OnCancel;
    public GameObject HostPanel = null;
    public Button cancelButton;

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
        StartCoroutine(Cancel_Async());
    }

    public IEnumerator Cancel_Async()
    {
        cancelButton.interactable = false;
        yield return OnCancel?.Invoke();
        HostPanel.SetActive(false);
        cancelButton.interactable = true;
    }
}
