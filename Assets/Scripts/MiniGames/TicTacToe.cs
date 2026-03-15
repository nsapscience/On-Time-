using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TicTacToeManager : MonoBehaviour
{
    public Button[] cells;          
    public TextMeshProUGUI statusText; 

    private string[] board = new string[9];
    private bool playerXTurn = true; 
    private bool gameEnded = false;

    void Start()
    {
        ResetGame();
    }

    public void MakeMove(int index)
    {
        // Sicherheitssperre: Nicht klicken wenn Spiel vorbei, NPC dran oder Feld belegt
        if (gameEnded || !playerXTurn || !string.IsNullOrEmpty(board[index])) 
        {
            return;
        }

        // 1. Spieler macht seinen Zug
        ExecuteMove(index, "X");

        // 2. Wenn das Spiel durch den Zug nicht beendet wurde, ist der NPC dran
        if (!gameEnded)
        {
            playerXTurn = false; // Klicks für den Spieler sperren
            statusText.text = "NPC überlegt...";
            StartCoroutine(NPCMoveCoroutine());
        }
    }

    IEnumerator NPCMoveCoroutine()
    {
        // Kurze Verzögerung für den "Lerneffekt"
        yield return new WaitForSecondsRealtime(0.6f); 

        List<int> freeIndices = new List<int>();
        for (int i = 0; i < board.Length; i++)
        {
            if (string.IsNullOrEmpty(board[i])) freeIndices.Add(i);
        }

        if (freeIndices.Count > 0 && !gameEnded)
        {
            int randomIndex = freeIndices[Random.Range(0, freeIndices.Count)];
            ExecuteMove(randomIndex, "O");
        }

        // NPC fertig, Spieler wieder freischalten (falls niemand gewonnen hat)
        if (!gameEnded)
        {
            playerXTurn = true;
            statusText.text = "Du bist dran (X)";
        }
    }

    void ExecuteMove(int index, string symbol)
    {
        board[index] = symbol;
        
        // Visuelle Aktualisierung des Buttons
        TextMeshProUGUI buttonText = cells[index].GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null) 
        {
            buttonText.text = symbol;
        }

        // Prüfung auf Sieg oder Unentschieden
        if (CheckWin())
        {
            gameEnded = true;
            
            if (symbol == "X") 
            {
                statusText.text = "Du hast gewonnen!";
                StartCoroutine(CloseAfterDelay(1.5f, false)); // Spieler hat gewonnen
            }
            else 
            {
                statusText.text = "NPC hat gewonnen!";
                StartCoroutine(CloseAfterDelay(1.5f, true)); // NPC hat gewonnen
            }
        }
        else if (CheckDraw())
        {
            statusText.text = "Unentschieden!";
            gameEnded = true;
            StartCoroutine(CloseAfterDelay(1.5f, false)); // Bei Unentschieden kein Verloren-Dialog
        }
    }

    // Coroutine zum Schließen und optionalen Starten des Verloren-Dialogs
    IEnumerator CloseAfterDelay(float delay, bool npcWon)
    {
        yield return new WaitForSecondsRealtime(delay);
        
        // 1. MiniGame-Fenster über den Selector schließen
        MiniGameSelector selector = Object.FindFirstObjectByType<MiniGameSelector>();
        if (selector != null) 
        {
            selector.CloseWindow();
        }

        // 2. Wenn der NPC gewonnen hat, den speziellen Dialog beim NPC starten
        if (npcWon)
        {
            NPCController npc = Object.FindFirstObjectByType<NPCController>();
            if (npc != null)
            {
                npc.PlayLoseDialog();
            }
        }
    }

    bool CheckWin()
    {
        int[,] winConditions = new int[,] { 
            {0,1,2}, {3,4,5}, {6,7,8}, // Horizontal
            {0,3,6}, {1,4,7}, {2,5,8}, // Vertikal
            {0,4,8}, {2,4,6}           // Diagonal
        };

        for (int i = 0; i < 8; i++)
        {
            if (!string.IsNullOrEmpty(board[winConditions[i,0]]) &&
                board[winConditions[i,0]] == board[winConditions[i,1]] &&
                board[winConditions[i,1]] == board[winConditions[i,2]])
            {
                return true;
            }
        }
        return false;
    }

    bool CheckDraw()
    {
        foreach (string cell in board) 
        {
            if (string.IsNullOrEmpty(cell)) return false;
        }
        return true;
    }

    public void ResetGame()
    {
        board = new string[9];
        for (int i = 0; i < 9; i++) board[i] = "";
        
        playerXTurn = true;
        gameEnded = false;
        statusText.text = "Du bist dran (X)";

        foreach (var cell in cells)
        {
            if (cell != null)
            {
                var txt = cell.GetComponentInChildren<TextMeshProUGUI>();
                if (txt != null) txt.text = "";
            }
        }
    }
}