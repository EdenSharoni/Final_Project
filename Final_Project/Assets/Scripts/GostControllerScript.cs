using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GostControllerScript : MonoBehaviour
{
    public AudioClip ghostEat;
    public AudioClip ghostFindingHome;
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
    public GameObject[] ghostPoints = new GameObject[4];

    void Start()
    {
        for (int i = 0; i < 4; i++)
            ghostPoints[i].SetActive(false);

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
            if (oneTimeEntrence)
            {
                oneTimeEntrence = false;
                source.clip = ghostFindingHome;
                source.loop = true;
                source.Play();
            }
            SetScript("gostGoHomeAIScript");
        }

        if (gostScript.controllerOneTimeEntrence)
        {
            gostScript.controllerOneTimeEntrence = false;
            oneTimeEntrence = true;
            oneTimeEat = true;
        }

        else if (pacman.ghostBlue && !(gostScript.GetComponent<Animator>().GetLayerWeight(2) == 1))
        {
            source.Stop();
            SetScript("gostScript");
        }


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
                    pacman.ghostBlueCount++;
                    oneTimeEat = false;
                    source.PlayOneShot(ghostEat);

                    GetComponent<Animator>().SetLayerWeight(2, 1);
                    StartCoroutine(Wait());
                }
            }
            else
            {
                pacman.isdead = true;
            }
        }

    }
    IEnumerator Wait()
    {
        if (pacman.ghostBlueCount == 1)
        {
            PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + 200);
            ghostPoints[0].transform.position = transform.position;
            ghostPoints[0].SetActive(true);
        }
        else if (pacman.ghostBlueCount == 2)
        {
            PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + 400);
            ghostPoints[1].transform.position = transform.position;
            ghostPoints[1].SetActive(true);
        }
        else if (pacman.ghostBlueCount == 3)
        {
            PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + 800);
            ghostPoints[2].transform.position = transform.position;
            ghostPoints[2].SetActive(true);
        }
        else if (pacman.ghostBlueCount == 4)
        {
            PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + 1600);
            ghostPoints[3].transform.position = transform.position;
            ghostPoints[3].SetActive(true);
        }

        yield return new WaitForSeconds(1f);
        for(int i = 0; i<4; i++)
            ghostPoints[i].SetActive(false);
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