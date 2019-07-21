using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GostScript : MonoBehaviour
{
    public Transform wayPoint1;
    public Transform wayPoint2;
    public LayerMask layermask;
    public bool startFindingPacman;
    public bool controllerOneTimeEntrence;
    public bool oneTimeBlue;
    GameObject gate;
    PacManScript pacman;
    Rigidbody2D rb;
    RaycastHit2D hitUp;
    RaycastHit2D hitDown;
    RaycastHit2D hitLeft;
    RaycastHit2D hitRight;
    List<int> options = new List<int>();
    List<int> checkLoopDirections = new List<int>();
    float speed = 0;
    int rand;
    int counter = 0;
    bool upDownStarter;
    bool gateOpen;
    bool oneTimeDirection;
    bool finishWaiting;
    bool getOutOfHome;
    bool[] freeDirection = { false, false, false, false };

    Vector2 left = Vector2.left,
            right = Vector2.right,
            down = Vector2.down,
            up = Vector2.up,
            currentDirection = Vector2.zero;

    private void Start()
    {
        pacman = GameObject.Find("Pacman").GetComponent<PacManScript>();
        rb = GetComponent<Rigidbody2D>();
        gate = GameObject.Find("Gate");
        initGhost();
        getOutOfHome = false;
    }

    public void initGhost()
    {
        currentDirection = Vector2.zero;
        gate.SetActive(true);
        oneTimeBlue = true;
        upDownStarter = true;
        oneTimeDirection = true;
        finishWaiting = true;
        gateOpen = false;
        startFindingPacman = false;
        speed = 0;
        counter = 0;
        GetComponent<Animator>().SetLayerWeight(2, 0);
        GetComponent<Animator>().SetBool("blue", false);
        StartCoroutine(StartWait());
    }

    IEnumerator StartWait()
    {
        yield return new WaitForSeconds(5f);
        speed = 5f;
        Switches(0);
        StartCoroutine(WaitForGate());
    }

    IEnumerator WaitForGate()
    {
        if (transform.name.Equals("PinkGost"))
            yield return new WaitForSeconds(3f);
        if (transform.name.Equals("LightBlueGost"))
            yield return new WaitForSeconds(8f);
        if (transform.name.Equals("YellowGost"))
            yield return new WaitForSeconds(12f);
        upDownStarter = false;
        gateOpen = true;
    }

    void FixedUpdate()
    {
        MakeRayCast();

        if (pacman.ghostBlue && !gateOpen && oneTimeBlue)
            StartCoroutine(Blue());

        rb.velocity = currentDirection * speed;

        GetOutOfHome();

        if (transform.position == wayPoint2.position && speed == 5f)
        {
            gate.SetActive(true);
            gateOpen = false;
            startFindingPacman = true;
            getOutOfHome = false;
            speed = 10f;
        }

        if (startFindingPacman)
        {
            Algorithm();
        }
    }

    void GetOutOfHome()
    {
        if (getOutOfHome && gateOpen && !GetComponent<Animator>().GetBool("blue"))
        {
            Vector2 p1 = Vector2.MoveTowards(transform.position, wayPoint1.position, speed * Time.deltaTime);
            GetComponent<Rigidbody2D>().MovePosition(p1);
            if (transform.position.x == p1.x)
            {
                Switches(0);
                Vector2 p2 = Vector2.MoveTowards(transform.position, wayPoint2.position, speed * Time.deltaTime);
                GetComponent<Rigidbody2D>().MovePosition(p2);
            }
        }
    }

    void Algorithm()
    {
        if (finishWaiting)
        {
            finishWaiting = false;

            FindHit();

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
                Switches(options[rand]);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("gostHome"))
        {
            GetComponent<Animator>().SetLayerWeight(2, 0);
            GetComponent<Animator>().SetBool("blue", false);
            speed = 5f;
            getOutOfHome = true;
            if (startFindingPacman)
            {
                gateOpen = true;
                controllerOneTimeEntrence = true;
            }
            startFindingPacman = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (upDownStarter)
            if (GetComponent<Animator>().GetBool("up"))
                Switches(2);
            else
                Switches(0);
    }

    IEnumerator Blue()
    {
        oneTimeBlue = false;
        GetComponent<Animator>().SetBool("blue", true);
        yield return new WaitForSeconds(8.5f);
        GetComponent<Animator>().SetBool("blue", false);
        yield return new WaitForSeconds(1.5f);
        pacman.ghostBlue = false;
        oneTimeBlue = true;
        pacman.ghostBlueCount = 0;
        transform.gameObject.layer = 11;
    }

    void Switches(int num)
    {
        GetComponent<Animator>().SetBool("up", false);
        GetComponent<Animator>().SetBool("down", false);
        GetComponent<Animator>().SetBool("left", false);
        GetComponent<Animator>().SetBool("right", false);

        switch (num)
        {
            case 0:
                currentDirection = up;
                GetComponent<Animator>().SetBool("up", true);
                break;
            case 1:
                currentDirection = right;
                GetComponent<Animator>().SetBool("right", true);
                break;
            case 2:
                currentDirection = down;
                GetComponent<Animator>().SetBool("down", true);
                break;
            case 3:
                currentDirection = left;
                GetComponent<Animator>().SetBool("left", true);
                break;
        }
    }

    void FindHit()
    {
        if (hitUp.collider != null)
            SetDirections(0, true, false);
        else SetDirections(0, false, true);
        if (hitRight.collider != null)
            SetDirections(1, true, false);
        else SetDirections(1, false, true);
        if (hitDown.collider != null)
            SetDirections(2, true, false);
        else SetDirections(2, false, true);
        if (hitLeft.collider != null)
            SetDirections(3, true, false);
        else SetDirections(3, false, true);
    }

    void SetDirections(int number, bool boolean1, bool boolean2)
    {
        if (freeDirection[number] == boolean1)
            oneTimeDirection = true;
        freeDirection[number] = boolean2;
    }

    void MakeRayCast()
    {
        hitUp = Physics2D.CircleCast(transform.position, 1f, Vector2.up, 1f, layermask);
        Debug.DrawRay(transform.position, Vector2.up);

        hitDown = Physics2D.CircleCast(transform.position, 1f, Vector2.down, 1f, layermask);
        Debug.DrawRay(transform.position, Vector2.down);

        hitRight = Physics2D.CircleCast(transform.position, 1f, Vector2.right, 1f, layermask);
        Debug.DrawRay(transform.position, Vector2.right);

        hitLeft = Physics2D.CircleCast(transform.position, 1f, Vector2.left, 1f, layermask);
        Debug.DrawRay(transform.position, Vector2.left);
    }
}