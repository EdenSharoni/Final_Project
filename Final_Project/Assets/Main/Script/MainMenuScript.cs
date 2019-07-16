using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainMenuScript : MonoBehaviour
{

    public Button howToPlayBtn;
    public Button difficultyBtn;
    public Button game1;
    public Button game2;
    public Button game3;
    public Button trailer;
    public GameObject panel;
    public bool video;

    void Start()
    {
        video = false;
        howToPlayBtn.enabled = true;
        howToPlayBtn.gameObject.SetActive(true);

        difficultyBtn.enabled = true;
        difficultyBtn.gameObject.SetActive(true);

        game1.enabled = false;
        game1.gameObject.SetActive(false);

        game2.enabled = false;
        game2.gameObject.SetActive(false);

        game3.enabled = false;
        game3.gameObject.SetActive(false);

        trailer.enabled = true;
        trailer.gameObject.SetActive(true);

        panel.gameObject.SetActive(false);

    }

    public void Video()
    {
        video = !video;
    }

    public void HowToPlay()
    {
        panel.gameObject.SetActive(!panel.gameObject.activeSelf);
        difficultyBtn.enabled = !difficultyBtn.enabled;
        difficultyBtn.gameObject.SetActive(!difficultyBtn.gameObject.activeSelf);
    }

    public void Difficulty()
    {
        game1.enabled = !game1.enabled;
        game1.gameObject.SetActive(!game1.gameObject.activeSelf);

        game2.enabled = !game2.enabled;
        game2.gameObject.SetActive(!game2.gameObject.activeSelf);

        game3.enabled = !game3.enabled;
        game3.gameObject.SetActive(!game3.gameObject.activeSelf);

        trailer.enabled = !trailer.enabled;
        trailer.gameObject.SetActive(!trailer.gameObject.activeSelf);
    }
}
