using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GostScript : MonoBehaviour
{
    public Transform wayPoint1;
    public Transform wayPoint2;
    public LayerMask layermask;
    PacManScript pacman;
    GameObject gate;
    Rigidbody2D rb;
    RaycastHit2D hitUp;
    RaycastHit2D hitDown;
    RaycastHit2D hitLeft;
    RaycastHit2D hitRight;
    RaycastHit2D rayRound;
    List<int> options;
    List<int> checkLoopDirections;
    Vector2 p1;
    Vector2 p2;
    bool gateOpen;
    bool startFindingPacman;
    bool oneTimeDirection;
    bool finishWaiting;
    bool getOutOfHome;
    bool[] freeDirection = { false, false, false, false };
    string[] directions = { "up", "right", "down", "left" };
    string lastdirection;
    float speed;
    int directionX;
    int directionY;
    int rand;
    int counter;

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(11, 11);
        pacman = GameObject.Find("Pacman").GetComponent<PacManScript>();
        rb = GetComponent<Rigidbody2D>();
        gate = GameObject.Find("Gate");

        options = new List<int>();
        checkLoopDirections = new List<int>();
        speed = 0;
        gateOpen = false;
        oneTimeDirection = true;
        startFindingPacman = false;
        finishWaiting = true;
        getOutOfHome = false;

        StartCoroutine(StartWait());
    }

    IEnumerator StartWait()
    {
        yield return new WaitForSeconds(5f);
        speed = 5f;
        Switches("up");
        StartCoroutine(WaitForGate());
    }

    void FixedUpdate()
    {
        if (pacman.isdead)
            rb.velocity = new Vector2(0, 0);
        else rb.velocity = new Vector2(speed * directionX, speed * directionY);

        if (!startFindingPacman && !transform.name.Equals("RedGost"))
        {
            if (gateOpen)
            {
                if (transform.name.Equals("LightBlueGost"))
                    Switches("right");
                else if (transform.name.Equals("YellowGost"))
                    Switches("left");
            }
        }

        if (getOutOfHome && gateOpen)
            GetOutOfHome();

        if (transform.position == wayPoint2.position && speed == 5f)
        {
            startFindingPacman = true;
            getOutOfHome = false;
            speed = 15f;
        }

        MakeRayCast();

        if (startFindingPacman)
        {
            if (finishWaiting)
            {
                finishWaiting = false;
                StartCoroutine(Wait());
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

                    if (checkLoopDirections.Count > 3)
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
            }
        }

        if (PlayerPrefs.GetInt("GostBlue", 0) == 1)
        {
            IsBlue();
        }
    }

    IEnumerator WaitForGate()
    {
        if (transform.name.Equals("PinkGost"))
            yield return new WaitForSeconds(5f);
        if (transform.name.Equals("LightBlueGost"))
            yield return new WaitForSeconds(10f);
        if (transform.name.Equals("YellowGost"))
            yield return new WaitForSeconds(15f);
        gateOpen = true;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (HitPacman())
        {
            if (PlayerPrefs.GetInt("GostBlue", 0) == 1)
            {
                GetComponent<Animator>().SetLayerWeight(2, 1);
            }

            else
            {
                pacman.isdead = true;
                pacman.GetComponent<Animator>().SetTrigger("die");
            }
        }

        if ((collision.gameObject.name.Equals("Wall") || collision.gameObject.name.Equals("Gate")) && !gateOpen)
        {
            if (GetComponent<Animator>().GetBool("up"))
                Switches("down");
            else
                Switches("up");
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("gostHome"))
        {
            speed = 5f;
            getOutOfHome = true;
        }
    }

    void FindHit()
    {
        if (hitUp.collider != null)
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

        if (hitRight.collider != null)
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

        if (hitDown.collider != null)
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

        if (hitLeft.collider != null)
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

    void MakeRayCast()
    {
        Quaternion q = Quaternion.AngleAxis(100 * Time.time, Vector3.forward);
        rayRound = Physics2D.Raycast(transform.position, q * Vector3.up * 5f, 1f, layermask);
        Debug.DrawRay(transform.position, q * Vector3.up * 5f);

        hitUp = Physics2D.CircleCast(transform.position, 1f, Vector2.up, 1f, layermask);
        Debug.DrawRay(transform.position, Vector2.up);

        hitDown = Physics2D.CircleCast(transform.position, 1f, Vector2.down, 1f, layermask);
        Debug.DrawRay(transform.position, Vector2.down);

        hitRight = Physics2D.CircleCast(transform.position, 1f, Vector2.right, 1f, layermask);
        Debug.DrawRay(transform.position, Vector2.right);

        hitLeft = Physics2D.CircleCast(transform.position, 1f, Vector2.left, 1f, layermask);
        Debug.DrawRay(transform.position, Vector2.left);
    }

    void IsBlue()
    {
        GetComponent<Animator>().SetBool("blue", true);
        StartCoroutine(Blue());
    }

    IEnumerator Blue()
    {
        yield return new WaitForSeconds(10f);
        GetComponent<Animator>().SetBool("blue", false);
        PlayerPrefs.SetInt("GostBlue", 0);
    }

    bool HitPacman()
    {
        if (hitUp.collider != null)
            return hitUp.collider.name.Equals("Pacman");
        else if (hitDown.collider != null)
            return hitDown.collider.name.Equals("Pacman");
        else if (hitLeft.collider != null)
            return hitLeft.collider.name.Equals("Pacman");
        else if (hitRight.collider != null)
            return hitRight.collider.name.Equals("Pacman");
        return false;
    }
    void GetOutOfHome()
    {
        p1 = Vector2.MoveTowards(transform.position, wayPoint1.position, speed * Time.deltaTime);
        GetComponent<Rigidbody2D>().MovePosition(p1);

        if (transform.position.x == p1.x)
        {
            Switches("up");
            p2 = Vector2.MoveTowards(transform.position, wayPoint2.position, speed * Time.deltaTime);
            GetComponent<Rigidbody2D>().MovePosition(p2);
        }
    }
}