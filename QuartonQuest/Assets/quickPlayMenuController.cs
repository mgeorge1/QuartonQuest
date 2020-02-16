using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class quickPlayMenuController : MonoBehaviour
{

    public string startGamePath;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame_buttonClicked()
    {
        SceneManager.LoadScene(startGamePath);
    }
}
