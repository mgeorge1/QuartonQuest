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

    private AudioSource audio;



    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void storyMode_buttonClicked()
    {
        Debug.Log("storyMode_button was clicked");
        audio.clip = storyModeButtonSound;
        audio.Play(0);
        SceneManager.LoadScene("level1");
    }

    public void quickPlay_buttonClicked()
    {
        Debug.Log("quickPlay_button was clicked");
        audio.clip = quickPlayButtonSound;
        audio.Play();
        SceneManager.LoadScene("level1.unity");
    }

    public void multiplayer_buttonClicked()
    {
        Debug.Log("multiplayer_button was clicked");
        audio.clip = multiplayerButtonSound;
        audio.Play();
        SceneManager.LoadScene("level1.unity");
    }

    public void settings_buttonClicked()
    {
        Debug.Log("settings_button was clicked");
        audio.clip = settingsButtonSound;
        audio.Play();
        //open panel
    }
}
