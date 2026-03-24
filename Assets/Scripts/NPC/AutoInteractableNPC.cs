using UnityEngine;
using System.Collections;

public class AutoInteractNPC : MonoBehaviour
{
    [Header("Einstellungen")]
    public float detectionRange = 5f;      // Ab wann der NPC dich sieht
    public float interactRange = 1.5f;     // Abstand beim Anhalten
    public float moveSpeed = 3f;
    public float interactionDuration = 3f; // Wie lange er beim Spieler bleibt

    [Header("Referenzen")]
    public Transform player;               
    private Vector3 startPosition;         
    private PlayerController playerCtrl; 
    
    private bool hasInteracted = false;    
    private bool isReturning = false;      
    private bool isInteractingNow = false; 

    void Start()
    {
        startPosition = transform.position;
        if (player == null) player = GameObject.FindWithTag("Player").transform;
        if (player != null) playerCtrl = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (hasInteracted && !isReturning) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // 1. ZUM SPIELER GEHEN
        if (!hasInteracted && distance <= detectionRange && distance > interactRange)
        {
            if(playerCtrl != null) playerCtrl.enabled = false; // Spieler stoppen
            MoveTowards(player.position);
        }
        // 2. INTERAGIEREN (Sobald nah genug dran)
        else if (!hasInteracted && distance <= interactRange && !isInteractingNow)
        {
            StartCoroutine(HandleInteraction());
        }
        // 3. ZURÜCKGEHEN (Nach dem Spiel)
        else if (isReturning)
        {
            MoveTowards(startPosition);
            if (Vector2.Distance(transform.position, startPosition) < 0.1f)
            {
                isReturning = false;
            }
        }
    }

    void MoveTowards(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        // Sprite spiegeln
        if (target.x > transform.position.x) transform.localScale = new Vector3(1, 1, 1);
        else transform.localScale = new Vector3(-1, 1, 1);
    }

    IEnumerator HandleInteraction()
    {
        isInteractingNow = true;

        // Er sucht auf dem NPC nach dem Skript, das die eigentliche Interaktion (Dialog/Minispiel) enthält
        AutoInteractable interactScript = GetComponent<AutoInteractable>();
        if (interactScript != null)
        {
            interactScript.Interact(); 
        }

        yield return new WaitForSeconds(interactionDuration);

        if(playerCtrl != null) playerCtrl.enabled = true; // Spieler wieder frei

        isInteractingNow = false;
        hasInteracted = true;
        isReturning = true;
    }
}