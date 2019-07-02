using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GostScript : MonoBehaviour
{
    public Transform wayPoint1;
    public Transform wayPoint2;
    GameObject pacman;
    GameObject gate;
    bool gateOpen;
    float speed = 3f;


    private void Start()
    {
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
        if (!gateOpen && !transform.name.Equals("RedGost"))
        {
            if (GetComponent<Animator>().GetBool("up"))
                Move("up");
            if (GetComponent<Animator>().GetBool("down"))
                Move("down");

        }

        if (gateOpen && !transform.name.Equals("RedGost"))
        {
            gate.SetActive(false);
            if(transform.name.Equals("PinkGost"))
                Switches("right");
            else
                Switches("left");
            Vector2 p1 = Vector2.MoveTowards(transform.position, wayPoint1.position, speed*Time.deltaTime);
            GetComponent<Rigidbody2D>().MovePosition(p1);

            if(transform.position.x  == p1.x)
            {
                Switches("up");
                Vector2 p2 = Vector2.MoveTowards(transform.position, wayPoint2.position, speed * Time.deltaTime);
                GetComponent<Rigidbody2D>().MovePosition(p2);
            }
        }

        if(transform.position == wayPoint2.position)
        {
            Debug.Log("algorithm starts");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Board"))
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
