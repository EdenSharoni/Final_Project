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
    public int ghostBlueCounter;
    Vector2 startPoint;
    RaycastHit2D hitUp;
    RaycastHit2D hitDown;
    RaycastHit2D hitLeft;
    RaycastHit2D hitRight;
    float speed = 10f;
    int directionX;
    int directionY;
    bool anotherDot;

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
        ghostBlueCounter = 0;
        ghostBlue = false;
        isdead = false;
        GetComponent<Animator>().SetBool("move", true);
        directionX = 1;
        directionY = 0;
        transform.eulerAngles = new Vector3(0, 0, 0);
        audioSource.volume = 0.3f;

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
        if (Input.GetKey(KeyCode.RightArrow) && hitRight.collider == null)
        {
            directionX = 1;
            directionY = 0;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (Input.GetKey(KeyCode.LeftArrow) && hitLeft.collider == null)
        {
            directionX = -1;
            directionY = 0;
            transform.eulerAngles = new Vector3(0, 0, 180);
        }

        if (Input.GetKey(KeyCode.DownArrow) && hitDown.collider == null)
        {
            directionX = 0;
            directionY = -1;
            transform.eulerAngles = new Vector3(0, 0, 270);
        }

        if (Input.GetKey(KeyCode.UpArrow) && hitUp.collider == null)
        {
            directionX = 0;
            directionY = 1;
            transform.eulerAngles = new Vector3(0, 0, 90);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("powerupDot"))
        {
            ghostBlue = true;
            if (anotherDot)
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
}