using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GostControllerScript : MonoBehaviour
{
    public LayerMask layermask;
    GostAI gostAI;
    GostScript gostScript;
    GhostGoHomeAIScript gostGoHomeAIScript;
    Vector2 startPoint;
    AudioSource source;
    PacManScript pacman;
    RaycastHit2D[] hitRound;
    bool oneTimeEntrence;
    bool oneTimeEat;

    void Start()
    {
        startPoint = transform.position;
        source = GetComponent<AudioSource>();

        pacman = GameObject.Find("Pacman").GetComponent<PacManScript>();
        gostAI = GameObject.Find(transform.name).GetComponent<GostAI>();
        gostScript = GameObject.Find(transform.name).GetComponent<GostScript>();
        gostGoHomeAIScript = GameObject.Find(transform.name).GetComponent<GhostGoHomeAIScript>();

        initGhost();
   
    }
    public void initGhost()
    {
        transform.position = startPoint;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        SetScript("gostScript");
        oneTimeEntrence = true;
        oneTimeEat = true;
        gostScript.initGhost();
    }
    public void StopAllCoroutinesInAllScripts()
    {
        StopAllCoroutines();
        gostScript.StopAllCoroutines();
    }

    void FixedUpdate()
    {
        
        MakeRayCastHit2D();

        if (gostScript.GetComponent<Animator>().GetLayerWeight(2) == 1)
        {
            gostScript.gate.GetComponent<BoxCollider2D>().enabled = false;
            gostScript.gate.GetComponent<PlatformEffector2D>().enabled = false;

            gostScript.gate.SetActive(false);
            oneTimeEntrence = false;
            
            SetScript("gostGoHomeAIScript");
        }

        if (gostScript.controllerOneTimeEntrence)
        {
            gostScript.controllerOneTimeEntrence = false;
            oneTimeEntrence = true;
            oneTimeEat = true;
        }

        else if (pacman.ghostBlue && !(gostScript.GetComponent<Animator>().GetLayerWeight(2) == 1))
            SetScript("gostScript");

        if (oneTimeEntrence && gostScript.startFindingPacman)
        {
            oneTimeEntrence = false;

            if (FindingPacmanWithRayCast2D())
            {
                SetScript("gostAI");
                StartCoroutine(FindPacmanAgain());
            }
            else
            {
                SetScript("gostScript");
                oneTimeEntrence = true;
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
                if (oneTimeEat)
                {
                    oneTimeEat = false;
                    source.Play();
                }
                
                PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + 200);
                GetComponent<Animator>().SetLayerWeight(2, 1);
            }
            else
            {
                pacman.isdead = true;
            }
        }

    }
    void SetScript(string s)
    {

        switch (s)
        {
            case "gostScript":
                gostAI.enabled = false;
                gostScript.enabled = true;
                gostGoHomeAIScript.enabled = false;
                break;
            case "gostAI":
                gostAI.enabled = true;
                gostScript.enabled = false;
                gostGoHomeAIScript.enabled = false;
                break;
            case "gostGoHomeAIScript":
                gostAI.enabled = false;
                gostScript.enabled = false;
                gostGoHomeAIScript.enabled = true;
                break;
        }
    }
}