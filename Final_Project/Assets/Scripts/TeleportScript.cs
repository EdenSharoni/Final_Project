using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{ 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(transform.name.Equals("Teleport"))
            collision.gameObject.transform.Translate(-65, 0, 0);
        if (transform.name.Equals("Teleport2"))
            collision.gameObject.transform.Translate(65, 0, 0);

    }
}
