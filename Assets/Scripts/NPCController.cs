using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    [SerializeField] GameObject choiceUI; // Ein Panel mit "Ja" und "Nein" Buttons
    [SerializeField] GameObject targetWindow; // Das eigentliche Fenster (z.B. Shop/Minispiel)

    public void Interact()
    {
        StartCoroutine(RunInteractionSequence());
    }

    private IEnumerator RunInteractionSequence()
    {
        // 1. Warte, bis der DialogManager fertig ist
        yield return StartCoroutine(DialogManager.Instance.ShowDialog(dialog));

        // 2. Dialog ist fertig -> Zeige die Auswahl-Buttons (Ja/Nein)
        if (choiceUI != null)
        {
            choiceUI.SetActive(true);
        }
    }

    // Diese Funktion wird vom "Ja"-Button aufgerufen
    public void OnAcceptChoice()
    {
        choiceUI.SetActive(false);
        if (targetWindow != null) targetWindow.SetActive(true);
    }

    // Diese Funktion wird vom "Nein"-Button aufgerufen
    public void OnDeclineChoice()
    {
        choiceUI.SetActive(false);
        Debug.Log("Spieler hat abgelehnt.");
    }
}