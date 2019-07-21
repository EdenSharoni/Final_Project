using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GostControllerScript : MonoBehaviour
{
    public AudioClip ghostEat;
    public AudioClip ghostFindingHome;
    public LayerMask layermask;
    public GameObject[] ghostPoints = new GameObject[4];
    GostAI gostAI;
    GostScript gostScript;
    GhostGoHomeAIScript gostGoHomeAIScript;
    Vector2 startPoint;
    AudioSource source;
    PacManScript pacman;
    RaycastHit2D hitRound;
    bool oneTimeEntrence;
    bool oneTimeEat;

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
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        SetScript(false, true, false);
        oneTimeEntrence = true;
        oneTimeEat = true;
        gostScript.initGhost();
    }

    public void StopAllCoroutinesInAllScripts()
    {
        gostScript.StopAllCoroutines();
    }

    void FixedUpdate()
    {
        MakeRayCastHit2D();
        if (GetComponent<Animator>().GetLayerWeight(2) == 1 && oneTimeEntrence)
        {
            oneTimeEntrence = false;
            transform.gameObject.layer = 13;
            source.clip = ghostFindingHome;
            source.loop = true;
            source.Play();
            SetScript(false, false, true);
        }

        if (gostScript.controllerOneTimeEntrence)
        {
            gostScript.controllerOneTimeEntrence = false;
            oneTimeEntrence = true;
            oneTimeEat = true;
        }

        else if (pacman.ghostBlue && GetComponent<Animator>().GetLayerWeight(2) != 1)
        {
            source.Stop();
            transform.gameObject.layer = 11;
            SetScript(false, true, false);
        }

        if (oneTimeEntrence && gostScript.startFindingPacman)
        {
            oneTimeEntrence = false;

            if (hitRound.collider != null)
            {
                SetScript(true, false, false);
                StartCoroutine(FindPacmanAgain());
            }
            else
            {
                SetScript(false, true, false);
                oneTimeEntrence = true;
            }
        }
    }

    IEnumerator FindPacmanAgain()
    {
        yield return new WaitForSeconds(3f);
        oneTimeEntrence = true;
    }

    void MakeRayCastHit2D()
    {
        Quaternion q = Quaternion.AngleAxis(2000 * Time.time, Vector3.forward);
        hitRound = Physics2D.Raycast(transform.position, q * Vector2.up, 10f, layermask);
        Debug.DrawRay(transform.position, q * Vector3.up * 10f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name.Equals("Pacman"))
            if (GetComponent<Animator>().GetBool("blue") && oneTimeEat)
                StartCoroutine(WaitForAnotherEat());
            else
                pacman.isdead = true;
    }

    IEnumerator WaitForAnotherEat()
    {
        oneTimeEntrence = true;
        oneTimeEat = false;
        pacman.ghostBlueCount++;
        source.PlayOneShot(ghostEat);
        GetComponent<Animator>().SetLayerWeight(2, 1);

        if (pacman.ghostBlueCount == 1)
            Points(0, 200);
        else if (pacman.ghostBlueCount == 2)
            Points(1, 400);
        else if (pacman.ghostBlueCount == 3)
            Points(2, 800);
        else if (pacman.ghostBlueCount == 4)
            Points(2, 1600);
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 4; i++)
            ghostPoints[i].SetActive(false);
    }

    void Points(int ghostnumer, int points)
    {
        PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + points);
        ghostPoints[ghostnumer].transform.position = transform.position;
        ghostPoints[ghostnumer].SetActive(true);
    }

    void SetScript(bool bool1, bool bool2, bool bool3)
    {
        gostAI.enabled = bool1;
        gostScript.enabled = bool2;
        gostGoHomeAIScript.enabled = bool3;
    }
}