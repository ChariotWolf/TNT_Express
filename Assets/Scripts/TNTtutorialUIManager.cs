using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TNTtutorialUIManager : MonoBehaviour
{
    public GameObject playButton;
    public GameObject Gamepad;
    public GameObject Keyboard;
    private PlayerInput playerInput;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.SwitchCurrentActionMap("UI");

        Invoke(nameof(SelectPlayButton), 0f);
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            Vector2 navigationInput = playerInput.actions["Navigate"].ReadValue<Vector2>();

            if (navigationInput.magnitude > 0.1f)
            {
                EventSystem.current.SetSelectedGameObject(playButton);
            }
        }
    }

    public void OnGamepadKeyboardPress()
    {

        if (Gamepad.activeSelf)
        {
            Gamepad.SetActive(false);
            Keyboard.SetActive(true);
        }
        else if (Keyboard.activeSelf)
        {
            Keyboard.SetActive(false);
            Gamepad.SetActive(true);
        }
        else
        {
            Gamepad.SetActive(true);
        }
    }


    private void SelectPlayButton()
    {
        if (playButton != null && EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(playButton);
        }
    }

    public void OnPlayPress()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
    }
}
