using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    public AudioClip gameSound;
    AudioSource audioSource;

    public Button volumeOn;
    public Button volumeOff;
    public Text score;
    public Image ready;
    public Image gameOver;
    public Image[] life = new Image[3];
    public int currentLife;
    PacManScript pacman;
    GameObject board;
    bool oneTimeEntrence;
    void Start()
    {
        oneTimeEntrence = true;
        audioSource = GetComponent<AudioSource>();

        PlayerPrefs.SetInt("gameOver", 0);
        board = GameObject.Find("Board");
        pacman = GameObject.Find("Pacman").GetComponent<PacManScript>();
        PlayerPrefs.SetInt("points", 0);
        score.text = "0";
        volumeOn.gameObject.SetActive(true);
        volumeOff.gameObject.SetActive(false);
        volumeOn.enabled = true;
        volumeOff.enabled = false;
        currentLife = 2;
        gameOver.enabled = false;
    }

    public void VolumeOn()
    {
        volumeOn.enabled = false;
        volumeOff.enabled = true;
        volumeOn.gameObject.SetActive(false);
        volumeOff.gameObject.SetActive(true);
    }

    public void VolumeOff()
    {
        volumeOn.enabled = true;
        volumeOff.enabled = false;
        volumeOn.gameObject.SetActive(true);
        volumeOff.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (board.transform.childCount == 0 || pacman.isdead)
        {
            PlayerPrefs.SetInt("gameOver", 1);
            gameOver.enabled = true;
        }
        score.text = PlayerPrefs.GetInt("points").ToString();

        if (pacman.afterInitAudio && oneTimeEntrence)
        {
            oneTimeEntrence = false;
            ready.enabled = false;
            audioSource.loop = true;
            audioSource.clip = gameSound;
            audioSource.Play();
        }
          
    }
}
