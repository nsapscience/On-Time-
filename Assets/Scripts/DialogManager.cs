using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] int letterPerSecond;
    
    // NEU: Für die Auswahl
    [SerializeField] GameObject choiceBox; 

    public event Action OnShowDialog;
    public event Action OnHideDialog;

    public static DialogManager Instance { get; private set; }
    private Coroutine typingCoroutine;

    private void Awake()
    {
        Instance = this;
    }

    // Variablen für den Zustand
    Dialog dialog;
    int currentLine = 0;
    bool isTyping;
    bool waitForChoice;
    Action<bool> onChoiceSelected;

    // Erweitert um Choice-Parameter
    public IEnumerator ShowDialog(Dialog dialog, bool askChoice = false, Action<bool> callback = null)
    {
        yield return new WaitForEndOfFrame();

        isTyping = false; 

        OnShowDialog?.Invoke();

        this.dialog = dialog;
        this.onChoiceSelected = callback;
        this.waitForChoice = askChoice;
        currentLine = 0;

        // WICHTIG: Hier am Anfang muss die ChoiceBox IMMER aus sein!
        choiceBox.SetActive(false); 
        dialogBox.SetActive(true);

        typingCoroutine = StartCoroutine(TypeDialog(dialog.Lines[0]));
    }

    public void HandleUpdate() 
    {
        // Wenn wir gerade tippen, erlauben wir "E" zum Überspringen (optional)
        // oder wir ignorieren es, aber wir müssen die Coroutine-Logik trennen.
        if (Input.GetKeyDown(KeyCode.E) && !isTyping)
        {
            currentLine++;

            if (currentLine < dialog.Lines.Count)
            {
                if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                typingCoroutine = StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            }
            else 
            {
                if (waitForChoice)
                {
                    choiceBox.SetActive(true);
                    // Wichtig: Wir beenden hier, damit das "E" nicht 
                    // sofort wieder den Dialog schließt, während die Buttons da sind.
                }
                else 
                {
                    EndDialog();
                }
            }
        }
    }

    // Diese Methode wird von den Buttons aufgerufen
    // Diese Funktionen sind absolut "Unity-sicher" für den Inspector
    public void SelectYes()
    {
        OnChoicePressed(true);
    }

    public void SelectNo()
    {
        OnChoicePressed(false);
    }

    // Die eigentliche Logik (jetzt privat, da die oberen gerufen werden)
    private void OnChoicePressed(bool choice)
    {
        choiceBox.SetActive(false);
        EndDialog();
        onChoiceSelected?.Invoke(choice);
    }

    private void EndDialog()
    {
        dialogBox.SetActive(false);
        currentLine = 0;

        // NEU: Wenn wir KEINE Auswahl hatten (waitForChoice = false), 
        // rufen wir den Callback trotzdem mit 'true' auf.
        // Das signalisiert dem NPC: "Der Text ist zu Ende, mach jetzt weiter!"
        if (!waitForChoice)
        {
            onChoiceSelected?.Invoke(true);
        }

        OnHideDialog?.Invoke();
    }

    public IEnumerator TypeDialog(string line)
    {
        isTyping = true;
        dialogText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / letterPerSecond);
        }
        isTyping = false;

        if(currentLine == dialog.Lines.Count - 1 && waitForChoice)
        {
            choiceBox.SetActive(true);
        }
    }
} // Diese Klammer schließt die Klasse ab.