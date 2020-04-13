using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelButton : MonoBehaviour
{
    public Button button;

    public void OnHover()
    {
        if (button.interactable && AudioManager.instance != null)
            AudioManager.instance.PlaySoundEffect("ButtonHover");
    }
    
    public void OnPressDown()
    {
        if (button.interactable && AudioManager.instance != null)
            AudioManager.instance.PlaySoundEffect("ButtonClick");
    }
}
