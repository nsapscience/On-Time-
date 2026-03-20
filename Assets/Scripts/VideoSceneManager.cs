using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoSceneManager : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    public string nextSceneName = "Welt1"; // Name deiner Spielwelt-Szene

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        // Dieses Event feuert, wenn das Video zu Ende ist
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void Update()
    {
        // Bonus: Mit Leertaste oder Esc das Video überspringen
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {
            LoadNextScene();
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        LoadNextScene();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}