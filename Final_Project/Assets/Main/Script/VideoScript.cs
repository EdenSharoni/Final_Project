using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoScript : MonoBehaviour
{
    public AudioSource source;
    public RawImage trailer;
    public VideoPlayer videoPlayer;
    MainMenuScript main;
    bool oneTimeEntrence;

    void Start()
    {
        oneTimeEntrence = true;
        main = GameObject.Find("Main Camera").GetComponent<MainMenuScript>();
        main.video = false;
        videoPlayer.isLooping = false;
    }

    void Update()
    {
        if (main.video && oneTimeEntrence)
        {
            oneTimeEntrence = false;
            StartCoroutine(PlayVideo());
        }
        if (!main.video)
            Pause();

        else if (main.video)
            Play();
    }

    IEnumerator PlayVideo()
    {
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
        {
            yield return new WaitForSeconds(1f);
            break;
        }
        trailer.texture = videoPlayer.texture;
        Play();
        StartCoroutine(VideoLength());
    }

    IEnumerator VideoLength()
    {
        float time = (float)videoPlayer.length;
        yield return new WaitForSeconds(time - 1f);
        main.video = false;
    }

    void Pause()
    {
        videoPlayer.Pause();
        source.Pause();
    }
    void Play()
    {
        videoPlayer.Play();
        source.Play();
    }

}
