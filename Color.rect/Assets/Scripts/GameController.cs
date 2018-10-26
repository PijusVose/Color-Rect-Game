using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static GameController Instance { get; set; }

    // Delegates and events.
    public delegate void GeneralEventHandler();
    public static event GeneralEventHandler PlayerDiedEvent;
    public static event GeneralEventHandler AddScoreEvent;
    public static event GeneralEventHandler RestartLevelEvent;

    public Text scoreText;
    public int score = 0;
    public int highScore = 0;
    public bool gameStarted = false;
    public bool inStartMenu = true;  

    private void Awake()
    {
        // Initialize singleton.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Subscribe the functions to events.
        PlayerDiedEvent += OnPlayerDied;
        AddScoreEvent += AddScore;
        RestartLevelEvent += RestartLevel;

        // Load the highscore.
        LoadScore();
    }

    #region Event Callbacks

    public static void OnPlayerDiedEvent()
    {
        PlayerDiedEvent();
    }

    public static void OnAddScoreEvent()
    {
        AddScoreEvent();
    }

    public static void OnRestartLevelEvent()
    {
        RestartLevelEvent();
    }

    #endregion

    public void StartGame()
    {
        if (inStartMenu == false)
        {
            gameStarted = true;
        }
    }

    public void OnPlayerDied()
    {
        gameStarted = false;

        Debug.Log("Player died.");

        // Play end screen animation.
        UIController.Instance.SceneCanvasTrigger("PlayerDied");

        // Set the highscore and save it.
        highScore = score > highScore ? score : highScore;
        SaveScore();

        // Update the UI.
        UIController.Instance.UpdateHighScore(highScore);
    }

    public void AddScore()
    {
        // Increase score by 1.
        score++;

        UIController.Instance.UpdateScore(score);
    }

    // Reload scene.
    public void RestartLevel()
    {
        // Reset score.
        score = 0;

        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);

        // Play end screen animation and update UI.
        UIController.Instance.SceneCanvasTrigger("RestartGame");
        UIController.Instance.UpdateScore(score);
    }

    // Saves the highscore.
    void SaveScore()
    {
        PlayerPrefs.SetInt("Highscore", highScore);
    }

    void LoadScore()
    {
        if (PlayerPrefs.HasKey("Highscore"))
        {
            highScore = PlayerPrefs.GetInt("Highscore");
        }
    }

}
