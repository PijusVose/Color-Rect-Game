using UnityEngine;
using UnityEngine.Advertisements;

public class AdController : MonoBehaviour {

    // Singleton.
    public static AdController Instance { get; set; }

    public float chanceOfAd = 50f; // Chance of getting an ad when requested.

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

    public void ShowAd()
    {
        bool showAd = CalculateAdChance();

        if (showAd && Advertisement.IsReady("video"))
        {
            Debug.Log("Showing video ad.");

            Advertisement.Show("video", new ShowOptions() { resultCallback = HandleAdResult });
        }
    }

    private bool CalculateAdChance()
    {    
        float percentage = Random.Range(0f, 100f);
        if (percentage <= chanceOfAd)
        {
            return true;
        }

        return false;
    }

    private void HandleAdResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Failed:
                Debug.Log("The Ad failed to play.");
                break;
            case ShowResult.Skipped:
                Debug.Log("The Ad was skipped.");
                break;
            case ShowResult.Finished:
                Debug.Log("The Ad was finished.");
                break;
            default:
                break;
        }
    }

}
