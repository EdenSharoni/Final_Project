using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GostScript : MonoBehaviour
{
    public Transform wayPoint1;
    public Transform wayPoint2;
    int counter = 0;
    GameObject gate;
    bool gateOpen;
    bool startFindingPacman;
    bool waitWithSong;
    float speed = 8f;
    float deploymentHeight;
    Vector3 directionRound;

    RaycastHit2D hitUp;
    RaycastHit2D hitDown;
    RaycastHit2D hitLeft;
    RaycastHit2D hitRight;
    Ray2D rayRound;
    string[] directions = { "up", "right", "down", "left" };

    bool[] freeDirection = { false, false, false, false };
    string lastdirection;
    bool oneTimeDirection;
    int rand;
    List<int> options;

    public PacManScript pacman;
    AudioClip pacmanAudioClip;
    Rigidbody2D rb;
    int directionX;
    int directionY;
    bool finishWaiting;
    private void Start()
    {
        directionX = 0;
        directionY = 1;
        finishWaiting = true;
        rb = GetComponent<Rigidbody2D>();
        counter = 0;
        options = new List<int>();
        lastdirection = "up";
        //pacmanAudioClip = pacman.audioClip;
        //Debug.Log(pacmanAudioClip.name);
        //waitWithSong = GetComponent<PacManScript>().afterInitAudio;
        //Debug.Log(waitWithSong);
        oneTimeDirection = true;
        startFindingPacman = false;
        directionRound = Vector3.up;
        deploymentHeight = 5f;
        gate = GameObject.Find("Gate");
        Switches("up");
        gateOpen = false;
        StartCoroutine(WaitForGate());
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

    void FixedUpdate()
    {
        rb.velocity = new Vector2(speed * directionX, speed * directionY);

        if (directionX == 1)
            Switches("right");
        else if (directionX == -1)
            Switches("left");

        if (directionY == 1)
            Switches("up");
        else if (directionY == -1)
            Switches("down");

        if (!startFindingPacman && !transform.name.Equals("RedGost"))
        {
            if (gateOpen)
            {
                gate.SetActive(false);
                if (transform.name.Equals("LightBlueGost"))
                    Switches("right");
                else
                    Switches("left");
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


        if (transform.position == wayPoint2.position)
            startFindingPacman = true;


        MakeRayCast();

        if (startFindingPacman)
        {
            if (finishWaiting)
            {
                finishWaiting = false;
                StartCoroutine(Wait());
                if (hitUp)
                {
                    if (hitUp.collider.name.Equals("Wall"))
                    {
                        if (freeDirection[0] == true)
                            oneTimeDirection = true;
                        freeDirection[0] = false;
                    }

                    else
                    {
                        if (freeDirection[0] == false)
                            oneTimeDirection = true;
                        /*if (lastdirection.Equals("down"))
                            freeDirection[0] = false;
                        else*/
                        freeDirection[0] = true;
                    }
                }
                if (hitRight)
                {
                    if (hitRight.transform.name.Equals("Wall"))
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
                }
                if (hitDown)
                {
                    if (hitDown.transform.name.Equals("Wall") || hitDown.transform.name.Equals("Gate"))
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
                }
                if (hitLeft)
                {
                    if (hitLeft.transform.name.Equals("Wall"))
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

                for (int i = 0; i < 4; i++)
                {
                    if (freeDirection[i] == true)
                    {
                        counter++;
                        options.Add(i);
                    }
                }

                if (oneTimeDirection)
                {
                    oneTimeDirection = false;
                    rand = Random.Range(0, counter);
                }

                lastdirection = directions[options[rand]];

                if (lastdirection.Equals("up"))
                {
                    directionX = 0;
                    directionY = 1;
                }
                if (lastdirection.Equals("right"))
                {
                    directionX = 1;
                    directionY = 0;
                }
                if (lastdirection.Equals("down"))
                {
                    directionX = 0;
                    directionY = -1;
                }
                if (lastdirection.Equals("left"))
                {
                    directionX = -1;
                    directionY = 0;
                }


                options.Clear();
                counter = 0;
            }

            /*Debug.Log("up: " + hitUp.transform.name);
              Debug.Log("down: " + hitDown.transform.name);
              Debug.Log("left: " + hitLeft.transform.name);
              Debug.Log("right: " + hitRight.collider.name);*/
        }
    }
    IEnumerator Wait()
    {

        yield return new WaitForSeconds(1f);
        finishWaiting = true;

    }

    void MakeRayCast()
    {
        Quaternion q = Quaternion.AngleAxis(100 * Time.time, Vector3.forward);
        rayRound = new Ray2D(transform.position, q * directionRound * deploymentHeight);
        Debug.DrawRay(transform.position, q * directionRound * deploymentHeight);

        hitUp = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1.5f), Vector2.up, 0.2f);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 1.1f), Vector2.up);

        hitDown = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 1.5f), Vector2.down, 0.2f);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - 1.2f), Vector2.down);

        hitRight = Physics2D.Raycast(new Vector2(transform.position.x + 1.5f, transform.position.y), Vector2.right, 0.2f);
        Debug.DrawRay(new Vector2(transform.position.x + 1.2f, transform.position.y), Vector2.right);

        hitLeft = Physics2D.Raycast(new Vector2(transform.position.x - 1.5f, transform.position.y), Vector2.left, 0.2f);
        Debug.DrawRay(new Vector2(transform.position.x - 1.2f, transform.position.y), Vector2.left);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Wall") || collision.gameObject.name.Equals("Gate"))
        {
            if (!gateOpen)
                if (GetComponent<Animator>().GetBool("up"))
                {
                    //Switches("down");
                    directionX = 0;
                    directionY = -1;
                }
                else
                {
                    //Switches("up");
                    directionX = 0;
                    directionY = 1;
                }
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
                GetComponent<Animator>().SetBool("right", true);
                break;
            case "left":
                GetComponent<Animator>().SetBool("left", true);
                break;
            case "up":
                GetComponent<Animator>().SetBool("up", true);
                break;
            case "down":
                GetComponent<Animator>().SetBool("down", true);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("teleport1"))
            transform.Translate(-70, 0, 0);
        if (collision.gameObject.CompareTag("teleport2"))
            transform.Translate(70, 0, 0);
    }
}
