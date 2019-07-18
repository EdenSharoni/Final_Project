using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManagerScript : MonoBehaviour
{
    public AudioClip main;
    public AudioClip ghostBlueSound;
    public Button volumeOn;
    public Button volumeOff;
    public Text score;
    public Text fireCount;
    public Image ready;
    public Image gameOver;
    public Image[] life = new Image[3];
    public Text cameraInstruction;

    private NavMeshGhost3DScript[] ghost = new NavMeshGhost3DScript[8];
    private AudioSource audioSource;
    private Pacman3DScript pacman;
    private GameObject food;
    private int currentLife = 2;
    private bool oneTimeEntrence;

    void Start()
    {
        PlayerPrefs.SetInt("points", 0);

        pacman = GameObject.Find("Pacman3D").GetComponent<Pacman3DScript>();
        ghost[0] = GameObject.Find("RedGhost").GetComponent<NavMeshGhost3DScript>();
        ghost[1] = GameObject.Find("OrangeGhost").GetComponent<NavMeshGhost3DScript>();
        ghost[2] = GameObject.Find("DarkGreenGhost").GetComponent<NavMeshGhost3DScript>();
        ghost[3] = GameObject.Find("GreenGhost").GetComponent<NavMeshGhost3DScript>();
        ghost[4] = GameObject.Find("PinkGhost").GetComponent<NavMeshGhost3DScript>();
        ghost[5] = GameObject.Find("LightBlueGhost").GetComponent<NavMeshGhost3DScript>();
        ghost[6] = GameObject.Find("YellowGhost").GetComponent<NavMeshGhost3DScript>();
        ghost[7] = GameObject.Find("PurpleGhost").GetComponent<NavMeshGhost3DScript>();

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
        StartCoroutine(Starter());
    }

    IEnumerator Starter()
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

    void Update()
    {

        score.text = "Points:  " + PlayerPrefs.GetInt("points").ToString();
        fireCount.text = "Bullets:  " + PlayerPrefs.GetInt("fireCount").ToString();

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

            for (int i = 0; i < 8; i++)
            {
                ghost[i].speed = 0f;
                ghost[i].GetComponent<AudioSource>().Stop();
                ghost[i].StopAllCoroutines();
                ghost[i].gameObject.GetComponent<Renderer>().enabled = false;
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
            else
                SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }

    IEnumerator PlayAgain()
    {
        yield return new WaitForSeconds(2f);

        life[currentLife].enabled = false;
        currentLife--;

        pacman.initPacman();

        for (int i = 0; i < 8; i++)
            ghost[i].InitGhost();

        init();
    }

    public void VolumeOn()
    {
        for (int i = 0; i < 8; i++)
            ghost[i].GetComponent<AudioSource>().volume = 0.3f;
        pacman.audioSource.volume = 0.3f;
        audioSource.volume = 0.3f;
        volumeOn.enabled = true;
        volumeOff.enabled = false;
        volumeOn.gameObject.SetActive(true);
        volumeOff.gameObject.SetActive(false);
    }

    public void VolumeOff()
    {
        for (int i = 0; i < 8; i++)
            ghost[i].GetComponent<AudioSource>().volume = 0f;
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
