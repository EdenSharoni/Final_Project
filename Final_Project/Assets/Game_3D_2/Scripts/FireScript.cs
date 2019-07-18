using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{

    public int fireSpeed;
    private NavMeshGhost3DScript[] ghost = new NavMeshGhost3DScript[8];
    private Pacman3DScript pacman;
    int i;
    private void Start()
    {
        pacman = GameObject.Find("Pacman3D").GetComponent<Pacman3DScript>();
        ghost[0] = GameObject.Find("RedGhost").GetComponent<NavMeshGhost3DScript>();
        ghost[1] = GameObject.Find("OrangeGhost").GetComponent<NavMeshGhost3DScript>();
        ghost[2] = GameObject.Find("DarkGreenGhost").GetComponent<NavMeshGhost3DScript>();
        ghost[3] = GameObject.Find("GreenGhost").GetComponent<NavMeshGhost3DScript>();
        ghost[4] = GameObject.Find("PinkGhost").GetComponent<NavMeshGhost3DScript>();
        ghost[5] = GameObject.Find("LightBlueGhost").GetComponent<NavMeshGhost3DScript>();
        ghost[6] = GameObject.Find("YellowGhost").GetComponent<NavMeshGhost3DScript>();
        ghost[7] = GameObject.Find("PurpleGhost").GetComponent<NavMeshGhost3DScript>();
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
                    other.transform.position = ghost[i].startPosition;
                    Destroy(gameObject);
                }
        }
        else
            Destroy(gameObject, 2f);
    }
}
