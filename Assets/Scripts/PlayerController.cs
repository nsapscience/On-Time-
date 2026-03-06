using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    private bool isMoving;

    private Vector2 input;

    private Animator animator;

    public LayerMask solidObjectsLayer;
    public LayerMask interacableLayer;
    public LayerMask battleLayer;


    private void Awake()
    {

        animator = GetComponent<Animator>();    

    }

    public void HandleUdpdate()
    {

        if(!isMoving)
        {

            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            //Debug.Log("This is input.x" + input.x);
            //Debug.Log("This is input.y" + input.y);

            if(input != Vector2.zero)
            {

                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if(isWalkable(targetPos))
                    StartCoroutine(Move(targetPos));

            }

        }

        animator.SetBool("isMoving", isMoving);

        if(Input.GetKeyDown(KeyCode.E))
            Interact();


    }

    void Interact()
    {
    
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir; 
    
        //Debug.DrawLine(transform.position, interactPos, Color.red, 1f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.2f, interacableLayer);
        if(collider != null)
        {
        
            collider.GetComponent<Interactable>()?.Interact();

        }

    }

    IEnumerator Move(Vector3 targetPos)
    {

        isMoving = true;

        while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {

            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;

        }

        transform.position = targetPos;

        isMoving = false;

        CheckForEncounters();

    }

    private bool isWalkable(Vector3 targetPos)
    {
        if(Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer | interacableLayer) != null)
        {
            return false;
        }

        return true;

    }

    private void CheckForEncounters()
    {
        
        if(Physics2D.OverlapCircle(transform.position, 0.01f, battleLayer) != null)
        {
            if(Random.Range(1, 100) <= 50)
            {
                
                Debug.Log("A battle has started!");

            }
            
        }

    }

}
