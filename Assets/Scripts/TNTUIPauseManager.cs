using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TNTUIPauseManager : MonoBehaviour
{
    public GameObject pauseUI;
    public GameObject defaultSelectedButton;
    public PlayerInput playerInput;

    public bool isPaused = false;

    void Update()
    {
        OnEnterPausePress();

            if (EventSystem.current.currentSelectedGameObject == null)
            {
                Vector2 navigationInput = playerInput.actions["Navigate"].ReadValue<Vector2>();


                if (navigationInput.magnitude > 0.1f)
                {
                    EventSystem.current.SetSelectedGameObject(defaultSelectedButton);
                }
            }

    }

    public void OnRestartPress()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isPaused = false;
        Time.timeScale = 1;
        playerInput.SwitchCurrentActionMap("PlayerTNT");
    }

    public void OnGameResumePress()
    {
        pauseUI.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
        playerInput.SwitchCurrentActionMap("PlayerTNT");
    }

    public void OnMainMenuPress()
    {
        isPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        playerInput.SwitchCurrentActionMap("UI");

    }

    public void OnEnterPausePress()
    {
        if (Input.GetKeyDown(KeyCode.P) || (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame))
        {
            if (!isPaused)
            {
                pauseUI.SetActive(true);
                isPaused = true;
                Time.timeScale = 0;

                playerInput.SwitchCurrentActionMap("UI");

                StartCoroutine(SelectDefaultButton());
            }
            else
            {
                pauseUI.SetActive(false);
                isPaused = false;
                Time.timeScale = 1;
            }
        }
    }

    private IEnumerator SelectDefaultButton()
    {
        yield return new WaitForEndOfFrame();

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(defaultSelectedButton);
    }

}
