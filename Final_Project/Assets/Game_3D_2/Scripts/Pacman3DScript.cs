using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacman3DScript : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public AudioClip wakkawakka;
    public AudioClip deadSound;
    Vector3 startPoint;
    public bool ghostBlue;
    public float speed = 0f;
    public Rigidbody rb;
    public bool isdead;
    bool anotherDot;
    public int ghostBlueCount;
    bool isMoving;
    public bool blueAgain;

    private Vector3 up = Vector3.zero,
                    right = new Vector3(0, 90, 0),
                    down = new Vector3(0, 180, 0),
                    left = new Vector3(0, 270, 0),
                    currentDirection = Vector3.zero;

    void Start()
    {
        startPoint = transform.position;
        rb = GetComponent<Rigidbody>();

        audioSource = GetComponent<AudioSource>();
        initPacman();
    }

    public void initPacman()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        blueAgain = false;
        transform.position = startPoint;
        currentDirection = down;
        isMoving = false;
        speed = 0;
        transform.position = startPoint;
        audioSource.PlayOneShot(audioClip);
        yield return new WaitForSeconds(audioClip.length);
        startPacman();
    }

    void startPacman()
    {
        ghostBlueCount = 0;
        speed = 10f;
        anotherDot = true;
        ghostBlue = false;
        isdead = false;
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        isMoving = true;

        if (isdead) isMoving = false;
        else if (Input.GetKey(KeyCode.UpArrow)) currentDirection = up;
        else if (Input.GetKey(KeyCode.RightArrow)) currentDirection = right;
        else if (Input.GetKey(KeyCode.DownArrow)) currentDirection = down;
        else if (Input.GetKey(KeyCode.LeftArrow)) currentDirection = left;
        else isMoving = false;

        transform.localEulerAngles = currentDirection;

        GetComponent<Animator>().SetBool("move", isMoving);

        if (isMoving)
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PowerFood3D"))
        {
            if (ghostBlue)
                blueAgain = true;
            else
                ghostBlue = true;

            if (anotherDot)
                audioSource.PlayOneShot(wakkawakka);
            StartCoroutine(WaitForSoundToEnd());
            PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + 50);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Food3D"))
        {
            if (anotherDot)
                StartCoroutine(WaitForSoundToEnd());
            PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + 10);
            Destroy(other.gameObject);
        }
    }

    IEnumerator WaitForSoundToEnd()
    {
        anotherDot = false;
        audioSource.PlayOneShot(wakkawakka);
        yield return new WaitForSeconds(wakkawakka.length);
        anotherDot = true;
    }
}
