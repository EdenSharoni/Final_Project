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

    public float cubeSize = 0.2f;
    public int cubesInRow = 5;

    float cubesPivotDistance;
    Vector3 cubesPivot;

    public float explosionForce = 50f;
    public float explosionRadius = 4f;
    public float explosionUpward = 0.4f;

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

        cubesPivotDistance = cubeSize * cubesInRow / 2;
        cubesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * fireSpeed * Time.deltaTime);
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
                    Explode(ghost[i]);
                    StartCoroutine(Wait(ghost[i]));
                }
        }
        if (other.gameObject.CompareTag("Wall3D"))
            Destroy(gameObject);
    }

    public void Explode(NavMeshGhost3DScript ghost)
    {
        //loop 3 times to create 5x5x5 pieces in x,y,z coordinates
        for (int x = 0; x < cubesInRow; x++)
            for (int y = 0; y < cubesInRow; y++)
                for (int z = 0; z < cubesInRow; z++)
                    createPiece(x, y, z, ghost);

        //get explosion position
        Vector3 explosionPos = ghost.transform.position;
        //get colliders in that position and radius
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
        //add explosion force to all colliders in that overlap sphere
        foreach (Collider hit in colliders)
        {
            //get rigidbody from collider object
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
                //add explosion force to this body with given parameters
                rb.AddExplosionForce(explosionForce, ghost.transform.position, explosionRadius, explosionUpward);
        }
    }

    void createPiece(int x, int y, int z, NavMeshGhost3DScript ghost)
    {
        //create piece
        GameObject piece;
        piece = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //set piece position and scale
        piece.transform.position = transform.position + new Vector3(cubeSize * x, cubeSize * y, cubeSize * z) - cubesPivot;
        piece.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

        //add rigidbody and set mass
        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = cubeSize;
        piece.GetComponent<Renderer>().sharedMaterial = ghost.material;

        Destroy(piece, 2);
    }

    IEnumerator Wait(NavMeshGhost3DScript ghost)
    {
        ghost.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        ghost.agent.enabled = false;
        ghost.speed = 0f;
        ghost.gameObject.GetComponent<MeshRenderer>().enabled = false;
        ghost.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        yield return new WaitForSeconds(2f);
        ghost.transform.position = ghost.startPosition;
        ghost.gameObject.GetComponent<MeshRenderer>().enabled = true;
        ghost.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        if (!pacman.isdead)
            ghost.speed = 8f;
        ghost.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        Destroy(gameObject);
    }
}
