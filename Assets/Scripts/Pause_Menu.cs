using UnityEngine;
using UnityEngine.SceneManagement;
public class Pause_Menu : MonoBehaviour
{
    private bool isPaused = false; // Hier wird sie zugewiesen

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // HIER wird der Wert benutzt!
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

    void Pause()
    {
        isPaused = true; // Wert setzen
        SceneManager.LoadScene("Pause");
    }

    void Resume()
    {
        isPaused = false; // Wert setzen
        // Logik um zur Spiel-Szene zurückzukehren
        ime.timeScale = 1f; // Zeit wieder starten
    // Entlädt nur die Pause-Szene, das Spiel im Hintergrund ist noch da
        SceneManager.UnloadSceneAsync("Pause");
    }
     public void Home()
    {
        
        SceneManager.LoadScene("Menu");

    }
}
