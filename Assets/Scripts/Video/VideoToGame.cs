using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoToGame : MonoBehaviour
{
    public VideoPlayer myVideoPlayer;
    public string sceneToLoad = "Level1";

    void Start()
    {
        // Wir abonnieren das Ereignis: Was passiert, wenn das Video fertig ist?
        myVideoPlayer.loopPointReached += LoadScene;
    }

    void LoadScene(VideoPlayer vp)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}