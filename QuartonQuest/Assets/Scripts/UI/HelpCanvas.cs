using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HelpCanvas : MonoBehaviour
{
    public Image[] helpImages;
    public Button NextButton;
    public Button BackButton;
    public GameObject WrittenRulesPanel;
    public TextMeshProUGUI helpText;
    private string[] helpStrings =
    {
        "When the game begins, the player going first will select the piece for the other player to place.",
        "Next, the second player will place the piece that the first player gave them. The second player will then select another piece for the first player to place.",
        "Continue selecting and placing pieces until one player places a piece which connects a row of 4 pieces that have at least one characteristic in common. " +
            "This can be done horizontally, vertically, or diagonally."
    };

    private int currentIndex = 0;
    private int CurrentIndex
    {
        get
        {
            return currentIndex;
        }

        set
        {
            Debug.Log("Setting current help index");
            if (value == 0)
            {
                BackButton.interactable = false;
                currentIndex = value;
            }
            else if (value == helpImages.Length - 1)
            {
                NextButton.interactable = false;
                currentIndex = value;
            }
            else
            {
                BackButton.interactable = NextButton.interactable = true;
                currentIndex = value;
            }
        }
    }

    private void Awake()
    {
        BackButton.interactable = false;
        helpText.text = helpStrings[CurrentIndex];
    }

    private void Start()
    {
        BackButton.interactable = false;
    }

    public void NextButtonClicked()
    {
        CurrentIndex++;
        helpImages[currentIndex].gameObject.SetActive(true);
        helpImages[currentIndex - 1].gameObject.SetActive(false);
        helpText.text = helpStrings[CurrentIndex];
    }

    public void BackButtonClicked()
    {
        CurrentIndex--;
        helpImages[currentIndex].gameObject.SetActive(true);
        helpImages[currentIndex + 1].gameObject.SetActive(false);
        helpText.text = helpStrings[CurrentIndex];
    }

    public void CloseButtonClicked()
    {
        this.gameObject.SetActive(false);
    }

    public void RulesButtonClicked()
    {
        WrittenRulesPanel.SetActive(true);
    }

    public void CloseRulesButtonClicked()
    {
        WrittenRulesPanel.SetActive(false);
    }
}
