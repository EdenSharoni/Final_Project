using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManScript : MonoBehaviour
{
    public float speed = 1f;
    AudioSource audioSource;
    public AudioClip audioClip;
    public AudioClip wakkawakka;
    bool afterInitAudio;
    bool[] switches = { false , false, false, false};

    private void Start()
    {
        afterInitAudio = false;
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioClip);
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(audioClip.length);
        afterInitAudio = true;
        GetComponent<Animator>().SetBool("right", true);
        audioSource.loop = true;
        audioSource.clip = wakkawakka;
        audioSource.Play();
    }
    void FixedUpdate()
    {
        if (afterInitAudio)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || GetComponent<Animator>().GetBool("right"))
            {
                Switches("right");
                transform.Translate(speed * Time.deltaTime, 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || GetComponent<Animator>().GetBool("left"))
            {
                Switches("left");
                transform.Translate(-1 * speed * Time.deltaTime, 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || GetComponent<Animator>().GetBool("down"))
            {
                Switches("down");
                transform.Translate(0, -1 * speed * Time.deltaTime, 0);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || GetComponent<Animator>().GetBool("up"))
            {
                Switches("up");
                transform.Translate(0, speed * Time.deltaTime, 0);
            }
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("dots"))
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("teleport1"))
            transform.Translate(-70, 0, 0);
        if (collision.gameObject.CompareTag("teleport2"))
            transform.Translate(70, 0, 0);
    }

    void Switches(string s)
    {
        switch (s)
        {
            case "right":
                GetComponent<Animator>().SetBool("right", true);
                GetComponent<Animator>().SetBool("left", false);
                GetComponent<Animator>().SetBool("down", false);
                GetComponent<Animator>().SetBool("up", false);    
                break;

            case "left":
                GetComponent<Animator>().SetBool("right", false);
                GetComponent<Animator>().SetBool("left", true);
                GetComponent<Animator>().SetBool("down", false);
                GetComponent<Animator>().SetBool("up", false);
                break;

            case "down":
                GetComponent<Animator>().SetBool("right", false);
                GetComponent<Animator>().SetBool("left", false);
                GetComponent<Animator>().SetBool("down", true);
                GetComponent<Animator>().SetBool("up", false);
                break;

            case "up":
                GetComponent<Animator>().SetBool("right", false);
                GetComponent<Animator>().SetBool("left", false);
                GetComponent<Animator>().SetBool("down", false);
                GetComponent<Animator>().SetBool("up", true);
                break;
        }
    }
}
