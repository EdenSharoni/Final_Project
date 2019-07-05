using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManScript : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioClip wakkawakka;
    AudioSource audioSource;
    public bool afterInitAudio;
    public Vector2 startPoint;
    float speed = 10f;
    public bool isdead;
    Rigidbody2D rb;
    int directionX;
    int directionY;

    private void Start()
    {
        PlayerPrefs.SetInt("GostBlue", 0);
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
        if (afterInitAudio && !isdead)
            rb.velocity = new Vector2(speed * directionX, speed * directionY);
    }

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            directionX = 1;
            directionY = 0;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            directionX = -1;
            directionY = 0;
            transform.eulerAngles = new Vector3(0, 0, 180);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            directionX = 0;
            directionY = -1;
            transform.eulerAngles = new Vector3(0, 0, 270);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            directionX = 0;
            directionY = 1;
            transform.eulerAngles = new Vector3(0, 0, 90);
        }
    }
}