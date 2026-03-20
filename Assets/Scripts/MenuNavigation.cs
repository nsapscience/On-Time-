using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video; // Wichtig für den VideoPlayer!

public class MenuNavigation : MonoBehaviour
{
    public GameObject videoOverlay; // Das Raw Image mit dem Video
    public VideoPlayer videoPlayer;
    public string weltSzene = "SampleScene";

    void Start()
    {
        // Event registrieren: Was passiert, wenn das Video fertig ist?
        if (videoPlayer != null)
            videoPlayer.loopPointReached += CheckOver;
    }

    public void SpielStarten()
    {
        // Menü ausblenden (optional) und Video starten
        videoOverlay.SetActive(true);
        videoPlayer.Play();
    }

    void CheckOver(VideoPlayer vp)
    {
        SceneManager.LoadScene(weltSzene);
    }

    public void SpielBeenden()
    {
        Application.Quit();
        Debug.Log("Das Spiel wurde beendet.");
    }
}