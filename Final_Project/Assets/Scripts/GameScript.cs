using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    public AudioClip main;
    public AudioClip ghostBlueSound;
    public Button volumeOn;
    public Button volumeOff;
    public Text score;
    public Image ready;
    public Image gameOver;
    public Image[] life = new Image[3];
    GostControllerScript[] ghost = new GostControllerScript[4];
    AudioSource audioSource;
    PacManScript pacman;
    GameObject board;
    int currentLife = 2;
    bool oneTimeEntrence;

    void Start()
    {
        Physics2D.IgnoreLayerCollision(11, 11);
        Physics2D.IgnoreLayerCollision(11, 13);
        Physics2D.IgnoreLayerCollision(12, 13);
        Physics2D.IgnoreLayerCollision(10, 13);
        PlayerPrefs.SetInt("points", 0);

        pacman = GameObject.Find("Pacman").GetComponent<PacManScript>();
        ghost[0] = GameObject.Find("RedGost").GetComponent<GostControllerScript>();
        ghost[1] = GameObject.Find("PinkGost").GetComponent<GostControllerScript>();
        ghost[2] = GameObject.Find("LightBlueGost").GetComponent<GostControllerScript>();
        ghost[3] = GameObject.Find("YellowGost").GetComponent<GostControllerScript>();
        audioSource = GetComponent<AudioSource>();
        board = GameObject.Find("Board");
        volumeOn.gameObject.SetActive(true);
        volumeOff.gameObject.SetActive(false);
        volumeOn.enabled = true;
        volumeOff.enabled = false;

        init();
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

    private void Update()
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

        if ((board.transform.childCount == 0 || pacman.isdead) && oneTimeEntrence)
        {
            StopAllCoroutines();
            oneTimeEntrence = false;
            gameOver.enabled = true;
            audioSource.Stop();
            pacman.audioSource.Stop();
            pacman.GetComponent<Animator>().SetBool("move", false);
            pacman.ghostBlue = false;
            pacman.rb.constraints = RigidbodyConstraints2D.FreezeAll;
            pacman.StopAllCoroutines();

            for (int i = 0; i < 4; i++)
            {
                ghost[i].GetComponent<AudioSource>().Stop();
                ghost[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                ghost[i].StopAllCoroutinesInAllScripts();
            }

            if (pacman.isdead)
            {
                pacman.GetComponent<Animator>().enabled = true;
                pacman.audioSource.clip = pacman.deadSound;
                pacman.audioSource.PlayOneShot(pacman.deadSound);
                pacman.GetComponent<Animator>().SetTrigger("die");
            }

            if (currentLife >= 0 && board.transform.childCount != 0)
                StartCoroutine(PlayAgain());
        }
    }

    IEnumerator PlayAgain()
    {
        yield return new WaitForSeconds(2f);

        life[currentLife].enabled = false;
        currentLife--;

        pacman.initPacman();

        for (int i = 0; i < 4; i++)
            ghost[i].initGhost();

        init();
    }

    public void VolumeOn()
    {
        for (int i = 0; i < 4; i++)
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
        for (int i = 0; i < 4; i++)
            ghost[i].GetComponent<AudioSource>().volume = 0f;
        pacman.audioSource.volume = 0f;
        audioSource.volume = 0f;

        volumeOn.enabled = false;
        volumeOff.enabled = true;
        volumeOn.gameObject.SetActive(false);
        volumeOff.gameObject.SetActive(true);
    }
}
