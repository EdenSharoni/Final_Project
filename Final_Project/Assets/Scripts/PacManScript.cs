using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManScript : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public AudioClip wakkawakka;
    public AudioClip deadSound;
    public Rigidbody2D rb;
    public LayerMask layermask;
    public bool isdead;
    public bool ghostBlue;
    Vector2 startPoint;
    RaycastHit2D hitUp;
    RaycastHit2D hitDown;
    RaycastHit2D hitLeft;
    RaycastHit2D hitRight;
    float speed = 10f;
    int directionX;
    int directionY;
    bool anotherDot;
    public int ghostBlueCount;
    string saveLastPress;
    string currentDirection;

    private void Start()
    {
        anotherDot = true;
        startPoint = transform.position;

        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        initPacman();
    }

    public void initPacman()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        transform.position = startPoint;
        audioSource.PlayOneShot(audioClip);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(audioClip.length);
        startPacman();
    }

    void startPacman()
    {
        saveLastPress = "right";
        currentDirection = "right";
        ghostBlueCount = 0;
        ghostBlue = false;
        isdead = false;
        GetComponent<Animator>().SetBool("move", true);
        directionX = 1;
        directionY = 0;
        transform.eulerAngles = new Vector3(0, 0, 0);

        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

    }
    private void Update()
    {
        GetInput();
    }

    void FixedUpdate()
    {
        MakeRayCast();
        rb.velocity = new Vector2(speed * directionX, speed * directionY);
    }

    void GetInput()
    {
        if (!isdead)
        {
            //Adjust the direction
            if (Input.GetKey(KeyCode.RightArrow) && hitRight.collider == null) Right();
            else if (Input.GetKey(KeyCode.LeftArrow) && hitLeft.collider == null) Left();
            else if (Input.GetKey(KeyCode.DownArrow) && hitDown.collider == null) Down();
            else if (Input.GetKey(KeyCode.UpArrow) && hitUp.collider == null) Up();

            //Save The input that was not free
            if (Input.GetKey(KeyCode.UpArrow) && hitUp.collider != null) saveLastPress = "up";
            else if (Input.GetKey(KeyCode.RightArrow) && hitRight.collider != null) saveLastPress = "right";
            else if (Input.GetKey(KeyCode.DownArrow) && hitDown.collider != null) saveLastPress = "down";
            else if (Input.GetKey(KeyCode.LeftArrow) && hitLeft.collider != null) saveLastPress = "left";

            //Turn to the free direction
            if (hitRight.collider == null && saveLastPress == "right") Right();
            else if (hitLeft.collider == null && saveLastPress == "left") Left();
            else if (hitUp.collider == null && saveLastPress == "up") Up();
            else if (hitDown.collider == null && saveLastPress == "down") Down();

            //Pacman hit the wall and animation stops
            if (hitRight.collider != null && currentDirection == "right") GetComponent<Animator>().enabled = false;
            else if (hitLeft.collider != null && currentDirection == "left") GetComponent<Animator>().enabled = false;
            else if (hitUp.collider != null && currentDirection == "up") GetComponent<Animator>().enabled = false;
            else if (hitDown.collider != null && currentDirection == "down") GetComponent<Animator>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("powerupDot"))
        {
            ghostBlue = true;
            if (anotherDot)
                audioSource.PlayOneShot(wakkawakka);
            StartCoroutine(WaitForSoundToEnd());
            PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + 50);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("dots"))
        {
            if (anotherDot)
                StartCoroutine(WaitForSoundToEnd());
            PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + 10);
            Destroy(collision.gameObject);
        }
    }

    IEnumerator WaitForSoundToEnd()
    {
        anotherDot = false;
        audioSource.PlayOneShot(wakkawakka);
        yield return new WaitForSeconds(wakkawakka.length);
        anotherDot = true;
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

    


    void Right()
    {
        GetComponent<Animator>().enabled = true;
        directionX = 1;
        directionY = 0;
        transform.eulerAngles = new Vector3(0, 0, 0);
        saveLastPress = null;
        currentDirection = "right";
    }
    void Left()
    {
        GetComponent<Animator>().enabled = true;
        directionX = -1;
        directionY = 0;
        transform.eulerAngles = new Vector3(0, 0, 180);
        saveLastPress = null;
        currentDirection = "left";
    }
    void Down()
    {
        GetComponent<Animator>().enabled = true;
        directionX = 0;
        directionY = -1;
        transform.eulerAngles = new Vector3(0, 0, 270);
        saveLastPress = null;
        currentDirection = "down";
    }
    void Up()
    {
        GetComponent<Animator>().enabled = true;
        directionX = 0;
        directionY = 1;
        transform.eulerAngles = new Vector3(0, 0, 90);
        saveLastPress = null;
        currentDirection = "up";
    }
}