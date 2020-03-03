using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;
using Photon.Pun;

public class GUIController : MonoBehaviorSingleton<GUIController>
{
    [SerializeField] public bool IsNetworkedGame = false;
    public bool IsPlayerFirst = true;
    public GameObject PlayerDisconnectedPanel;
    public GameObject UICanvas;
    public GameObject OpponentControllerObject;
    private IOpponent OpponentController;

    private static bool playerGoesFirst = true;
    private static bool playerDidSelectTurn = false;
    public bool PlayerGoesFirst
    {
        get
        {
            return playerGoesFirst;
        }

        set
        {
            Debug.Log("Setting PlayerGoesFirst");
            playerDidSelectTurn = true;
            playerGoesFirst = value;
        }
    }

    public struct SceneNames
    {
        public const string Level1 = "Level1Scene";
        public const string Level2 = "Level2Scene";
        public const string Level3 = "Level3Scene";
        public const string MainMenu = "MainMenuScene";
        public const string QuickPlayMenu = "QuickPlayMenuScene";
        public const string NetworkMenu = "NetworkMenuScene";
        public const string NetworkGame = "NetGameScene";
        public const string Story1 = "Story1Scene";
        public const string Story2 = "Story2Scene";
        public const string Credits = "CreditsScene";
    }

   void Awake()
    {
        Debug.Log("GUIController initializing");
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        Debug.Log("Disabling GUIController");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Checking scene");
        switch (scene.name)
        {
            case SceneNames.Level1:
            case SceneNames.Level2:
            case SceneNames.Level3:
            case SceneNames.NetworkGame:
                StartGame();
                break;
        }
    }

    void StartGame()
    {
        if (IsNetworkedGame)
            AttachNetworkController();
        else
            AttachAIController();

        Debug.Log("PlayerDidSelectTurn = " + playerDidSelectTurn);
        if (playerDidSelectTurn)
        {
            StartCoroutine(GameCoreController.Instance.PlayGame(PlayerGoesFirst));
            playerDidSelectTurn = false;
        }
        else
            StartCoroutine(GameCoreController.Instance.PlayGame());
    }

    void AttachNetworkController()
    {
        Debug.Log(NetworkController.Instance);
        NetworkController.Instance.InstantiateRPCController();
        GameCoreController.Instance.Opponent = NetworkController.Instance;
    }

    void AttachAIController()
    {
        OpponentControllerObject = new GameObject();
        OpponentControllerObject.AddComponent(typeof(AIController));
        OpponentControllerObject.name = typeof(AIController).ToString();
        GameCoreController.Instance.Opponent = OpponentControllerObject.GetComponent<IOpponent>();
    }

    public void DisplayPlayerDisconnectedPanel(string playerName)
    {
        if (PlayerDisconnectedPanel != null)
        {
            PlayerDisconnectedPanel script = PlayerDisconnectedPanel.GetComponent<PlayerDisconnectedPanel>();
            script.SetPlayerName(playerName);
            PlayerDisconnectedPanel.SetActive(true);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
