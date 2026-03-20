using UnityEngine;
using TMPro; // Wichtig für TextMeshPro

public class BusTimer : MonoBehaviour
{
    public float restZeit = 60f;
    public bool timerLaeuft = false;
    public TextMeshProUGUI timerText; // Hier das UI-Element im Inspector reinziehen

    void Update()
    {
        if (timerLaeuft)
        {
            if (restZeit > 0)
            {
                restZeit -= Time.deltaTime;
                DisplayTime(restZeit);
            }
            else
            {
                restZeit = 0;
                timerLaeuft = false;
                Debug.Log("Bus ist weg!");
            }
        }
    }

    void DisplayTime(float zeitAnzeige)
    {
        float minuten = Mathf.FloorToInt(zeitAnzeige / 60); 
        float sekunden = Mathf.FloorToInt(zeitAnzeige % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minuten, sekunden);
    }

    // Diese Funktion aufrufen, wenn der Bus startet
    public void StartBusTimer()
    {
        timerLaeuft = true;
    }
}