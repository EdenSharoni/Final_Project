using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManScript : MonoBehaviour
{
    public AudioClip audioClip;

    public AudioClip wakkawakka;
    public AudioClip deadSound;
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
    bool oneTimeEntrence;
    GameScript game;
    private void Start()
    {
        game = GameObject.Find("Main Camera").GetComponent<GameScript>();
        oneTimeEntrence = true;
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
        audioSource.volume = 0.5f;
        audioSource.clip = wakkawakka;
        audioSource.Play();
        GetComponent<Animator>().SetBool("move", true);
    }
    private void Update()
    {
        if (afterInitAudio && !isdead)
        {
            GetInput();
        }


        if ((PlayerPrefs.GetInt("gameOver", 0) == 1 || isdead) && oneTimeEntrence)
        {
            oneTimeEntrence = false;
            audioSource.clip = deadSound;
            audioSource.PlayOneShot(deadSound);
            GetComponent<Animator>().SetTrigger("die");
            rb.velocity = new Vector2(0, 0);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            if (game.life.Length > 0)
                StartCoroutine(PlayAgain());
        }
    }

    IEnumerator PlayAgain()
    {
        game.life[game.currentLife].enabled = false;
        game.currentLife--;
        yield return new WaitForSeconds(2f);
        transform.position = startPoint;
    }

    void FixedUpdate()
    {
        MakeRayCast();
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
        if (collision.gameObject.CompareTag("dots"))
        {
            PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + 10);
            Destroy(collision.gameObject);
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