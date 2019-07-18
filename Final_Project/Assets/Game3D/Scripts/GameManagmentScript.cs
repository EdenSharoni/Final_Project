using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagmentScript : MonoBehaviour
{
    public AudioClip main;
    public AudioClip ghostBlueSound;
    public Button volumeOn;
    public Button volumeOff;
    public Text score;
    public Image ready;
    public Image gameOver;
    public Image[] lifes = new Image[3];

    private NavMeshGhost3DScript[] ghosts = new NavMeshGhost3DScript[4];
    private AudioSource audioSource;
    private Pacman3DScript pacman;
    private int currentLife = 2;
    private bool oneTimeEntrence;
    private GameObject food;

    public Text cameraInstruction;

    void Start()
    {
        PlayerPrefs.SetInt("points", 0);

        pacman = GameObject.Find("Pacman3D").GetComponent<Pacman3DScript>();
        ghosts[0] = GameObject.Find("GreenGhost").GetComponent<NavMeshGhost3DScript>();
        ghosts[1] = GameObject.Find("PinkGhost").GetComponent<NavMeshGhost3DScript>();
        ghosts[2] = GameObject.Find("RedGhost").GetComponent<NavMeshGhost3DScript>();
        ghosts[3] = GameObject.Find("YellowGhost").GetComponent<NavMeshGhost3DScript>();

        food = GameObject.Find("Food3D");
        audioSource = GetComponent<AudioSource>();

        volumeOn.gameObject.SetActive(true);
        volumeOff.gameObject.SetActive(false);
        volumeOn.enabled = true;
        volumeOff.enabled = false;
        StartCoroutine(StartCam());
    }

    IEnumerator StartCam()
    {
        cameraInstruction.gameObject.SetActive(false);
        gameOver.enabled = false;
        ready.enabled = false;
        yield return new WaitForSeconds(1f);
        StartCoroutine(TextBlink());
        yield return new WaitForSeconds(8f);
        cameraInstruction.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);        
        init();
    }
    IEnumerator TextBlink()
    {
        cameraInstruction.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        cameraInstruction.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        cameraInstruction.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        cameraInstruction.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        cameraInstruction.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        cameraInstruction.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        cameraInstruction.gameObject.SetActive(true);
    }

    void init()
    {
        StartCoroutine(Initial());
    }

    IEnumerator Initial()
    {
        audioSource.clip = main;
        pacman.isdead = false;
        oneTimeEntrence = true;
        gameOver.enabled = false;
        ready.enabled = true;
        yield return new WaitForSeconds(5f);
        VolumeOn();
        ready.enabled = false;
        audioSource.loop = true;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        score.text = PlayerPrefs.GetInt("points").ToString();

        if (pacman.ghostBlue && audioSource.clip == main && !pacman.isdead)
        {
            audioSource.Stop();
            audioSource.clip = ghostBlueSound;
            audioSource.Play();
        }

        else if (!pacman.ghostBlue && audioSource.clip == ghostBlueSound && !pacman.isdead)
        {
            audioSource.Stop();
            audioSource.clip = main;
            audioSource.Play();
        }

        if ((food.transform.childCount == 0 || pacman.isdead) && oneTimeEntrence)
        {
            StopAllCoroutines();
            oneTimeEntrence = false;
            gameOver.enabled = true;

            audioSource.Stop();

            pacman.audioSource.Stop();
            pacman.speed = 0f;
            pacman.GetComponent<Animator>().SetBool("move", false);
            pacman.ghostBlue = false;
            pacman.StopAllCoroutines();

            for (int i = 0; i < 4; i++)
            {
                ghosts[i].speed = 0f;
                ghosts[i].GetComponent<AudioSource>().Stop();
                ghosts[i].StopAllCoroutines();
                ghosts[i].gameObject.GetComponent<Renderer>().enabled = false;
            }

            if (pacman.isdead)
            {
                pacman.GetComponent<Animator>().enabled = true;
                pacman.audioSource.clip = pacman.deadSound;
                pacman.audioSource.PlayOneShot(pacman.deadSound);
                pacman.GetComponent<Animator>().SetTrigger("die");
            }

            if (currentLife >= 0 && food.transform.childCount != 0)
                StartCoroutine(PlayAgain());
            if(currentLife == -1)
                StartCoroutine(WaitTwoSeconds(0));
            if (food.transform.childCount == 0)
                StartCoroutine(WaitTwoSeconds(3));
        }
    }
    IEnumerator WaitTwoSeconds(int i)
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(i, LoadSceneMode.Single);
    }
    IEnumerator PlayAgain()
    {
        yield return new WaitForSeconds(2f);

        lifes[currentLife].enabled = false;
        currentLife--;

        pacman.initPacman();

        for (int i = 0; i < 4; i++)        
            ghosts[i].InitGhost();

        init();
    }

    public void VolumeOn()
    {
        for (int i = 0; i < 4; i++)
            ghosts[i].GetComponent<AudioSource>().volume = 0.3f;
        pacman.audioSource.volume = 0.3f;
        audioSource.volume = 0.3f;
        volumeOn.enabled = true;
        volumeOff.enabled = false;
        volumeOn.gameObject.SetActive(true);
        volumeOff.gameObject.SetActive(false);
    }

    public void VolumeOff()
    {
        for (int i = 0; i < 4; i++)
            ghosts[i].GetComponent<AudioSource>().volume = 0f;
        pacman.audioSource.volume = 0f;
        audioSource.volume = 0f;

        volumeOn.enabled = false;
        volumeOff.enabled = true;
        volumeOn.gameObject.SetActive(false);
        volumeOff.gameObject.SetActive(true);
    }

    public void Exit()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
