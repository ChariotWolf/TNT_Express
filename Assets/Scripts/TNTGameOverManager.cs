using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class TNTGameOverManager : MonoBehaviour
{
    public GameObject defaultSelectedButton;
    public PlayerInput playerInput;


    void OnEnable()
    {
        if (playerInput != null)
        {
            playerInput.SwitchCurrentActionMap("UI");
        }

        StartCoroutine(SelectDefaultButton());
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            Vector2 navigationInput = playerInput.actions["Navigate"].ReadValue<Vector2>();

            
            if (navigationInput.magnitude > 0.1f)
            {
                EventSystem.current.SetSelectedGameObject(defaultSelectedButton);
            }
        }
    }

    private IEnumerator SelectDefaultButton()
    {
        yield return new WaitForEndOfFrame();

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(defaultSelectedButton);
    }

    public void OnReturnToMainPress()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void OnRestartPress()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

   
}
