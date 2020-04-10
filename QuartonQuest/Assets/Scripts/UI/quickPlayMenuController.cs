using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class quickPlayMenuController : MonoBehaviour
{
    string SceneToLoad = GUIController.SceneNames.Level1;
    AIController.DifficultySetting Difficulty = AIController.DifficultySetting.ONE;

    public void startGame_buttonClicked()
    {
        GUIController.Opponent = GUIController.OpponentType.AI;
        GUIController.AIDifficulty = Difficulty;
        GUIController.Instance.LoadScene(SceneToLoad);
    }

    public void OnSelectedPlayerFirst(bool isOn)
    {
        if (isOn)
        {
            GUIController.Instance.PlayerGoesFirst = true;
        }
    }

    public void OnSelectedOpponentFirst(bool isOn)
    {
        if (isOn)
        {
            GUIController.Instance.PlayerGoesFirst = false;
        }
    }

    public void OnSelectedLevel1(bool isOn)
    {
        if (isOn)
        {
            SceneToLoad = GUIController.SceneNames.Level1;
        }
    }

    public void OnSelectedLevel2(bool isOn)
    {
        if (isOn)
        {
            SceneToLoad = GUIController.SceneNames.Level2;
        }
    }

    public void OnSelectedLevel3(bool isOn)
    {
        if (isOn)
        {
            SceneToLoad = GUIController.SceneNames.Level3;
        }
    }

    public void OnSelectedEasy(bool isOn)
    {
        if (isOn)
        {
            Difficulty = AIController.DifficultySetting.ONE;
        }
    }

    public void OnSelectedMedium(bool isOn)
    {
        if (isOn)
        {
            Difficulty = AIController.DifficultySetting.TWO;
        }
    }

    public void OnSelectedHard(bool isOn)
    {
        if (isOn)
        {
            Difficulty = AIController.DifficultySetting.THREE;
        }
    }

    public void ReturnToMainMenu()
    {
        GUIController.Instance.LoadSceneWithTransition(GUIController.SceneNames.MainMenu);
    }
}
