using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class mainMenuGUI : MonoBehaviour
{
    public AudioClip storyModeButtonSound;
    public AudioClip quickPlayButtonSound;
    public AudioClip multiplayerButtonSound;
    public AudioClip settingsButtonSound;

    public string storyModePath;
    public string quickPlayPath;
    public string multiplayerPath;

    public GameObject helpPanel;

    private AudioSource audio;



    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (helpPanel.activeInHierarchy)
                helpPanel.SetActive(false);
            else
                helpPanel.SetActive(true);
        }
    }


    public void storyMode_buttonClicked()
    {
        Debug.Log("storyMode_button was clicked");
        //audio.clip = storyModeButtonSound;
        //audio.Play(0);
        SceneManager.LoadScene(storyModePath);
    }

    public void quickPlay_buttonClicked()
    {
        Debug.Log("quickPlay_button was clicked");
        SceneManager.LoadScene(quickPlayPath);
    }

    public void multiplayer_buttonClicked()
    {
        Debug.Log("multiplayer_button was clicked");
        SceneManager.LoadScene(multiplayerPath);
    }

    public void quit_buttonClicked()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}
