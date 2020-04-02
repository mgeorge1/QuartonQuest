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
    public GameObject HUDCanvas;
    public GameObject OpponentControllerObject;
    public GameObject ErrorCanvas;
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

    public string CurrentScene
    {
        get
        {
            return SceneManager.GetActiveScene().name;
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

        GameCoreController.Instance.GameOver += GameOver;

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

    public void GameOver()
    {
        Debug.Log("GameOver from the GUIController");
        if (HUDCanvas != null)
        {
            string gameOverText = GetGameOverText();
            HUDCanvasController script = HUDCanvas.GetComponent<HUDCanvasController>();
            script.DisplayGameOverCanvas(gameOverText);
        }
    }

    public void ReloadCurrentScene()
    {
        Debug.Log("Reloading current scene");
        Scene scene = SceneManager.GetActiveScene();
        LoadScene(scene.name);
    }

    string GetGameOverText()
    {
        switch (GameCoreController.Instance.CurrentTurn)
        {
            case GameCoreController.GameTurnState.PLAYERWON:
                return "You won!";
            case GameCoreController.GameTurnState.OPPONENTFORFEIT:
                return "You won!";
            case GameCoreController.GameTurnState.OPPONENTWON:
                return "Opponent Won";
            case GameCoreController.GameTurnState.PLAYERFORFEIT:
                return "Opponent won!";
            case GameCoreController.GameTurnState.GAMETIED:
                return "Tie!";
            default:
                throw new System.Exception("Invalid end game state");
        }
    }

    public void RequestRematchFromOpponent()
    {
        Debug.Log("Requesting rematch from opponent in the GUIController");
        GameCoreController.Instance.RequestRematchFromOpponent();
        HUDCanvasController script = HUDCanvas.GetComponent<HUDCanvasController>();

        // This string should have the opponent's name in it. 
        // Or maybe this should be a whole different panel.
        script.DisplayGameOverCanvas("Requesting rematch from opponent...");
    }

    public void RequestRematchFromPlayer()
    {
        Debug.Log("Requesting rematch from player in the GUIController");
        HUDCanvasController script = HUDCanvas.GetComponent<HUDCanvasController>();

        // This string should have the opponents name in it
        script.DisplayRematchRequest("Opponent would like a rematch");
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void DisplayErrorCanvas(string errorText)
    {
        ErrorCanvas script = ErrorCanvas.GetComponent<ErrorCanvas>();
        script.SetErrorText(errorText);
        ErrorCanvas.SetActive(true);
    }
}
