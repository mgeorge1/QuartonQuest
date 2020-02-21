using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class advanceStoryBoard : MonoBehaviour
{
    // Start is called before the first frame update

    public string nextScene;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Input.GetKey("n"))
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
