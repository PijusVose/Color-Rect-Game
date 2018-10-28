using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventController {

    // Delegates and events.
    public delegate void GeneralEventHandler();
    public static event GeneralEventHandler PlayerDiedEvent;
    public static event GeneralEventHandler AddScoreEvent;
    public static event GeneralEventHandler RestartLevelEvent;
    public static event GeneralEventHandler LoadShapesEvent;
    public static event GeneralEventHandler ShowAdEvent;
    public static event GeneralEventHandler StartGameEvent;

    public delegate void AnimationEventHandler(string str);
    public static event AnimationEventHandler CanvasTriggerEvent;

    public delegate void ScoreEventHandler(int score);
    public static event ScoreEventHandler UpdateScoreEvent;
    public static event ScoreEventHandler UpdateHighScoreEvent;

    #region Event Callbacks

    public static void OnPlayerDiedEvent()
    {
        if (PlayerDiedEvent != null)
        {
            PlayerDiedEvent();
        }
    }

    public static void OnAddScoreEvent()
    {
        if (AddScoreEvent != null)
        {
            AddScoreEvent();
        }
    }

    public static void OnRestartLevelEvent()
    {
        if (RestartLevelEvent != null)
        {
            RestartLevelEvent();
        }
    }

    public static void OnLoadShapesEvent()
    {
        if (LoadShapesEvent != null)
        {
            LoadShapesEvent();
        }
    }

    public static void OnShowAdEvent()
    {
        if (ShowAdEvent != null)
        {
            ShowAdEvent();
        }
    }

    public static void OnStartGameEvent()
    {
        if (StartGameEvent != null)
        {
            StartGameEvent();
        }
    }

    public static void OnCanvasTriggerEvent(string str)
    {
        if (CanvasTriggerEvent != null)
        {
            CanvasTriggerEvent(str);
        }
    }

    public static void OnUpdateScoreEvent(int score)
    {
        if (UpdateScoreEvent != null)
        {
            UpdateScoreEvent(score);
        }
    }

    public static void OnUpdateHighScoreEvent(int score)
    {
        if (UpdateHighScoreEvent != null)
        {
            UpdateHighScoreEvent(score);
        }
    }

    #endregion

}
