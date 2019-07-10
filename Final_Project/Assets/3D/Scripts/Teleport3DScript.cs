using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport3DScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.transform.Translate(0, 0, -100);
    }
}
