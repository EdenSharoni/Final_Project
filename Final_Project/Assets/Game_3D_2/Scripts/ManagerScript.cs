using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerScript : MonoBehaviour
{
    public AudioClip main;
    public AudioClip ghostBlueSound;
    //public Button volumeOn;
    //public Button volumeOff;
    //public Text score;
   // public Image ready;
   // public Image gameOver;
    //public Image[] life = new Image[3];
    NavMeshGhost3DScript[] ghost = new NavMeshGhost3DScript[8];
    AudioSource audioSource;
    Pacman3DScript pacman;
    GameObject board;
    int currentLife = 2;
    bool oneTimeEntrence;
    GameObject food;

    // Start is called before the first frame update
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
        //volumeOn.gameObject.SetActive(true);
        //volumeOff.gameObject.SetActive(false);
        //volumeOn.enabled = true;
        //volumeOff.enabled = false;

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
        //gameOver.enabled = false;
        //ready.enabled = true;
        yield return new WaitForSeconds(5f);
        VolumeOn();
        //ready.enabled = false;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void VolumeOn()
    {
        for (int i = 0; i < 8; i++)
            ghost[i].GetComponent<AudioSource>().volume = 1f;
        pacman.audioSource.volume = 1f;
        audioSource.volume = 1f;
        //volumeOn.enabled = true;
        //volumeOff.enabled = false;
        //volumeOn.gameObject.SetActive(true);
        //volumeOff.gameObject.SetActive(false);
    }

    void Update()
    {
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
            //gameOver.enabled = true;
            audioSource.Stop();

            pacman.audioSource.Stop();
            pacman.GetComponent<Animator>().SetBool("move", false);
            pacman.ghostBlue = false;
            pacman.rb.constraints = RigidbodyConstraints.FreezeAll;
            pacman.StopAllCoroutines();

            for (int i = 0; i < 4; i++)
            {
                ghost[i].GetComponent<AudioSource>().Stop();
                ghost[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                ghost[i].StopAllCoroutines();
            }

            if (pacman.isdead)
            {
                pacman.GetComponent<Animator>().enabled = true;
                pacman.audioSource.clip = pacman.deadSound;
                pacman.audioSource.PlayOneShot(pacman.deadSound);
                pacman.GetComponent<Animator>().SetTrigger("die");
            }

            /*if (currentLife >= 0 && board.transform.childCount != 0)
                StartCoroutine(PlayAgain());*/
        }
    }
}
