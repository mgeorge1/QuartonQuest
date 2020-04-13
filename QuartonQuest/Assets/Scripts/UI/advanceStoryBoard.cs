using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class advanceStoryBoard : MonoBehaviour
{
    public string nextScene;
    
    public TextMeshProUGUI textBox;
    public GameObject OptionsPanel;

    // Typewriter effect here: http://answers.unity.com/answers/987268/view.html
    string[] storyText = new string[] { 
        "Captain, sensors report that the hull has been breached", 
        "The Cargo Bar door has been opened", 
        "We are being boarded", 
        "Pirates are demanding our valuables", 
        "We must play them" 
    };
    int currentlyDisplayingText = 0;
    bool typingText = false;

    // Time management code from here: https://forum.unity.com/threads/getkey-is-too-fast.222127/
    public float keyDelay = .25f; 
    private float timePassed = 0f;

    private bool OptionsIsActive
    {
        get
        {
            return OptionsPanel.activeInHierarchy;
        }
    }

    private void Update()
    {
        timePassed += Time.deltaTime;

        if (!OptionsIsActive && Input.anyKeyDown && timePassed >= keyDelay)
        {
            if (Input.GetMouseButtonDown(0)
                 || Input.GetMouseButtonDown(1)
                 || Input.GetMouseButtonDown(2))
                return;

            timePassed = 0f;
            if (typingText)
            {
                SkipToFinishText();
            }
            else if (currentlyDisplayingText < storyText.Length)
            {
                StartCoroutine(AnimateText());
            }
            else
            {
                GUIController.Opponent = GUIController.OpponentType.AI;
                GUIController.AIDifficulty = AIController.DifficultySetting.ONE;
                string sceneName;

                if (GUIController.Instance.CurrentScene == GUIController.SceneNames.Story1)
                    sceneName = GUIController.SceneNames.Level1;
                else
                    sceneName = GUIController.SceneNames.Level2;

                GUIController.Instance.LoadScene(sceneName);
            }
        }
    }

    //This is a function for a button you press to skip to the next text
    public void SkipToFinishText()
    {
        StopAllCoroutines();
        textBox.text = storyText[currentlyDisplayingText];
        currentlyDisplayingText++;
        typingText = false;
    }

    IEnumerator AnimateText()
    {
        typingText = true;
        for (int i = 0; i < (storyText[currentlyDisplayingText].Length + 1); i++)
        {
            textBox.text = storyText[currentlyDisplayingText].Substring(0, i);
            yield return new WaitForSeconds(.03f);
        }
        currentlyDisplayingText++;
        typingText = false;    
    }

    public void MainMenuButtonClicked()
    {
        GUIController.Instance.LoadSceneWithTransition(GUIController.SceneNames.MainMenu);
    }
}
