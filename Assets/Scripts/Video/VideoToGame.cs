using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoToGame : MonoBehaviour
{
    public VideoPlayer myVideoPlayer;
    public string sceneToLoad = "SampleScene";

    void Start()
    {
        myVideoPlayer.loopPointReached += LoadScene;
    }

    void LoadScene(VideoPlayer vp)
    {
        SceneManager.LoadScene("SampleScene");
    }
}