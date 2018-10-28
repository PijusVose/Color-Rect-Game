using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonScript : MonoBehaviour {

    // Call when the "Play Again" button is pressed.
    public void OnButtonPressed(string type)
    {
        if (type == "restart")
        {
            EventController.OnRestartLevelEvent();
        }
        else if(type == "play")
        {
            EventController.OnCanvasTriggerEvent("StartGame");
            EventController.OnStartGameEvent();
        }
    }
}
