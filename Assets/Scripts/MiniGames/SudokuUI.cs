using UnityEngine;
using TMPro;

public class SudokuUI : MonoBehaviour
{
    public GameObject cellPrefab;
    public Transform gridParent;
    
    private TMP_InputField[,] allCells = new TMP_InputField[9, 9];
    private SudokuLogic logic = new SudokuLogic();

    // WICHTIG: Diese Methode muss Start() heißen, damit Unity sie beim Play-Drücken aufruft!
    void Start()
    {
        if (cellPrefab == null || gridParent == null)
        {
            Debug.LogError("Bitte Prefab und GridParent im Inspector zuweisen!");
            return;
        }
        CreateGrid();
    }

    void CreateGrid()
    {
        // Wir löschen zur Sicherheit alte Kinder (falls vorhanden)
        foreach (Transform child in gridParent) { Destroy(child.gameObject); }

        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                GameObject newCell = Instantiate(cellPrefab, gridParent);
                TMP_InputField input = newCell.GetComponent<TMP_InputField>();
                
                // WICHTIG: Hier füllen wir das Array!
                allCells[r, r] = input; 

                int val = logic.Puzzle[r, c];
                if (val != 0)
                {
                    input.text = val.ToString();
                    input.interactable = false;
                    input.image.color = Color.gray;
                }
                else
                {
                    input.text = "";
                }
            }
        }
        Debug.Log("Grid erfolgreich mit 81 Feldern erstellt!");
    }

    public void CheckSolution()
    {
        // Wenn hier 'null' kommt, wurde die Schleife oben nicht korrekt ausgeführt
        if (allCells[0, 0] == null)
        {
            Debug.LogError("Die Felder wurden nicht im Array gespeichert!");
            return;
        }
        // ... restlicher Check-Code
    }
}