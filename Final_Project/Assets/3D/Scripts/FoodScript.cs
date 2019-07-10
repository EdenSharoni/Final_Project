using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Pacman3D"))
        {
            // set points + 10
            Destroy(transform.gameObject);
        }
    }
}
