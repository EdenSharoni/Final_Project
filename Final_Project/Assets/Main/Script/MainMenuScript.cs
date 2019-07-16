using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    public Button howToPlayBtn;
    public Button difficultyBtn;
    public Button game1;
    public Button game2;
    public Button game3;
    public Button trailer;
    public GameObject howToPlayPanel;
    public GameObject gamesPanel;
    public bool video;

    void Start()
    {
        video = false;
        howToPlayBtn.enabled = true;
        howToPlayBtn.gameObject.SetActive(true);

        difficultyBtn.enabled = true;
        difficultyBtn.gameObject.SetActive(true);

        trailer.enabled = true;
        trailer.gameObject.SetActive(true);

        howToPlayPanel.gameObject.SetActive(false);
        gamesPanel.gameObject.SetActive(true);

    }

    public void Video()
    {
        video = !video;
    }

    public void HowToPlay()
    {
        howToPlayPanel.gameObject.SetActive(!howToPlayPanel.gameObject.activeSelf);
        difficultyBtn.enabled = !difficultyBtn.enabled;
        difficultyBtn.gameObject.SetActive(!difficultyBtn.gameObject.activeSelf);
        game1.enabled = !game1.enabled;
        game1.gameObject.SetActive(!game1.gameObject.activeSelf);

        game2.enabled = !game2.enabled;
        game2.gameObject.SetActive(!game2.gameObject.activeSelf);

        game3.enabled = !game3.enabled;
        game3.gameObject.SetActive(!game3.gameObject.activeSelf);
    }

    public void Difficulty()
    {
        if (gamesPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ButtonSetStart"))
            gamesPanel.GetComponent<Animator>().SetTrigger("pressOff");

        else
            gamesPanel.GetComponent<Animator>().SetTrigger("pressOn");
    }

    public void Pacman1()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void Pacman2()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    public void Pacman3()
    {
        SceneManager.LoadScene(3, LoadSceneMode.Single);
    }
}
