using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsCanvasController : MonoBehaviour
{
    public OptionsPanel Panel;
    public static bool OptionsPanelShowing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (OptionsPanelShowing)
            {
                Panel.ClosePanel();
                OptionsPanelShowing = false;
            }
            else
            {
                Panel.OpenPanel();
                OptionsPanelShowing = true;
            }
        }
    }
}
