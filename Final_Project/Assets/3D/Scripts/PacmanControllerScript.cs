using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PacmanControllerScript : MonoBehaviour
{
    public float movementSpeed = 0f;

    public Text scoreText;

    public Text lifesText;

    public bool blue3DGhost;

    private Animator animator = null;

    private Vector3 up = Vector3.zero,
                    right = new Vector3(0, 90, 0),
                    down = new Vector3(0, 180, 0),
                    left = new Vector3(0, 270, 0),
                    currentDirection = Vector3.zero;

    private Vector3 initialPosition = Vector3.zero;
    private NavMeshScript agent;

    private int countPoints;
    private int lifesCount;
    private bool isDied;
    private int ghostsCount = 4;

    public void Reset()
    {
        transform.position = initialPosition;
        animator.SetBool("isDead", false);
        animator.SetBool("isMoving", false);
        currentDirection = down;
        if (isDied)
        {
            lifesCount--;
            setLifes();
            isDied = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0;

        initialPosition = transform.position;

        animator = GetComponent<Animator>();

        //Physics.IgnoreLayerCollision(14, 14);

        countPoints = 0;
        setScore();

        lifesCount = 3;
        setLifes();

        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        bool isMoving = true;
        bool isDead = animator.GetBool("isDead");

        if (isDead) isMoving = false;
        else if (Input.GetKey(KeyCode.UpArrow)) currentDirection = up;
        else if (Input.GetKey(KeyCode.RightArrow)) currentDirection = right;
        else if (Input.GetKey(KeyCode.DownArrow)) currentDirection = down;
        else if (Input.GetKey(KeyCode.LeftArrow)) currentDirection = left;
        else isMoving = false;

        transform.localEulerAngles = currentDirection;

        animator.SetBool("isMoving", isMoving);

        if (isMoving)
            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (lifesCount <= 0 || ghostsCount <= 0)
            endGame();

        if (blue3DGhost)
        {
            if (collision.collider.CompareTag("Ghost3D"))
            {
                agent = collision.collider.gameObject.GetComponent<NavMeshScript>();
                agent.isGhostDead = true;
                StartCoroutine(WaitForGhostToReturn(collision.collider.gameObject));
                ghostsCount--;
            }

        }
        else if (collision.collider.CompareTag("Ghost3D"))
        {
            animator.SetBool("isDead", true);
            isDied = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food3D"))
        {
            countPoints += 10;
            setScore();
            Destroy(other.transform.gameObject);
        }
        else if (other.CompareTag("PowerFood3D"))
        {            
            StartCoroutine(WaitForFixedSeconds());
            countPoints += 50;
            setScore();
            Destroy(other.transform.gameObject);
        }
    }

    void setScore()
    {
        scoreText.text = "Score: " + countPoints.ToString();
    }

    void setLifes()
    {
        lifesText.text = "Lifes: " + lifesCount.ToString();
    }

    void endGame()
    {
        
    }

    IEnumerator WaitForFixedSeconds()
    {
        // change color to blue
        blue3DGhost = true;
        yield return new WaitForSeconds(10f);
        blue3DGhost = false;
        // change color to original
    }

    IEnumerator WaitForGhostToReturn(GameObject ghost)
    {
        ghost.SetActive(false);
        yield return new WaitForSeconds(3f);
        ghost.SetActive(true);
    }
}
