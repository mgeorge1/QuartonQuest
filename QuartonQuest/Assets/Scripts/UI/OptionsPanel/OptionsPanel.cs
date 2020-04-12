using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class OptionsPanel : MonoBehaviour
{
    public bool OptionsPanelShowing
    {
        get
        {
            return Panel.activeInHierarchy;
        }
        set
        {
            Panel.SetActive(value);
        }
    }
    public GameObject Panel;

    public void OnMusicVolumeChanged(float volume)
    {
        AudioManager.instance.SetMusicVolumeLevel(volume / 1);
    }

    public void OnSoundEffectsVolumeChanged(float volume)
    {
        AudioManager.instance.SetSoundEffectsVolumeLevel(volume / 1);
    }

    // For opening and closing the panels, we might want to 
    // pause game time as well. 
    public void OpenPanel()
    {
        Debug.Log("Opening options panel");
        OptionsPanelShowing = true;
        // Time.timeScale = 0f;
    }

    public void ClosePanel()
    {
        Debug.Log("Closing options panel");
        OptionsPanelShowing = false;
        // Time.timeScale = 1f;
    }

    public void ForfeitButtonClicked()
    {
        Debug.Log("Forfeiting match...");
        GameCoreController.Instance.Forfeit();
        ClosePanel();
        GUIController.Instance.GameOver();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }
}
