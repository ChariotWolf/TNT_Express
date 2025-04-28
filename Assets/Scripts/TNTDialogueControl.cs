using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TNTDialogueControl : MonoBehaviour
{
    [Header("Components")]
    public GameObject dialogueObj;
    public Text actorNameText;
    public Text speechText;
    private TNTDialogue Dialogue;

    [Header("Settings")]
    public float typingSpeed;
    private string[] sentences;
    private int index;
    private Coroutine typingCoroutine;
    


    
    void Update()
    {
        if (!dialogueObj.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.J) || (Gamepad.current != null && Gamepad.current.buttonNorth.wasPressedThisFrame))
        {
            NextSentence();
        }
    }

    public void Speech(string[] txt, string actorName, TNTDialogue currentDialog)
    {
        Dialogue = currentDialog;
        if(typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        dialogueObj.SetActive(true);
        speechText.text = "";
        actorNameText.text = actorName;
        sentences = txt;
        index = 0;
        typingCoroutine = StartCoroutine(TypeSentence());
    }

    
    IEnumerator TypeSentence()
    {
        speechText.text = "";
        foreach(char letter in sentences[index].ToCharArray())
        {
            speechText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        typingCoroutine = null;
    }

    public void NextSentence()
    {

        if(speechText.text != sentences[index])
        {
            if(typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            speechText.text = sentences[index];
        }


        else if(speechText.text == sentences[index])
        {
            if (index < sentences.Length - 1)
            {
                index++;
                speechText.text = "";
                if(typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                }
                typingCoroutine = StartCoroutine(TypeSentence());
            }
            else
            {
                EndDialogue();
                
            }
        }
    }

    public void HidePanel()
    {
        if(typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        speechText.text = "";
        actorNameText.text = "";
        index = 0;
        dialogueObj.SetActive(false);
        if(Dialogue != null)
        {
            Dialogue.isDialogueActive = false;
        }
    }

    public void EndDialogue()
    {
        speechText.text = "";
        index = 0;
        dialogueObj.SetActive(false);
        if (Dialogue != null)
        {
            Dialogue.isDialogueActive = false;
        }
    }
}
