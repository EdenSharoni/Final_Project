using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    TeleportScript teleportScript;
    TeleportScript teleport2Script;
    bool oneTimeEntrence;

    private void Start()
    {
        oneTimeEntrence = true;
        teleportScript = GameObject.Find("Teleport").GetComponent<TeleportScript>();
        teleport2Script = GameObject.Find("Teleport2").GetComponent<TeleportScript>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (transform.name.Equals("Teleport") && teleport2Script.oneTimeEntrence)
        {
            oneTimeEntrence = false;
            collision.gameObject.transform.position = teleport2Script.transform.position;
            StartCoroutine(Wait());
        }

        if (transform.name.Equals("Teleport2") && teleportScript.oneTimeEntrence)
        {
            oneTimeEntrence = false;
            collision.gameObject.transform.position = teleportScript.transform.position;
            StartCoroutine(Wait());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (transform.name.Equals("Teleport") && teleport2Script.oneTimeEntrence)
        {
            oneTimeEntrence = false;
            collision.gameObject.transform.position = teleport2Script.transform.position;
            StartCoroutine(Wait());
        }

        if (transform.name.Equals("Teleport2") && teleportScript.oneTimeEntrence)
        {
            oneTimeEntrence = false;
            collision.gameObject.transform.position = teleportScript.transform.position;
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.1f);
        oneTimeEntrence = true;
    }
}
