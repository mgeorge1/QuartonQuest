using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class clickabilityTester : MonoBehaviour
{


    public Text logText;
    public ScrollView scroller;

    // Start is called before the first frame update
    void Start()
    {
        logText.text = "";
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Right clicked the board");
            logText.text += "Right clicked the board\n";
        }
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left clicked the board");
            logText.text += "Left clicked the board\n";
        }
        //https://answers.unity.com/questions/12564/clickable-world-object.html
    }
}
