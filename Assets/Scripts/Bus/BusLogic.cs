using UnityEngine;
using TMPro;

public class BusLogic : MonoBehaviour, AutoInteractable
{
    [Header("Bewegung")]
    public float targetSpeed = 10.0f;      // Die Endgeschwindigkeit
    public float acceleration = 2.0f;     // Wie schnell der Bus beschleunigt (Einheiten pro Sekunde)
    public float waitTime = 5.0f;
    public float finishLineX = 50.0f;

    [Header("UI & Kamera")]
    public GameObject resultPanel;
    public TextMeshProUGUI resultText;
    public GameObject busCamera;

    [Header ("Ergebnisbilder")]
    public GameObject winImage;
    public GameObject loseImage;

    [Header ("Zusätzliche UI")]    
    public GameObject timerGameObject;

    private float currentSpeed = 0.0f;    // Startet bei 0
    private float timer = 0.0f;
    private bool isPlayerInBus = false;
    private bool gameEnded = false;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (resultPanel != null) resultPanel.SetActive(false);
        if (busCamera != null) busCamera.SetActive(false);
    }

    void Update()
    {
        if (gameEnded) return;

        timer += Time.deltaTime;

        // Der Bus fährt los, wenn die Zeit um ist ODER der Spieler drin ist
        if (timer >= waitTime || isPlayerInBus)
        {
            Accelerate();
            Drive();
            CheckFinishLine();
        }
    }

    private void Accelerate()
    {
        // Erhöht die aktuelle Geschwindigkeit sanft bis zum Target-Wert
        if (currentSpeed < targetSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
            
            // Verhindert, dass wir über das Ziel hinausschießen
            currentSpeed = Mathf.Min(currentSpeed, targetSpeed);
        }
    }

    private void Drive()
    {
        // Wir nutzen jetzt currentSpeed statt speed
        transform.Translate(Vector3.right * currentSpeed * Time.deltaTime);
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
        currentSpeed = 0; // Sofortiger Stopp am Ende
        resultPanel.SetActive(true);

        if(timerGameObject != null)
        {    
            timerGameObject.SetActive(false);
        }

        if(resultPanel != null) resultPanel.SetActive(true);


        if(winImage != null) winImage.gameObject.SetActive( false); 
        if(loseImage != null) loseImage.gameObject.SetActive( false);

        if (isPlayerInBus)
        {
            winImage.gameObject.SetActive( true);
        }
        else
        {
            loseImage.gameObject.SetActive( true);
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
        isPlayerInBus = true;

        if (player != null)
        {
            if (busCamera != null) busCamera.SetActive(true);

            player.transform.SetParent(this.transform);
            player.transform.localPosition = Vector3.zero;

            if (player.TryGetComponent<SpriteRenderer>(out var renderer)) renderer.enabled = false;
            if (player.TryGetComponent<Collider2D>(out var col)) col.enabled = false;
            if (player.TryGetComponent<PlayerController>(out var controller)) controller.enabled = false;
        }
    }
}