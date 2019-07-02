using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    public Button volumeOn;
    public Button volumeOff;
    public Text score;

    void Start()
    {
        PlayerPrefs.SetInt("points", 0);
        score.text = "0";
        volumeOn.gameObject.SetActive(true);
        volumeOff.gameObject.SetActive(false);
        volumeOn.enabled = true;
        volumeOff.enabled = false;
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
        score.text = PlayerPrefs.GetInt("points").ToString();
    }
}
