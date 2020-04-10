using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ErrorCanvas : MonoBehaviour
{
    public delegate void HandleBackNavigation();
    public event HandleBackNavigation BackButtonClicked = null;
    public TextMeshProUGUI ErrorText;
    public TextMeshProUGUI BackButtonText;
    public Button BackButton;
    private string defaultText = "Main Menu";

    public void OnBackButtonClicked()
    {
        if (BackButtonClicked == null)
            GUIController.Instance.LoadSceneWithTransition(GUIController.SceneNames.MainMenu);
        else
            BackButtonClicked.Invoke();

        BackButtonText.text = defaultText;
    }

    public void Display(string errorText, HandleBackNavigation BackButtonClicked = null)
    {
        ErrorText.text = errorText;
        this.BackButtonClicked = BackButtonClicked;

        if (BackButtonClicked != null)
            BackButtonText.text = "Back";

        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
        BackButtonText.text = defaultText;

        BackButtonClicked = null;
    }

    public void SetErrorText(string text)
    {
        ErrorText.text = text;
    }
}
