using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GostControllerScript : MonoBehaviour
{
    public LayerMask layermask;
    RaycastHit2D[] hitRound;

    GostAI gostAI;
    GostScript gostScript;

    bool oneTimeEntrence;

    void Start()
    {
        oneTimeEntrence = true;
        gostAI = GameObject.Find(transform.name).GetComponent<GostAI>();
        gostAI.enabled = false;
        gostScript = GameObject.Find(transform.name).GetComponent<GostScript>();
        gostScript.enabled = true;
    }

    void FixedUpdate()
    {
        MakeRayCastHit2D();
        if (gostScript.startFindingPacman)
        {
            if (PlayerPrefs.GetInt("GostBlue", 0) == 1)
            {
                gostAI.enabled = false;
                gostScript.enabled = true;
            }
            else
            if (oneTimeEntrence)
            {
                oneTimeEntrence = false;

                if (FindingPacmanWithRayCast2D())
                {
                    gostAI.enabled = true;
                    gostScript.enabled = false;
                    StartCoroutine(FindPacmanAgain());
                }
                else
                {
                    oneTimeEntrence = true;
                    gostAI.enabled = false;
                    gostScript.enabled = true;
                }
            }
        }
    }

    IEnumerator FindPacmanAgain()
    {
        yield return new WaitForSeconds(3f);
        oneTimeEntrence = true;
    }

    bool FindingPacmanWithRayCast2D()
    {
        foreach (RaycastHit2D hit in hitRound)
            if (hit.transform.name.Equals("Pacman"))
                return true;
        return false;
    }

    void MakeRayCastHit2D()
    {
        Quaternion q = Quaternion.AngleAxis(2000 * Time.time, Vector3.forward);
        hitRound = Physics2D.RaycastAll(transform.position, q * Vector2.up, 10f, layermask);
        Debug.DrawRay(transform.position, q * Vector3.up * 10f);
    }
}