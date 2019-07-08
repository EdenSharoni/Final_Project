using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GostControllerScript : MonoBehaviour
{
    PacManScript pacman;
    public LayerMask layermask;
    RaycastHit2D[] hitRound;

    GostAI gostAI;
    GostScript gostScript;
    GhostGoHomeAIScript gostGoHomeAIScript;

    bool oneTimeEntrence;
    bool oneTimeEntrence2;
    Vector2 startPoint;
    GameScript game;
    void Start()
    {
        game = GameObject.Find("Main Camera").GetComponent<GameScript>();
        oneTimeEntrence2 = true;
        startPoint = transform.position;
        pacman = GameObject.Find("Pacman").GetComponent<PacManScript>();
        oneTimeEntrence = true;
        gostAI = GameObject.Find(transform.name).GetComponent<GostAI>();
        gostAI.enabled = false;
        gostScript = GameObject.Find(transform.name).GetComponent<GostScript>();
        gostScript.enabled = true;
        gostGoHomeAIScript = GameObject.Find(transform.name).GetComponent<GhostGoHomeAIScript>();
        gostGoHomeAIScript.enabled = false;
    }
     IEnumerator PlayAgain()
    {
        yield return new WaitForSeconds(2f);
        transform.position = startPoint;
    }
    void FixedUpdate()
    {
        MakeRayCastHit2D();

        if (PlayerPrefs.GetInt("gameOver", 0) == 1|| pacman.isdead && oneTimeEntrence2)
        {
            oneTimeEntrence2 = false;
            gostGoHomeAIScript.enabled = false;
            gostAI.enabled = false;
            gostScript.enabled = false;
            if (game.life.Length > 0)
                StartCoroutine(PlayAgain());
        }

        if (gostScript.GetComponent<Animator>().GetLayerWeight(2) == 1)
        {
            gostScript.gate.GetComponent<BoxCollider2D>().enabled = false;
            gostScript.gate.GetComponent<PlatformEffector2D>().enabled = false;
            oneTimeEntrence = false;
            gostGoHomeAIScript.enabled = true;
            gostAI.enabled = false;
            gostScript.enabled = false;
            gostScript.gate.SetActive(false);
        }
        if (gostScript.controllerOneTimeEntrence)
        {
            gostScript.controllerOneTimeEntrence = false;
            oneTimeEntrence = true;
        }
        else
        if (pacman.ghostBlue && !(gostScript.GetComponent<Animator>().GetLayerWeight(2) == 1))
        {
            gostAI.enabled = false;
            gostScript.enabled = true;
            gostGoHomeAIScript.enabled = false;
        }

        if (oneTimeEntrence && gostScript.startFindingPacman)
        {
            oneTimeEntrence = false;

            if (FindingPacmanWithRayCast2D())
            {
                gostAI.enabled = true;
                gostScript.enabled = false;
                gostGoHomeAIScript.enabled = false;
                StartCoroutine(FindPacmanAgain());
            }
            else
            {
                oneTimeEntrence = true;
                gostAI.enabled = false;
                gostScript.enabled = true;
                gostGoHomeAIScript.enabled = false;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name.Equals("Pacman"))
        {
            if (gostScript.GetComponent<Animator>().GetBool("blue"))
            {
                GetComponent<Animator>().SetLayerWeight(2, 1);
            }
            else
            {
                pacman.isdead = true;
            }
        }
    }
}