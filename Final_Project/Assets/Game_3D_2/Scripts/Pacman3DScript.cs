using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pacman3DScript : MonoBehaviour
{
    public AudioSource audioSource;
    public Rigidbody bullet;
    public AudioClip audioClip;
    public AudioClip wakkawakka;
    public AudioClip deadSound;
    public Rigidbody rb;
    public bool ghostBlue;
    public bool blueAgain;
    public bool isdead;
    public float speed = 0f;
    public int ghostBlueCount;
    public Vector3 upDirection = Vector3.zero,
                rightDirection = Vector3.right,
                downDirection = Vector3.down,
                leftDirection = Vector3.left,
                currentDirection = Vector3.zero;

    Vector3 go = Vector3.zero;
    Vector3 up = new Vector3(0, 0, 1);
    Vector3 right = new Vector3(1, 0, 0);
    Vector3 left = new Vector3(-1, 0, 0);
    Vector3 down = new Vector3(0, 0, -1);

    private CameraControllerScript cameraControll;
    private Vector3 startPoint;
    private Scene activeScene;
    private bool anotherDot;
    private bool isMoving;
    private string sceneName;
    private int tempFoodCount = 0;
    private int tempBulletCount = 0;


    void Start()
    {
        cameraControll = GameObject.Find("Camera Manager").GetComponent<CameraControllerScript>();
        startPoint = transform.position;
        rb = GetComponent<Rigidbody>();

        audioSource = GetComponent<AudioSource>();
        StartCoroutine(StartCam());

        activeScene = SceneManager.GetActiveScene();
        sceneName = activeScene.name;
        PlayerPrefs.SetInt("fireCount", 0);
    }

    IEnumerator StartCam()
    {
        yield return new WaitForSeconds(9f);
        initPacman();
    }

    public void initPacman()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        GetComponent<Animator>().applyRootMotion = true;
        blueAgain = false;
        transform.position = startPoint;
        currentDirection = Vector3.zero;
        go = Vector3.zero;
        isMoving = false;
        speed = 0;
        transform.position = startPoint;
        audioSource.PlayOneShot(audioClip);
        yield return new WaitForSeconds(audioClip.length);
        startPacman();
    }

    void startPacman()
    {
        isMoving = true;
        ghostBlueCount = 0;
        speed = 15f;
        anotherDot = true;
        ghostBlue = false;
        isdead = false;
        currentDirection = up;
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private void Update()
    {
        GetInput();
    }

    void GetInput()
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
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                isMoving = true;
                go = up;
                currentDirection = upDirection;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                isMoving = true;
                go = right;
                currentDirection = rightDirection;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                isMoving = true;
                go = down;
                currentDirection = downDirection;
            }

            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                isMoving = true;
                go = left;
                currentDirection = leftDirection;
            }
            transform.localEulerAngles = currentDirection;
        }

        GetComponent<Animator>().SetBool("move", isMoving);

        if (Input.GetKeyDown("space") && sceneName == "Game_3D_2" && tempBulletCount > 0 && speed != 0)
        {
            Instantiate(bullet, transform.position, transform.rotation);
            PlayerPrefs.SetInt("fireCount", --tempBulletCount);
        }
    }

    private void FixedUpdate()
    {
        if (!cameraControll.backCameraBool)
            rb.velocity = go * speed;
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
            getBullets(50);
            StartCoroutine(WaitForSoundToEnd());
            PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + 50);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Food3D"))
        {
            if (anotherDot)
                StartCoroutine(WaitForSoundToEnd());
            getBullets(10);
            PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + 10);
            Destroy(other.gameObject);
        }
    }

    void getBullets(int amount)
    {
        tempFoodCount += amount;
        if (tempFoodCount >= 100)
        {
            tempBulletCount++;
            PlayerPrefs.SetInt("fireCount", tempBulletCount);
            tempFoodCount = 0;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Wall3D"))
        {
            isMoving = false;
        }
    }
}
