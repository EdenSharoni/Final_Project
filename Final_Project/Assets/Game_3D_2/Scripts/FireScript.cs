using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{
    public AudioClip shoot;
    public AudioClip explode;
    public int fireSpeed;
    private NavMeshGhost3DScript[] ghost = new NavMeshGhost3DScript[8];
    private Pacman3DScript pacman;
    int i;
    AudioSource source;

    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<SphereCollider>().isTrigger = true;

        pacman = GameObject.Find("Pacman3D").GetComponent<Pacman3DScript>();
        ghost[0] = GameObject.Find("RedGhost").GetComponent<NavMeshGhost3DScript>();
        ghost[1] = GameObject.Find("OrangeGhost").GetComponent<NavMeshGhost3DScript>();
        ghost[2] = GameObject.Find("DarkGreenGhost").GetComponent<NavMeshGhost3DScript>();
        ghost[3] = GameObject.Find("GreenGhost").GetComponent<NavMeshGhost3DScript>();
        ghost[4] = GameObject.Find("PinkGhost").GetComponent<NavMeshGhost3DScript>();
        ghost[5] = GameObject.Find("LightBlueGhost").GetComponent<NavMeshGhost3DScript>();
        ghost[6] = GameObject.Find("YellowGhost").GetComponent<NavMeshGhost3DScript>();
        ghost[7] = GameObject.Find("PurpleGhost").GetComponent<NavMeshGhost3DScript>();

        source = GetComponent<AudioSource>();
        source.volume = 0.1f;
        source.PlayOneShot(shoot);
    }
    // Update is called once per frame
    void Update()
    {
        float amoutToMove = fireSpeed * Time.deltaTime;
        transform.Translate(Vector3.forward * amoutToMove);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ghost3D"))
        {
            PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + 200);
            for (i = 0; i < 8; i++)
                if (ghost[i].gameObject.Equals(other.gameObject))
                {
                    source.PlayOneShot(explode);
                    GetComponent<MeshRenderer>().enabled = false;
                    GetComponent<SphereCollider>().isTrigger = false;
                    ghost[i].explode = true;
                    StartCoroutine(Wait(ghost[i]));
                }
        }
        if (other.gameObject.CompareTag("Wall3D"))
            Destroy(gameObject);
    }
    IEnumerator Wait(NavMeshGhost3DScript ghost)
    {
        ghost.gameObject.GetComponent<MeshRenderer>().enabled = false;
        ghost.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        yield return new WaitForSeconds(2f);
        ghost.transform.position = ghost.startPosition;
        ghost.gameObject.GetComponent<MeshRenderer>().enabled = true;
        ghost.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        Destroy(gameObject);
    }
}
