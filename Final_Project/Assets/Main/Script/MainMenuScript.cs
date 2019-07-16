using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainMenuScript : MonoBehaviour
{
    public AudioSource source;
    public Button howToPlayBtn;
    public Button difficultyBtn;
    public Button game1;
    public Button game2;
    public Button game3;
    public RawImage trailer;
    public VideoPlayer videoPlayer;
    public GameObject panel;

    void Start()
    {
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

        //trailer.enabled = true;
        //trailer.gameObject.SetActive(true);

        panel.gameObject.SetActive(false);

    }

    public void Video()
    {
        StartCoroutine(PlayVideo());
    }

    IEnumerator PlayVideo()
    {
        Debug.Log("aaa");
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
        {
            yield return new WaitForSeconds(1f);
            break;
        }
        trailer.texture = videoPlayer.texture;
        videoPlayer.Play();
        source.Play();
    }

    public void HowToPlay()
    {
        panel.gameObject.SetActive(!panel.gameObject.activeSelf);
        difficultyBtn.enabled = !difficultyBtn.enabled;
        difficultyBtn.gameObject.SetActive(!difficultyBtn.gameObject.activeSelf);
    }

}
