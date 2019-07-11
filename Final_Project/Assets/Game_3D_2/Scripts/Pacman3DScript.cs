using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacman3DScript : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip audioClip;
    public AudioClip wakkawakka;
    public AudioClip deadSound;
    Vector3 startPoint;
    public bool ghostBlue;
    float speed = 10f;
    public Rigidbody rb;
    public bool isdead;

    void Start()
    {
        startPoint = transform.position;
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
        rb.constraints = RigidbodyConstraints.FreezeAll;
        yield return new WaitForSeconds(audioClip.length);
        startPacman();
    }

    void startPacman()
    {
        ghostBlue = false;
        isdead = false;
        GetComponent<Animator>().SetBool("move", true);
        //directionX = 1;
        //directionY = 0;
        transform.eulerAngles = new Vector3(0, 0, 0);

        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        
    }
}
