using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickplayToggle : MonoBehaviour
{
    public string Data;
    public Toggle GO;
    public quickPlayMenuController quickPlayScript;
    public delegate void ToggleChangeHandler(Toggle toggle);
    public event ToggleChangeHandler OnToggleChanged;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleChanged(bool isOn)
    {
        if (isOn)
            OnToggleChanged?.Invoke(GO);
    }
}
