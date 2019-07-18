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
    public Image play;
    VideoScript videoScript;

    void Start()
    {
        video = false;
        videoScript = GameObject.Find("Pacman Video").GetComponent<VideoScript>();

        howToPlayBtn.enabled = true;
        howToPlayBtn.gameObject.SetActive(true);

        difficultyBtn.enabled = true;
        difficultyBtn.gameObject.SetActive(true);

        trailer.enabled = true;
        trailer.gameObject.SetActive(true);

        howToPlayPanel.gameObject.SetActive(false);
        gamesPanel.gameObject.SetActive(true);

        play.gameObject.SetActive(true);
    }
    private void Update()
    {
        if (videoScript.videoEnded)
        {
            videoScript.videoEnded = false;
            GetComponent<AudioSource>().Play();
        }
    }

    public void Video()
    {
        video = !video;
        if (video)
            GetComponent<AudioSource>().Pause();
        else
            GetComponent<AudioSource>().Play();
        play.gameObject.SetActive(false);
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
        trailer.enabled = !trailer.enabled;

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
