using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UnityZahlenraten : MonoBehaviour
{
    private int gesuchteZahl;
    
    [Header("UI Referenzen")]
    public TMP_InputField eingabeFeld; 
    public TMP_Text feedbackText;      
    public Button rateButton;          

    void Start()
    {
        // Spiel initialisieren
        gesuchteZahl = Random.Range(1, 101);
        feedbackText.text = "Rate eine Zahl zwischen 1 und 100!";
        
        // Button-Klick im UI zuweisen
        rateButton.onClick.AddListener(CheckZahl);

        // Sorgt dafür, dass man sofort tippen kann, ohne erst ins Feld zu klicken
        eingabeFeld.ActivateInputField();
    }

    void Update()
    {
        // Prüfen, ob Enter oder Numpad-Enter gedrückt wurde
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CheckZahl();
        }
    }

    void CheckZahl()
    {
        // Nur prüfen, wenn das Feld nicht leer ist
        if (string.IsNullOrEmpty(eingabeFeld.text)) return;

        if (int.TryParse(eingabeFeld.text, out int tipp))
        {
            if (tipp < gesuchteZahl)
            {
                feedbackText.text = "Die Zahl ist GRÖSSER!";
                PrepareNextInput();
            }
            else if (tipp > gesuchteZahl)
            {
                feedbackText.text = "Die Zahl ist KLEINER!";
                PrepareNextInput();
            }
            else
            {
                feedbackText.text = "Richtig! Gut gemacht.";
                // Button deaktivieren, damit man nicht mehrfach "Richtig" auslöst
                rateButton.interactable = false;
                StartCoroutine(CloseAfterDelay(2.0f)); 
            }
        }
    }

    // Hilfsfunktion: Feld leeren und Cursor wieder reinsetzen
    void PrepareNextInput()
    {
        eingabeFeld.text = "";
        eingabeFeld.ActivateInputField();
    }

    IEnumerator CloseAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        
        MiniGameSelector selector = Object.FindFirstObjectByType<MiniGameSelector>();
        if (selector != null) 
        {
            selector.CloseWindow();
        }
    }
}