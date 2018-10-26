using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonScript : MonoBehaviour {

    // Call when the "Play Again" button is pressed.
    public void OnButtonPressed(string type)
    {
        if (type == "restart")
        {
            GameController.Instance.RestartLevel();
        }
        else if(type == "play")
        {
            UIController.Instance.SceneCanvasTrigger("StartGame");

            GameController.Instance.inStartMenu = false;
            GameController.Instance.StartGame();
        }
    }
}
