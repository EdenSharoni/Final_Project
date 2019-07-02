using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GostScript : MonoBehaviour
{
    public Transform wayPoint1;
    public Transform wayPoint2;
    int counter = 0;
    GameObject pacman;
    GameObject gate;
    bool gateOpen;
    bool startFindingPacman;
    float speed = 3f;
    float deploymentHeight;
    Ray2D rayRound;
    Ray2D rayRight;
    Ray2D rayLeft;
    Ray2D rayUp;
    Ray2D rayDown;
    Vector3 directionRound;

    RaycastHit2D hitUp;
    RaycastHit2D hitDown;
    RaycastHit2D hitLeft;
    RaycastHit2D hitRight;
    string[] directions = { "up", "right", "down", "left"};

    bool[] freeDirection = { false, false, false, false };
    string lastdirection;
    bool oneTimeDirection;
    int rand;
    List<int> options;

    private void Start()
    {
        oneTimeDirection = true;
        startFindingPacman = false;
        directionRound = Vector3.up;
        deploymentHeight = 5f;
        gate = GameObject.Find("Gate");
        Switches("up");
        gateOpen = false;
        pacman = GameObject.Find("Pacman");
        StartCoroutine(WaitForGate());
    }

    IEnumerator WaitForGate()
    {
        yield return new WaitForSeconds(5f);
        gateOpen = true;
    }

    void FixedUpdate()
    {
        if (!startFindingPacman && !transform.name.Equals("RedGost"))
        {
            if (!gateOpen)
            {
                if (GetComponent<Animator>().GetBool("up"))
                    Move("up");
                if (GetComponent<Animator>().GetBool("down"))
                    Move("down");
            }

            if (gateOpen)
            {
                gate.SetActive(false);
                if (transform.name.Equals("PinkGost"))
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
            lastdirection = "up";
            gate.SetActive(true);
            if (hitUp)
            {
                if (hitUp.collider.name.Equals("Wall"))
                {
                    freeDirection[0] = false;
                }

                else
                {
                    if (freeDirection[0] == false)
                        oneTimeDirection = true;
                    if (!lastdirection.Equals("down"))
                            freeDirection[0] = true;
                }
            }
            if (hitRight)
            {
                if (hitRight.transform.name.Equals("Wall"))
                {
                    freeDirection[1] = false;
                }
                else
                {
                    if (freeDirection[1] == false)
                        oneTimeDirection = true;
                    if (!lastdirection.Equals("left"))
                        freeDirection[1] = true;
                }
            }
            if (hitDown)
            {
                if (hitDown.transform.name.Equals("Wall") || hitDown.transform.name.Equals("Gate"))
                {
                    freeDirection[2] = false;
                }
                else
                {
                    if (freeDirection[2] == false)
                        oneTimeDirection = true;
                    if (!lastdirection.Equals("up"))
                        freeDirection[2] = true;
                }
            }
            if (hitLeft)
            {
                if (hitLeft.transform.name.Equals("Wall"))
                {
                    freeDirection[3] = false;
                }
                else
                {
                    if (freeDirection[3] == false)
                        oneTimeDirection = true;
                    if (!lastdirection.Equals("right"))
                        freeDirection[3] = true;
                }
            }
            Debug.Log(freeDirection[0]);
            Debug.Log(freeDirection[1]);
            Debug.Log(freeDirection[2]);
            Debug.Log(freeDirection[3]);

            counter = 0;

            options = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                if (freeDirection[i] == true)
                {
                    counter++;
                    options.Add(i);
                }
            }

            Debug.Log("o[0]: " + options[0]);
            Debug.Log("o[1]: " + options[1]);

            if (counter > 1)
            {
                if (oneTimeDirection)
                {
                    oneTimeDirection = false;
                    rand = Random.Range(0, counter);
                    Debug.Log("rand: " + rand);
                }
                lastdirection = directions[options[rand]];
                Debug.Log("lastdirection: "+lastdirection);
            }
            Move(lastdirection);
            options.Clear();




            /*if (hitDown.transform.name.Equals("Board"))
            Debug.Log("up: " + hitUp.transform.name);
              Debug.Log("down: " + hitDown.transform.name);
              Debug.Log("left: " + hitLeft.transform.name);
              Debug.Log("right: " + hitRight.collider.name);*/
        }
    }


    void MakeRayCast()
    {
        /*Quaternion q = Quaternion.AngleAxis(100 * Time.time, Vector3.forward);
        rayRound = new Ray2D(transform.position, directionRound);
        Debug.DrawRay(transform.position, q * directionRound * deploymentHeight);*/

        hitUp = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1.1f), Vector2.up, 1f);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 1.1f), Vector2.up);

        hitDown = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 1.2f), Vector2.down, 1f);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - 1.2f), Vector2.down);

        hitRight = Physics2D.Raycast(new Vector2(transform.position.x + 1.2f, transform.position.y), Vector2.right, 1f);
        Debug.DrawRay(new Vector2(transform.position.x + 1.2f, transform.position.y), Vector2.right);

        hitLeft = Physics2D.Raycast(new Vector2(transform.position.x - 1.2f, transform.position.y), Vector2.left, 1f);
        Debug.DrawRay(new Vector2(transform.position.x - 1.2f, transform.position.y), Vector2.left);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Wall"))
        {
            if (!gateOpen)
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

    void Move(string s)
    {
        switch (s)
        {
            case "up":
                transform.Translate(0, speed * Time.deltaTime, 0);
                break;
            case "down":
                transform.Translate(0, -speed * Time.deltaTime, 0);
                break;
            case "left":
                transform.Translate(-speed * Time.deltaTime, 0, 0);
                break;
            case "right":
                transform.Translate(speed * Time.deltaTime, 0, 0);
                break;
        }
    }
}
