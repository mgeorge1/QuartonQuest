using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;
using Photon.Pun;

public class GUIController : MonoBehaviorSingleton<GUIController>
{
    public enum OpponentType {  NETWORK, AI };
    public static OpponentType Opponent = OpponentType.AI;
    public bool IsPlayerFirst = true;
    public GameObject HUDCanvas;
    public GameObject OpponentControllerObject;
    public GameObject ErrorCanvas;
    public Camera GameCamera;
    public Color FadeColor = Color.white;

    private HUDCanvasController canvasController;
    private CameraMotionControls cameraControls;
    private static bool playerGoesFirst = true;
    private static bool playerDidSelectTurn = false;
    [SerializeField]
    private GameObject blocker;
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

    public bool CurrentSceneIsLevel
    {
        get
        {
            return (
                CurrentScene == SceneNames.Level1 ||
                CurrentScene == SceneNames.Level2 ||
                CurrentScene == SceneNames.Level3
            );

        }
    }

    public static string CurrentStoryScene = null;
    public static AIController.DifficultySetting AIDifficulty = AIController.DifficultySetting.ONE;

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

        if (HUDCanvas != null)
            canvasController = HUDCanvas.GetComponent<HUDCanvasController>();

        if (GameCamera != null)
            cameraControls = GameCamera.GetComponent<CameraMotionControls>();
    }

    private void FixedUpdate()
    {
        if (!CurrentSceneIsLevel)
            return;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            canvasController.EnableBlocker();
            CameraMotionControls.IsEnabled = true;
        } 
        else
        {
            CameraMotionControls.IsEnabled = false;
            canvasController.DisableBlocker();
        }
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
                StartGame();
                break;
            case SceneNames.Story1:
                Debug.Log("Setting current story to 1");
                CurrentStoryScene = SceneNames.Story1;
                AudioManager.instance.PlaySong("Track3");
                break;
            case SceneNames.Story2:
                CurrentStoryScene = SceneNames.Story2;
                AudioManager.instance.PlaySong("Track2");
                break;
            case SceneNames.MainMenu:
                if (CurrentStoryScene != null)
                {
                    AudioManager.instance.PlaySong("Track1");
                    CurrentStoryScene = null;
                }
                break;
        }
    }

    void StartGame()
    {
        switch (Opponent)
        {
            case OpponentType.NETWORK:
                AttachNetworkController();
                break;
            case OpponentType.AI:
                AttachAIController();
                break;
        }

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
        NetworkController.Instance.InstantiateRPCController();
        GameCoreController.Instance.Opponent = NetworkController.Instance;

        HUDCanvasController script = HUDCanvas.GetComponent<HUDCanvasController>();
        script.OpponentName = NetworkController.OpponentName;
    }

    void AttachAIController()
    {
        OpponentControllerObject = new GameObject();
        AIController controllerScript = OpponentControllerObject.AddComponent(typeof(AIController)) as AIController;
        controllerScript.Difficulty = AIDifficulty;
        OpponentControllerObject.name = typeof(AIController).ToString();
        GameCoreController.Instance.Opponent = OpponentControllerObject.GetComponent<IOpponent>();

        switch (CurrentScene)
        {
            case SceneNames.Level1:
            case SceneNames.Level3:
                canvasController.OpponentName = "Pirate Captain";
                break;
            case SceneNames.Level2:
                canvasController.OpponentName = "Quartonian";
                break;
        }
    }

    public void GameOver()
    {
        Debug.Log("GameOver from the GUIController");
        if (canvasController != null)
        {
            canvasController.DisplayGameOverCanvas();
        }
    }

    public void ReloadCurrentScene()
    {
        Debug.Log("Reloading current scene");
        Scene scene = SceneManager.GetActiveScene();
        LoadScene(scene.name);
    }

    public void RequestRematchFromOpponent()
    {
        Debug.Log("Requesting rematch from opponent in the GUIController");
        GameCoreController.Instance.RequestRematchFromOpponent();

        // This string should have the opponent's name in it. 
        // Or maybe this should be a whole different panel.
        canvasController.SetGameOverText($"Requesting rematch from {canvasController.OpponentName}...");
    }

    public void RequestRematchFromPlayer()
    {
        Debug.Log("Requesting rematch from player in the GUIController");

        // This string should have the opponents name in it
        canvasController.DisplayRematchRequest();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneWithTransition(string sceneName)
    {
        blocker.SetActive(true);
        Initiate.Fade(sceneName, Color.black, 2.0f);
    }

    public void DisplayErrorCanvas(string errorText, ErrorCanvas.HandleBackNavigation handleBackNavigation = null)
    {
        // Set other canvases and panels to hidden
        // When this canvas shows, the user should be blocked from all 
        // action accepting quiting out of current state
        if (canvasController != null)
        {
            canvasController.HideAllPanels();
        }

        ErrorCanvas errorCanvas = ErrorCanvas.GetComponent<ErrorCanvas>();
        errorCanvas.Display(errorText, handleBackNavigation);
    }

    public void HideErrorCanvas()
    {
        ErrorCanvas errorCanvas = ErrorCanvas.GetComponent<ErrorCanvas>();
        errorCanvas.Hide();
    }

    public void ResetCamera()
    {
        cameraControls.ResetCamera();
    }
}
