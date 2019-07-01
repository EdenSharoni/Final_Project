using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManScript : MonoBehaviour
{
    public float speed = 1f;
    AudioSource audioSource;
    public AudioClip audioClip;
    public AudioClip wakkawakka;
    bool afterAudio;

    private void Start()
    {
        afterAudio = false;
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioClip);
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(audioClip.length);
        afterAudio = true;
        GetComponent<Animator>().SetBool("right", true);
        audioSource.loop = true;
        audioSource.clip = wakkawakka;
        audioSource.Play();
    }
    void FixedUpdate()
    {
        if (afterAudio)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || GetComponent<Animator>().GetBool("right"))
            {
                GetComponent<Animator>().SetBool("up", false);
                GetComponent<Animator>().SetBool("down", false);
                GetComponent<Animator>().SetBool("left", false);
                GetComponent<Animator>().SetBool("right", true);
                transform.Translate(speed * Time.deltaTime, 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || GetComponent<Animator>().GetBool("left"))
            {
                GetComponent<Animator>().SetBool("up", false);
                GetComponent<Animator>().SetBool("down", false);
                GetComponent<Animator>().SetBool("right", false);
                GetComponent<Animator>().SetBool("left", true);
                transform.Translate(-1 * speed * Time.deltaTime, 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || GetComponent<Animator>().GetBool("down"))
            {
                GetComponent<Animator>().SetBool("up", false);
                GetComponent<Animator>().SetBool("right", false);
                GetComponent<Animator>().SetBool("left", false);
                GetComponent<Animator>().SetBool("down", true);
                transform.Translate(0, -1 * speed * Time.deltaTime, 0);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || GetComponent<Animator>().GetBool("up"))
            {
                GetComponent<Animator>().SetBool("right", false);
                GetComponent<Animator>().SetBool("left", false);
                GetComponent<Animator>().SetBool("down", false);
                GetComponent<Animator>().SetBool("up", true);
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
            transform.Translate(-60, 0, 0);
        if (collision.gameObject.CompareTag("teleport2"))
            transform.Translate(60, 0, 0);
    }
}
