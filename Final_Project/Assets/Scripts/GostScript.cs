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
    public GameObject gate;
    PacManScript pacman;
    Rigidbody2D rb;
    RaycastHit2D hitUp;
    RaycastHit2D hitDown;
    RaycastHit2D hitLeft;
    RaycastHit2D hitRight;
    List<int> options = new List<int>();
    List<int> checkLoopDirections = new List<int>();
    string[] directions = { "up", "right", "down", "left" };
    string lastdirection;
    float speed = 0;
    int directionX;
    int directionY;
    int rand;
    int counter = 0;
    bool upDownStarter;
    bool oneTimeBlue;
    bool gateOpen;
    bool oneTimeDirection;
    bool finishWaiting;
    bool getOutOfHome;
    bool[] freeDirection = { false, false, false, false };

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(11, 11);

        pacman = GameObject.Find("Pacman").GetComponent<PacManScript>();
        rb = GetComponent<Rigidbody2D>();
        gate = GameObject.Find("Gate");
        initGhost();
        getOutOfHome = false;
    }

    public void initGhost()
    {
        directionX = 0;
        directionY = 0;
        gate.SetActive(true);
        oneTimeBlue = true;
        upDownStarter = true;
        oneTimeDirection = true;
        finishWaiting = true;
        gateOpen = false;
        startFindingPacman = false;
        speed = 0;
        counter = 0;
        gate.GetComponent<BoxCollider2D>().enabled = true;
        gate.GetComponent<PlatformEffector2D>().enabled = true;
        GetComponent<Animator>().SetLayerWeight(2, 0);
        GetComponent<Animator>().SetBool("blue", false);
        StartCoroutine(StartWait());
    }

    IEnumerator StartWait()
    {
        yield return new WaitForSeconds(5f);
        speed = 5f;
        Switches("up");
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

        rb.velocity = new Vector2(speed * directionX, speed * directionY);

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
                Switches("up");
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
                lastdirection = directions[options[rand]];
                Switches(lastdirection);
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
            gate.GetComponent<BoxCollider2D>().enabled = true;
            gate.GetComponent<PlatformEffector2D>().enabled = true;

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
        {
            if (GetComponent<Animator>().GetBool("up"))
                Switches("down");
            else
                Switches("up");
        }
    }

    IEnumerator Blue()
    {
        oneTimeBlue = false;
        GetComponent<Animator>().SetBool("blue", true);
        yield return new WaitForSeconds(10f);
        GetComponent<Animator>().SetBool("blue", false);
        pacman.ghostBlue = false;
        oneTimeBlue = true;
    }

    void Switches(string s)
    {
        GetComponent<Animator>().SetBool("up", false);
        GetComponent<Animator>().SetBool("down", false);
        GetComponent<Animator>().SetBool("left", false);
        GetComponent<Animator>().SetBool("right", false);

        switch (s)
        {
            case "right":
                directionX = 1;
                directionY = 0;
                lastdirection = "right";
                GetComponent<Animator>().SetBool("right", true);
                break;
            case "left":
                directionX = -1;
                directionY = 0;
                lastdirection = "left";
                GetComponent<Animator>().SetBool("left", true);
                break;
            case "up":
                directionX = 0;
                directionY = 1;
                lastdirection = "up";
                GetComponent<Animator>().SetBool("up", true);
                break;
            case "down":
                directionX = 0;
                directionY = -1;
                lastdirection = "down";
                GetComponent<Animator>().SetBool("down", true);
                break;
        }
    }

    void FindHit()
    {
        if (hitUp.collider != null)
        {
            if (hitUp.collider.name.Equals("Wall"))
            {
                if (freeDirection[0] == true)
                    oneTimeDirection = true;
                freeDirection[0] = false;
            }
        }
        else
        {
            if (freeDirection[0] == false)
                oneTimeDirection = true;
            freeDirection[0] = true;
        }

        if (hitRight.collider != null)
        {
            if (hitRight.collider.name.Equals("Wall"))
            {
                if (freeDirection[1] == true)
                    oneTimeDirection = true;
                freeDirection[1] = false;
            }
        }
        else
        {
            if (freeDirection[1] == false)
                oneTimeDirection = true;
            freeDirection[1] = true;
        }

        if (hitDown.collider != null)
        {
            if (hitDown.collider.name.Equals("Wall") || hitDown.collider.name.Equals("Gate"))
            {
                if (freeDirection[2] == true)
                    oneTimeDirection = true;
                freeDirection[2] = false;
            }
        }
        else
        {
            if (freeDirection[2] == false)
                oneTimeDirection = true;
            freeDirection[2] = true;
        }

        if (hitLeft.collider != null)
        {
            if (hitLeft.collider.name.Equals("Wall"))
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