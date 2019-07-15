using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshGhost3DScript : MonoBehaviour
{
    AudioSource source;
    public AudioClip ghostEat;
    public Transform target;
    //public Transform wayPoint;
    public Transform wayPoint2;
    private NavMeshAgent agent;
    public GameObject[] ghostPoints = new GameObject[4];
    Pacman3DScript pacman;
    Vector3 startPosition;
    private Material material;
    private Color original;
    bool oneTimeBlue;
    public float speed = 0f;
    RaycastHit forward;
    RaycastHit backwards;
    RaycastHit left;
    RaycastHit right;
    RaycastHit hitRound;
    public LayerMask layermask;
    bool[] freeDirection = { false, false, false, false };
    bool oneTimeDirection;
    bool finishWaiting;
    int counter = 0;
    List<int> options = new List<int>();
    List<int> checkLoopDirections = new List<int>();
    string lastdirection;
    int rand;
    Vector3[] directions = new Vector3[4];
    bool oneTimeEntrence;
    Vector3 moveDirection;
    bool gotowaypoint2;
    bool startFindingPacman;
    bool agentBool;
    Quaternion q;
    bool oneTimeEat;
    bool notBlueAnymore;
    bool resetAlgo;
    void Start()
    {
        source = GetComponent<AudioSource>();

        for (int i = 0; i < 4; i++)
            ghostPoints[i].SetActive(false);
        startPosition = transform.position;
        pacman = GameObject.Find("Pacman3D").GetComponent<Pacman3DScript>();
        agent = GetComponent<NavMeshAgent>();

        directions[0] = Vector3.forward;
        directions[1] = Vector3.right;
        directions[2] = Vector3.back;
        directions[3] = Vector3.left;

        material = GetComponent<Renderer>().material;
        InitGhost();
    }
    public void InitGhost()
    {
        StartCoroutine(WaitInit());
    }

    IEnumerator WaitInit()
    {
        agent.angularSpeed = 0f;
        resetAlgo = true;
        notBlueAnymore = true;
        transform.position = startPosition;
        oneTimeEat = true;
        agentBool = false;
        startFindingPacman = false;
        gotowaypoint2 = false;
        oneTimeEntrence = true;
        GetComponent<Renderer>().enabled = true;
        counter = 0;
        finishWaiting = true;
        oneTimeDirection = true;
        oneTimeBlue = true;
        transform.position = startPosition;
        original = material.color;
        agent.speed = 0;
        speed = 0;
        yield return new WaitForSeconds(5f);
        StartCoroutine(WaitToGetOut());
    }

    IEnumerator WaitToGetOut()
    {
        if (transform.name.Equals("GreenGhost"))
            yield return new WaitForSeconds(3f);
        if (transform.name.Equals("DarkGreenGhost"))
            yield return new WaitForSeconds(1f);
        if (transform.name.Equals("PinkGhost"))
            yield return new WaitForSeconds(8f);
        if (transform.name.Equals("LightBlueGhost"))
            yield return new WaitForSeconds(10f);
        if (transform.name.Equals("YellowGhost"))
            yield return new WaitForSeconds(12f);
        if (transform.name.Equals("PurpleGhost"))
            yield return new WaitForSeconds(15f);
        agent.speed = 5;
        speed = 8f;
    }



    void FixedUpdate()
    {
        MakeRayCast();

        if (startFindingPacman && speed != 0)
        {
            if (transform.name.Equals("DarkGreenGhost"))
                Debug.Log("aaaa");
                //GetComponent<CapsuleCollider>().isTrigger = false;

            if (FindingPacmanWithRayCast())
            {
                agentBool = true;
                StartCoroutine(FindPacmanAgain());
            }
            if (agentBool)
            {
                agent.enabled = true;
                resetAlgo = true;
                agent.SetDestination(target.position);
            }
            else
            {
                if (resetAlgo)
                    ResetAlgorithm();
                Algorithm();
            }
        }

        if (pacman.ghostBlue && oneTimeBlue)
            StartCoroutine(Blue());

        if (transform.position == wayPoint2.position)
        {
            if(transform.name.Equals("DarkGreenGhost"))
            Debug.Log("sss");
            startFindingPacman = true;
        }
            


        if (speed == 8f && !startFindingPacman)
            GetOutOfHome();

        if (moveDirection != null && !agentBool)
            transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    void ResetAlgorithm()
    {
        agent.enabled = false;
        resetAlgo = false;
        finishWaiting = true;
        if (checkLoopDirections.Count > 5)
            checkLoopDirections.Clear();
        options.Clear();
        counter = 0;
    }

    IEnumerator FindPacmanAgain()
    {
        yield return new WaitForSeconds(3f);
        agentBool = false;
    }

    void MakeRayCast()
    {
        q = Quaternion.AngleAxis(2000 * Time.time, Vector3.up);
        Debug.DrawRay(transform.position, q * Vector3.forward * 10f);
    }

    bool FindingPacmanWithRayCast()
    {
        if (Physics.Raycast(transform.position, q * Vector3.forward, out hitRound))
            if (hitRound.collider.name.Equals("Pacman3D"))
                return true;
        return false;
    }

    void GetOutOfHome()
    {
        agent.SetDestination(wayPoint2.position);
        /*//GetComponent<CapsuleCollider>().isTrigger = true;
        Vector3 p1 = Vector3.MoveTowards(transform.position, wayPoint.position, speed * Time.deltaTime);
        //GetComponent<Rigidbody>().MovePosition(p1);
        transform.Translate(p1);
        //Quaternion.Slerp(transform.position, p1, Time.deltaTime*speed);
        if (transform.position.z == p1.z)
        {
            Vector3 p2 = Vector3.MoveTowards(transform.position, wayPoint2.position, speed * Time.deltaTime);
            //GetComponent<Rigidbody>().MovePosition(p2);
            transform.Translate(p2);
        }*/
    }

    void Algorithm()
    {
        if (finishWaiting)
        {
            finishWaiting = false;

            FindRayCastHit();

            /*Debug.Log("forward: " + freeDirection[0]);
            Debug.Log("right: " + freeDirection[1]);
            Debug.Log("back: " + freeDirection[2]);
            Debug.Log("left: " + freeDirection[3]);*/

            for (int i = 0; i < 4; i++)
            {
                if (freeDirection[i] == true)
                {
                    counter++;
                    options.Add(i);
                }
            }

            if (oneTimeDirection && (counter != 0))
            {
                oneTimeDirection = false;

                if (checkLoopDirections.Count > 2)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        rand = Random.Range(0, counter);
                        if (checkLoopDirections[checkLoopDirections.Count - 2] != options[rand])
                            break;
                    }
                }
                else
                    rand = Random.Range(0, counter);
                checkLoopDirections.Add(options[rand]);
                moveDirection = directions[options[rand]];
            }
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        if (checkLoopDirections.Count > 5)
            checkLoopDirections.Clear();
        options.Clear();
        counter = 0;
        yield return new WaitForSeconds(0.5f);
        finishWaiting = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name.Equals("Pacman3D"))
        {
            if (!notBlueAnymore)
            {
                notBlueAnymore = true;
                if (oneTimeEat)
                    StartCoroutine(WaitForAnotherEat());
                transform.position = startPosition;
                startFindingPacman = false;
                material.color = original;
            }
            else
                pacman.isdead = true;
        }

    }

    IEnumerator WaitForAnotherEat()
    {
        oneTimeEntrence = true;
        pacman.ghostBlueCount++;
        oneTimeEat = false;
        source.PlayOneShot(ghostEat);
        if (pacman.ghostBlueCount == 1)
        {
            PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + 200);
            ghostPoints[0].transform.position = transform.position;
            ghostPoints[0].SetActive(true);
        }
        else if (pacman.ghostBlueCount == 2)
        {
            PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + 200);
            ghostPoints[0].transform.position = transform.position;
            ghostPoints[0].SetActive(true);
        }
        else if (pacman.ghostBlueCount == 3)
        {
            PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + 400);
            ghostPoints[1].transform.position = transform.position;
            ghostPoints[1].SetActive(true);
        }
        else if (pacman.ghostBlueCount == 4)
        {
            PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + 400);
            ghostPoints[1].transform.position = transform.position;
            ghostPoints[1].SetActive(true);
        }
        else if (pacman.ghostBlueCount == 5)
        {
            PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + 800);
            ghostPoints[2].transform.position = transform.position;
            ghostPoints[2].SetActive(true);
        }
        else if (pacman.ghostBlueCount == 6)
        {
            PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + 800);
            ghostPoints[2].transform.position = transform.position;
            ghostPoints[2].SetActive(true);
        }
        else if (pacman.ghostBlueCount == 7)
        {
            PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + 1600);
            ghostPoints[3].transform.position = transform.position;
            ghostPoints[3].SetActive(true);
        }
        else if (pacman.ghostBlueCount == 8)
        {
            PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + 1600);
            ghostPoints[3].transform.position = transform.position;
            ghostPoints[3].SetActive(true);
        }

        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 4; i++)
            ghostPoints[i].SetActive(false);
    }


    void FindRayCastHit()
    {
        if (Physics.SphereCast(transform.position, 1.5f, Vector3.forward, out forward, 3f, layermask))
        {
            if (freeDirection[0] == true)
                oneTimeDirection = true;
            freeDirection[0] = false;
        }
        else
        {
            if (freeDirection[0] == false)
                oneTimeDirection = true;
            freeDirection[0] = true;
        }
        if (Physics.SphereCast(transform.position, 1.5f, Vector3.right, out right, 3f, layermask))
        {
            if (freeDirection[1] == true)
                oneTimeDirection = true;
            freeDirection[1] = false;
        }
        else
        {
            if (freeDirection[1] == false)
                oneTimeDirection = true;
            freeDirection[1] = true;
        }
        if (Physics.SphereCast(transform.position, 1.5f, Vector3.back, out backwards, 3f, layermask))
        {
            if (freeDirection[2] == true)
                oneTimeDirection = true;
            freeDirection[2] = false;
        }
        else
        {
            if (freeDirection[2] == false)
                oneTimeDirection = true;
            freeDirection[2] = true;
        }

        if (Physics.SphereCast(transform.position, 1.5f, Vector3.left, out left, 3f, layermask))
        {
            if (freeDirection[3] == true)
                oneTimeDirection = true;
            freeDirection[3] = false;
        }
        else
        {
            if (freeDirection[3] == false)
                oneTimeDirection = true;
            freeDirection[3] = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.forward, 1.5f);
        Gizmos.DrawWireSphere(transform.position + Vector3.right, 1.5f);
        Gizmos.DrawWireSphere(transform.position + Vector3.back, 1.5f);
        Gizmos.DrawWireSphere(transform.position + Vector3.left, 1.5f);
    }

    IEnumerator Blue()
    {
        notBlueAnymore = false;
        pacman.ghostBlue = true;
        oneTimeBlue = false;
        material.color = Color.blue;
        yield return new WaitForSeconds(40f);
        pacman.ghostBlue = false;
        material.color = original;
        oneTimeBlue = true;
        notBlueAnymore = true;
    }
}
