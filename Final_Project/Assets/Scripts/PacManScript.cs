using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManScript : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioClip wakkawakka;
    AudioSource audioSource;
    public bool afterInitAudio;
    float speed = 15f;
    bool isdead;
    Rigidbody2D rb;
    int directionX;
    int directionY;
    private void Start()
    {
        directionX = 1;
        directionY = 0;
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
        {
            //transform.Translate(speed * Time.deltaTime, 0, 0);


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
    void FixedUpdate()
    {
        if (afterInitAudio && !isdead)
        {
            rb.velocity = new Vector2(speed * directionX, speed * directionY);
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("dots"))
        {
            PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + 10);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("teleport1"))
            transform.Translate(-70, 0, 0);
        if (collision.gameObject.CompareTag("teleport2"))
            transform.Translate(70, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("gost"))
        {
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<Rigidbody2D>().isKinematic = true;
            isdead = true;
        }
    }
}
