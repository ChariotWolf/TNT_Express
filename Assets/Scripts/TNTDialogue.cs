using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTDialogue : MonoBehaviour
{

    public string[] speechText;
    public string actorName;
    private TNTDialogueControl dc;
    private bool onRadius;
    public bool isDialogueActive = false;
    public LayerMask playerLayer;
    public float radius;


    void Start()
    {
        dc = FindObjectOfType<TNTDialogueControl>();
    }

    private void FixedUpdate()
    {
        Interact();
    }
    
    void Update()
    {
        if (onRadius && !isDialogueActive)
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        isDialogueActive = true;
        dc.Speech(speechText, actorName, this);

    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        dc.HidePanel();
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void Interact()
    {
        

        Collider2D hits = Physics2D.OverlapCircle(transform.position, radius, playerLayer);
        


        if(hits != null)
        {
            
            onRadius = true;
        }
        else
        {
            
            onRadius = false;

            if (isDialogueActive)
            {
                EndDialogue();
            }
        }
    }


}
