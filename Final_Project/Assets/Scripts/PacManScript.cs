using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManScript : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioClip wakkawakka;
    public LayerMask layermask;
    AudioSource audioSource;
    public bool afterInitAudio;
    public Vector2 startPoint;
    float speed = 10f;
    public bool isdead;
    Rigidbody2D rb;
    int directionX;
    int directionY;
    public bool ghostBlue;
    RaycastHit2D hitUp;
    RaycastHit2D hitDown;
    RaycastHit2D hitLeft;
    RaycastHit2D hitRight;

    private void Start()
    {
        ghostBlue = false;
        directionX = 1;
        directionY = 0;

        startPoint = transform.position;
        rb = GetComponent<Rigidbody2D>();
        isdead = false;
        afterInitAudio = false;
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioClip);
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(audioClip.length);
        afterInitAudio = true;
        audioSource.loop = true;
        audioSource.clip = wakkawakka;
        audioSource.Play();
        GetComponent<Animator>().SetBool("move", true);
    }
    private void Update()
    {
        if (afterInitAudio && !isdead)
            GetInput();

        if (isdead)
            rb.velocity = new Vector2(0, 0);

    }

    void FixedUpdate()
    {
        MakeRayCast();
        //FindHit();
        if (afterInitAudio && !isdead)
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
            PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + 50);
            Destroy(collision.gameObject);
        }
    }
    void MakeRayCast()
    {
        hitUp = Physics2D.Raycast(transform.position, Vector2.up, 2f, layermask);
        Debug.DrawRay(transform.position, Vector2.up);

        hitDown = Physics2D.Raycast(transform.position, Vector2.down, 2f, layermask);
        Debug.DrawRay(transform.position, Vector2.down);

        hitRight = Physics2D.Raycast(transform.position, Vector2.right, 2f, layermask);
        Debug.DrawRay(transform.position, Vector2.right);

        hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 2f, layermask);
        Debug.DrawRay(transform.position, Vector2.left);
    }

    /*void FindHit()
    {
        if (hitUp.collider != null)
            Debug.Log("WALL UP");

        if (hitRight.collider != null)
            Debug.Log("WALLRight");

        if (hitDown.collider != null)
            Debug.Log("WALL DOWN");

        if (hitLeft.collider != null)
            Debug.Log("WALL LEFT");
    }*/
}