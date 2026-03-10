using UnityEngine;
using UnityEngine.SceneManagement; // Ganz wichtig für den Szenenwechsel!

public class MenuNavigation : MonoBehaviour
{
    public void SpielStarten()
    {
        // Lädt die Szene mit dem Namen "Level1"
        // Alternativ kannst du auch den Index nutzen: SceneManager.LoadScene(1);
        SceneManager.LoadScene("SampleScene");
    }

    public void SpielBeenden()
    {
        Application.Quit();
        Debug.Log("Das Spiel wurde beendet."); // Erscheint nur im Editor-Log
    }
}
