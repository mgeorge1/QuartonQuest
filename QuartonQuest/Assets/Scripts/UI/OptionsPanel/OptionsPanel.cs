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
    public AudioMixer audioMixer;

    public void SetVolume(float volume)
    {
        Debug.Log(volume);
        audioMixer.SetFloat("Volume", volume);
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
