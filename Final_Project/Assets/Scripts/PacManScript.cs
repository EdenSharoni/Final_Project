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
    public int ghostBlueCount;
    RaycastHit2D hitUp, hitDown, hitLeft, hitRight;
    float speed = 10f;
    bool anotherDot;
    Vector2 left = Vector2.left,
        right = Vector2.right,
        down = Vector2.down,
        up = Vector2.up,
        currentDirection = Vector2.zero,
        saveLastPress = Vector2.zero,
        startPoint = Vector2.zero;

    Vector3 upEulerAngles = new Vector3(0, 0, 90),
                    rightEulerAngles = Vector3.zero,
                    downEulerAngles = new Vector3(0, 0, 270),
                    leftEulerAngles = new Vector3(0, 0, 180);

    private void Start()
    {
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
        anotherDot = true;
        ghostBlue = false;
        isdead = false;
        saveLastPress = currentDirection = right;
        ghostBlueCount = 0;
        GetComponent<Animator>().SetBool("move", true);
        transform.eulerAngles = rightEulerAngles;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Update()
    {
        GetInput();
    }

    void FixedUpdate()
    {
        MakeRayCast();
        rb.velocity = currentDirection * speed;
    }

    void GetInput()
    {
        if (!isdead)
        {
            //Adjust the direction OR Turn to the free direction
            if ((Input.GetKey(KeyCode.RightArrow) && hitRight.collider == null)
                || (hitRight.collider == null && saveLastPress == right))
                SetDirection(right, rightEulerAngles);
            else if ((Input.GetKey(KeyCode.LeftArrow) && hitLeft.collider == null)
                || (hitLeft.collider == null && saveLastPress == left))
                SetDirection(left, leftEulerAngles);
            else if ((Input.GetKey(KeyCode.DownArrow) && hitDown.collider == null)
                || (hitDown.collider == null && saveLastPress == down))
                SetDirection(down, downEulerAngles);
            else if ((Input.GetKey(KeyCode.UpArrow) && hitUp.collider == null)
                || (hitUp.collider == null && saveLastPress == up))
                SetDirection(up, upEulerAngles);

            //Save The input that was not free in saveLastPress
            if (Input.GetKey(KeyCode.UpArrow) && hitUp.collider != null) saveLastPress = up;
            else if (Input.GetKey(KeyCode.RightArrow) && hitRight.collider != null) saveLastPress = right;
            else if (Input.GetKey(KeyCode.DownArrow) && hitDown.collider != null) saveLastPress = down;
            else if (Input.GetKey(KeyCode.LeftArrow) && hitLeft.collider != null) saveLastPress = left;

            //Pacman hit the wall and animation stops
            if ((hitRight.collider != null && currentDirection == right)
                || (hitLeft.collider != null && currentDirection == left)
                || (hitUp.collider != null && currentDirection == up)
                || (hitDown.collider != null && currentDirection == down))
                GetComponent<Animator>().enabled = false;
            else GetComponent<Animator>().enabled = true;
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
        audioSource.volume = 0.1f;
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

    void SetDirection(Vector2 direction, Vector3 eulerAngles)
    {
        currentDirection = direction;
        transform.eulerAngles = eulerAngles;
        saveLastPress = Vector2.zero;
    }
}