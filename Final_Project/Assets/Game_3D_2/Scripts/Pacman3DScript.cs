using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    CameraControllerScript cameraControll;

    public Vector3 up = Vector3.zero,
                    right = new Vector3(0, 90, 0),
                    down = new Vector3(0, 180, 0),
                    left = new Vector3(0, 270, 0),
                    currentDirection = Vector3.zero;

    void Start()
    {
        cameraControll = GameObject.Find("Camera Manager").GetComponent<CameraControllerScript>();
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
        currentDirection = up;
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
        if (SceneManager.GetActiveScene().name.Equals("Game_3D_2"))
        {
            if (cameraControll.backCameraBool)
            {
                if (isdead) isMoving = false;
                else if (Input.GetKey(KeyCode.RightArrow))
                    transform.Rotate(transform.rotation.x, transform.rotation.y + 3f, transform.rotation.z);
                else if (Input.GetKey(KeyCode.LeftArrow))
                    transform.Rotate(transform.rotation.x, transform.rotation.y - 3f, transform.rotation.z);
                else if (Input.GetKey(KeyCode.DownArrow))
                    transform.Translate(Vector3.back * speed * Time.deltaTime);
                else if (Input.GetKey(KeyCode.UpArrow))
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);
                else isMoving = false;
            }
            else
            {
                if (isdead) isMoving = false;
                else if (Input.GetKey(KeyCode.UpArrow)) currentDirection = up;
                else if (Input.GetKey(KeyCode.RightArrow)) currentDirection = right;
                else if (Input.GetKey(KeyCode.DownArrow)) currentDirection = down;
                else if (Input.GetKey(KeyCode.LeftArrow)) currentDirection = left;
                else isMoving = false;
                if (isMoving)
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);
                transform.localEulerAngles = currentDirection;
            }
        }
        else
        {
            if (isdead) isMoving = false;
            else if (Input.GetKey(KeyCode.UpArrow)) currentDirection = up;
            else if (Input.GetKey(KeyCode.RightArrow)) currentDirection = right;
            else if (Input.GetKey(KeyCode.DownArrow)) currentDirection = down;
            else if (Input.GetKey(KeyCode.LeftArrow)) currentDirection = left;
            else isMoving = false;
            if (isMoving)
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            transform.localEulerAngles = currentDirection;
        }

        GetComponent<Animator>().SetBool("move", isMoving);


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
