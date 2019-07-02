using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManScript : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioClip wakkawakka;
    AudioSource audioSource;
    bool afterInitAudio;
    float speed = 15f;

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
        audioSource.loop = true;
        audioSource.clip = wakkawakka;
        audioSource.Play();
        GetComponent<Animator>().SetBool("move", true);
    }
    void FixedUpdate()
    {
        if (afterInitAudio && !GetComponent<Animator>().GetBool("die"))
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);

            if (Input.GetKeyDown(KeyCode.RightArrow))
                transform.eulerAngles = new Vector3(0, 0, 0);
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                transform.eulerAngles = new Vector3(0, 0, 180);
            if (Input.GetKeyDown(KeyCode.DownArrow))
                transform.eulerAngles = new Vector3(0, 0, 270);
            if (Input.GetKeyDown(KeyCode.UpArrow))
                transform.eulerAngles = new Vector3(0, 0, 90);
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
            GetComponent<Animator>().SetBool("die", true);
    }

}
