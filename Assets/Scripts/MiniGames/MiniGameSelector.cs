using UnityEngine;
using System.Collections.Generic;

public class MiniGameSelector : MonoBehaviour
{
    [Header("Einstellungen")]
    public GameObject miniGameWindow;
    public Transform gameContainer;
    public List<GameObject> allMiniGamePrefabs;

    // Diese Funktion gibt nur den Namen eines zufälligen Spiels zurück
    public string GetRandomGameName()
    {
        if (allMiniGamePrefabs == null || allMiniGamePrefabs.Count == 0) 
            return "ein Spiel";

        int randomIndex = Random.Range(0, allMiniGamePrefabs.Count);
        return allMiniGamePrefabs[randomIndex].name;
    }

    // Diese Funktion öffnet ein ganz bestimmtes Spiel anhand des Namens
    public void OpenSpecificGame(string gameName)
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        miniGameWindow.SetActive(true);

        // Alten Inhalt löschen
        foreach (Transform child in gameContainer)
        {
            Destroy(child.gameObject);
        }

        // Das passende Prefab finden
        GameObject prefab = allMiniGamePrefabs.Find(p => p.name == gameName);

        if (prefab != null)
        {
            GameObject spawnedGame = Instantiate(prefab, gameContainer);
            
            // UI-Positionierung sicherstellen
            RectTransform rect = spawnedGame.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchoredPosition = Vector2.zero;
            }
        }
        else
        {
            Debug.LogError("Fehler: Kein Prefab mit dem Namen " + gameName + " gefunden!");
        }
    }

    public void CloseWindow()
    {
        miniGameWindow.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}