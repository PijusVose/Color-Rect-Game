using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public static UIController Instance { get; set; }

    public GameObject sceneCanvas;
    public GameObject endScreen;
    public Text scoreText;
    public Text highScoreText;
    public Animator sceneCanvasAnim;

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

    // Use this for initialization
    void Start ()
    {
        sceneCanvas = GameObject.FindGameObjectWithTag("SceneCanvas");

        // Just incase, check if the scene canvas was found.
        if (sceneCanvas != null)
        {
            endScreen = sceneCanvas.transform.Find("EndScreen").gameObject;
            scoreText = endScreen.transform.Find("Score").GetComponent<Text>();
            highScoreText = scoreText.transform.Find("Highscore").transform.Find("HighscoreText").GetComponent<Text>();
            sceneCanvasAnim = sceneCanvas.GetComponent<Animator>();
        }
        else
        {
            Debug.LogWarning("SceneCanvas has not been found.");
        }
    }

    public void OnPlayAgainPressed()
    {
        GameController.Instance.RestartLevel();
    }

    public void UpdateHighScore(int hs)
    {
        highScoreText.text = hs.ToString();
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void SceneCanvasTrigger(string animName)
    {
        if (sceneCanvasAnim != null)
        {
            sceneCanvasAnim.SetTrigger(animName);
        }
    }

}
