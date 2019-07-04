using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{ 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.transform.Translate(-70, 0, 0);
    }
}
