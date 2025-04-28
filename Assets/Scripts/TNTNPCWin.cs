using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class TNTNPCWin : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D playerRigidbody;
    public Animator playerAnimator;
    public MonoBehaviour TNTPlayerMovement; 
    public MonoBehaviour TNTStamina;
    public GameObject ENDdialogueCanvas;
    public AudioSource energeticoSound;
    public Animator npcAnimator;
    public Rigidbody2D npcRigidbody;
    public float npcRunSpeed = 5f;
    public Transform npc;
    public Transform targetPoint;

    private bool isSequenceStarted = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isSequenceStarted)
        {
            isSequenceStarted = true;
            StartCoroutine(EndLevelSequence());
        }
    }

    IEnumerator EndLevelSequence()
    {

        player.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        TNTPlayerMovement.enabled = false;
        TNTStamina.enabled = false;
        playerRigidbody.velocity = Vector2.zero;
        playerAnimator.SetBool("isRunning", false);
        playerAnimator.SetBool("isJumping", false);
        playerAnimator.SetBool("isWallSliding", false);
        playerAnimator.SetBool("isDashing", false);



        ENDdialogueCanvas.SetActive(true);


        yield return new WaitForSeconds(2f);



        energeticoSound.Play();
        npcAnimator.SetTrigger("Drink");

        yield return new WaitForSeconds(3f);

        ENDdialogueCanvas.SetActive(false);

        npcAnimator.SetTrigger("Run");


        npcRigidbody.velocity = new Vector2(-npcRunSpeed, npcRigidbody.velocity.y);


        yield return new WaitUntil(() => Vector2.Distance(npc.position, targetPoint.position) < 0.5f);


        SceneManager.LoadScene(3);
    }

}
