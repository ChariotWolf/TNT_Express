using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTEnergeticosBase : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject);
        }
    }
}
