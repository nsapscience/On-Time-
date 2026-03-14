using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_Menu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public bool isPaused = false;

    private bool inputBlocked = false;

    void Update()
    {
        if (inputBlocked) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        StartCoroutine(BlockInput());
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    System.Collections.IEnumerator BlockInput()
    {
        inputBlocked = true;
        yield return new WaitForSecondsRealtime(0.2f);
        inputBlocked = false;
    }

    public void Home()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}