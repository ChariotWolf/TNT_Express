using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TNTStartMenuUIManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject creditsMenu;
    public GameObject firstSelectedButton;
    public GameObject returnFromCreditsButton;

    private PlayerInput playerInput;

    void Start()
    {
        
        playerInput = GetComponent<PlayerInput>();

        playerInput.SwitchCurrentActionMap("UI");

        if (firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }

    void Update()
    {
        CheckAndRestoreSelectedButton();
    }



    public void OnStartPress()
    {
        SceneManager.LoadScene(1);
    }



    public void OnCreditsPress()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);

        Invoke(nameof(SelectReturnButton), 0f);
    }

    private void SelectReturnButton()
    {
        if (returnFromCreditsButton != null && EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(returnFromCreditsButton);
        }
    }


    public void OnReturnFromCredits()
    {
        creditsMenu.SetActive(false);
        mainMenu.SetActive(true);

        if (firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }

    private void CheckAndRestoreSelectedButton()
    {
        if (EventSystem.current.currentSelectedGameObject != null) return;

        Vector2 navigationInput = playerInput.actions["Navigate"].ReadValue<Vector2>();

        if (navigationInput.magnitude > 0.1f)
        {
            if (creditsMenu.activeSelf && returnFromCreditsButton != null)
            {
                EventSystem.current.SetSelectedGameObject(returnFromCreditsButton);
            }
            else if (mainMenu.activeSelf && firstSelectedButton != null)
            {
                EventSystem.current.SetSelectedGameObject(firstSelectedButton);
            }
        }
    }

    public void OnGameExit()
    {
        Application.Quit();
    }

}

