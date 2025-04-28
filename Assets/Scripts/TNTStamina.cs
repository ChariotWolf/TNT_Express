using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TNTStamina : MonoBehaviour
{
    public float playerStamina = 100f;
    public Image Slider01;
    public float regenPorSegundo = -2.5f;
    public AudioSource energeticoSound;


    void Update()
    {

        playerStamina += regenPorSegundo * Time.deltaTime;

        playerStamina = Mathf.Clamp(playerStamina, 0f, 100f);

        Slider01.fillAmount = playerStamina * 0.01f;

        if (playerStamina <= 0)
        {
            SceneManager.LoadScene(4);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TNTEnergyDrink"))
        {
            playerStamina += 30f;
            if (!energeticoSound.isPlaying)
            {
                energeticoSound.Play();
            }
        }

        if (collision.CompareTag("power up"))
        {
            playerStamina += 30f;
            if (!energeticoSound.isPlaying)
            {
                energeticoSound.Play();
            }
        }
    }
}
