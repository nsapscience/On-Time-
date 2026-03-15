using UnityEngine;
using System.Collections.Generic;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog; // Der Text aus dem Inspector
    [SerializeField] Dialog loseDialog;
    public MiniGameSelector selector; // Der Selector in der Szene
    private string chosenGameName; // Hier merkt sich der NPC das Spiel

    public void Interact()
    {
        chosenGameName = selector.GetRandomGameName();

        // Maus sichtbar machen, damit man die Auswahl anklicken kann
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Dialog customizedDialog = new Dialog();
        foreach (string line in dialog.Lines)
        {
            string newLine = line.Replace("[Spiel]", chosenGameName);
            customizedDialog.Lines.Add(newLine);
        }

        StartCoroutine(DialogManager.Instance.ShowDialog(customizedDialog, true, (bool wantsToPlay) => {
            if (wantsToPlay)
            {
                selector.OpenSpecificGame(chosenGameName);
            }
            else
            {
                // Wenn man "Nein" klickt, Maus wieder verstecken für normales Gameplay
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }));
    }

    public void PlayLoseDialog()
    {
        // Maus sicherheitshalber zeigen
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Wir starten den Dialog und nutzen den Callback, 
        // der aufgerufen wird, wenn der Dialog beendet wird.
        StartCoroutine(DialogManager.Instance.ShowDialog(loseDialog, false, (bool ignored) => {
            // Dieser Block wird ausgeführt, wenn der Spieler den Dialog wegklickt
            if (selector != null)
            {
                // Startet das Spiel sofort wieder mit dem Namen, den wir uns gemerkt haben
                selector.OpenSpecificGame(chosenGameName);
            }
        }));
    }
}