using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class netMenu1Controller : MonoBehaviour
{
    public string joinButtonPath;
    public string createButtonPath;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void joinGame_buttonClicked()
    {
        Debug.Log("StartGame was clicked");
        SceneManager.LoadScene(joinButtonPath);
    }

    public void createGame_buttonClicked()
    {
        Debug.Log("StartGame was clicked");
        SceneManager.LoadScene(createButtonPath);
    }
}
