using UnityEngine;
using TMPro;

public class BusLogic : MonoBehaviour, Interactable
{
    [Header("Bewegung")]
    public float speed = 10.0f;
    public float waitTime = 5.0f;
    public float finishLineX = 50.0f;

    [Header("UI & Kamera")]
    public GameObject resultPanel;
    public TextMeshProUGUI resultText;
    public GameObject busCamera; // Ziehe hier eine Kamera rein, die am Bus klebt

    private float timer = 0.0f;
    private bool isPlayerInBus = false;
    private bool gameEnded = false;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (resultPanel != null) resultPanel.SetActive(false);
        if (busCamera != null) busCamera.SetActive(false); // Bus-Kamera am Anfang aus
    }

    void Update()
    {
        if (gameEnded) return;

        timer += Time.deltaTime;

        if (timer >= waitTime || isPlayerInBus)
        {
            Drive();
            CheckFinishLine();
        }
    }

    private void Drive()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void CheckFinishLine()
    {
        if (transform.position.x >= finishLineX)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        gameEnded = true;
        speed = 0;
        resultPanel.SetActive(true);

        if (isPlayerInBus)
        {
            resultText.text = "Gewonnen!";
            resultText.color = Color.green;
        }
        else
        {
            // Falls verloren, Kamera am Bus lassen, damit man sieht wie er wegfährt
            resultText.text = "Verloren!";
            resultText.color = Color.red;
        }
    }

    public void Interact()
    {
        if (!isPlayerInBus && !gameEnded)
        {
            EnterBus();
        }
    }

    private void EnterBus()
    {
        Debug.Log("Einsteigen...");
        isPlayerInBus = true;

        if (player != null)
        {
            // ERST die Bus-Kamera an, damit Unity immer ein Bild hat
            if (busCamera != null) 
            {
                busCamera.SetActive(true);
                Debug.Log("Bus-Kamera wurde aktiviert.");
            }
            else 
            {
                Debug.LogError("Fehler: Keine Bus-Kamera im Inspector zugewiesen!");
            }

            // DANN den Spieler an den Bus binden
            player.transform.SetParent(this.transform);
            player.transform.localPosition = Vector3.zero;

            // Anstatt SetActive(false), deaktivieren wir nur die Komponenten,
            // um den Coroutine-Fehler im GameController zu vermeiden:
            if (player.TryGetComponent<SpriteRenderer>(out var renderer)) renderer.enabled = false;
            if (player.TryGetComponent<Collider2D>(out var col)) col.enabled = false;
            if (player.TryGetComponent<PlayerController>(out var controller)) controller.enabled = false;
        }
    }
}