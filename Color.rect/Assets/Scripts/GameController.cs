using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static GameController Instance { get; set; }

    public enum State
    {
        START,
        PLAYING,
        PAUSE,
        LOST,
        TRANSITION
    }

    public Text scoreText;
    public int score = 0;
    public int highScore = 0;
    public State state;

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
        // Set the frame rate to 60fps.
        Application.targetFrameRate = 60;

        // Set the state.
        state = State.START;

        // Subscribe the functions to events.
        EventController.PlayerDiedEvent += OnPlayerDied;
        EventController.RestartLevelEvent += RestartLevel;
        EventController.AddScoreEvent += AddScore;
        EventController.StartGameEvent += StartGame;

        // Load the highscore.
        LoadScore();
    }

    private void OnApplicationQuit()
    {
        SaveScore();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            PauseLevel();

            SaveScore();
        }
    }

    public void StartGame()
    {
        ShapeController.Instance.SpawnInitialShapes();

        state = State.PLAYING;
    }

    public void OnPlayerDied()
    {
        if (state == State.PLAYING)
        {
            state = State.LOST;

            Debug.Log("Player died.");

            // When the player dies, show an advertisement.
            EventController.OnShowAdEvent();

            // Play end screen animation.
            EventController.OnCanvasTriggerEvent("PlayerDied");

            // Set the highscore and save it.
            highScore = score > highScore ? score : highScore;
            SaveScore();

            // Update the UI.
            EventController.OnUpdateHighScoreEvent(highScore);
        }
    }

    public void AddScore()
    {
        // Increase score by 1.
        score++;

        EventController.OnUpdateScoreEvent(score);
    }

    // Reload scene.
    public void RestartLevel()
    {
        // Reset score.
        score = 0;

        string currentSceneName = SceneManager.GetActiveScene().name;
        StartCoroutine(LoadScene(currentSceneName));
    }

    IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation asyncSceneLoad = SceneManager.LoadSceneAsync(sceneName);

        yield return new WaitUntil(() => asyncSceneLoad.isDone == true);

        // Play end screen animation and update UI.
        EventController.OnCanvasTriggerEvent("RestartGame");
        EventController.OnUpdateScoreEvent(score);
        EventController.OnStartGameEvent();

        Debug.Log("Scene was restarted.");
    }

    public void PauseLevel()
    {
        if (state == State.PLAYING)
        {
            state = State.PAUSE;

            EventController.OnCanvasTriggerEvent("PauseGame");
        }
    }

    public void ResumeLevel()
    {
        if (state == State.PAUSE)
        {
            state = State.PLAYING;

            EventController.OnCanvasTriggerEvent("ResumeGame");
        }
    }

    // Saves the highscore.
    void SaveScore()
    {
        PlayerPrefs.SetInt("Highscore", highScore);
        PlayerPrefs.Save();
    }

    void LoadScore()
    {
        if (PlayerPrefs.HasKey("Highscore"))
        {
            highScore = PlayerPrefs.GetInt("Highscore");
        }
    }

}
