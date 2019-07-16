using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport3DScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pacman3D"))
            collision.gameObject.transform.Translate(0, 0, -100);
        else if (collision.gameObject.CompareTag("Ghost3D"))
            collision.gameObject.transform.Translate(-100, 0, 0);
    }
}
