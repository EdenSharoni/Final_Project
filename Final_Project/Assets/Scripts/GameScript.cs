﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    public AudioClip gameSound;
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
        PlayerPrefs.SetInt("points", 0);
        score.text = PlayerPrefs.GetInt("points").ToString();

        pacman = GameObject.Find("Pacman").GetComponent<PacManScript>();
        ghost[0] = GameObject.Find("RedGost").GetComponent<GostControllerScript>();
        ghost[1] = GameObject.Find("PinkGost").GetComponent<GostControllerScript>();
        ghost[2] = GameObject.Find("LightBlueGost").GetComponent<GostControllerScript>();
        ghost[3] = GameObject.Find("YellowGost").GetComponent<GostControllerScript>();

        audioSource = GetComponent<AudioSource>();
        board = GameObject.Find("Board");

        oneTimeEntrence = true;

        volumeOn.gameObject.SetActive(true);
        volumeOff.gameObject.SetActive(false);
        volumeOn.enabled = true;
        volumeOff.enabled = false;

        gameOver.enabled = false;

        StartCoroutine(Starter());
    }

    IEnumerator Starter()
    {
        yield return new WaitForSeconds(5f);
        ready.enabled = false;
        audioSource.loop = true;
        audioSource.clip = gameSound;
        audioSource.Play();
    }

    private void Update()
    {
        score.text = PlayerPrefs.GetInt("points").ToString();

        if ((board.transform.childCount == 0 || pacman.isdead) && oneTimeEntrence)
        {
            oneTimeEntrence = false;
            gameOver.enabled = true;
            audioSource.Stop();

            pacman.audioSource.Stop();
            pacman.audioSource.clip = pacman.deadSound;
            pacman.audioSource.PlayOneShot(pacman.deadSound);
            pacman.GetComponent<Animator>().SetTrigger("die");
            pacman.rb.constraints = RigidbodyConstraints2D.FreezeAll;

            for (int i = 0; i < 4; i++)
                ghost[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

            if (life.Length > 0 && board.transform.childCount != 0)
                StartCoroutine(PlayAgain());
        }
    }

    IEnumerator PlayAgain()
    {
        yield return new WaitForSeconds(2f);
        pacman.transform.position = pacman.startPoint;
        for (int i = 0; i < 4; i++)
        {
            ghost[i].transform.position = ghost[i].startPoint;
            ghost[i].gostScript.enabled = false;
            ghost[i].gostAI.enabled = false;
            ghost[i].gostGoHomeAIScript.enabled = false;
        }
        life[currentLife].enabled = false;
        currentLife--;
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
}
