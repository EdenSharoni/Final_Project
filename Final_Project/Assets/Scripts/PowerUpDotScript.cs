using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDotScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("pacman"))
        {
            PlayerPrefs.SetInt("GostBlue", 1);
            PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + 50);
            Destroy(transform.gameObject);
        }
    }
}
