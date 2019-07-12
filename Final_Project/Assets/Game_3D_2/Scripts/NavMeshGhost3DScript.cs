using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshGhost3DScript : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    Pacman3DScript pacman;
    Vector3 startPosition;
    Vector3[] directions = new Vector3[4];
    private Material material;
    private Color original;
    bool oneTimeBlue;

    RaycastHit forward;
    RaycastHit backwards;
    RaycastHit left;
    RaycastHit right;
    public LayerMask layermask;

    void Start()
    {
        pacman = GameObject.Find("Pacman3D").GetComponent<Pacman3DScript>();
        agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position;

        material = GetComponent<Renderer>().material;

        directions[0] = Vector3.forward;
        directions[1] = Vector3.right;
        directions[2] = Vector3.back;
        directions[3] = Vector3.left;
        StartCoroutine(Wait());

    }
    IEnumerator Wait()
    {
        oneTimeBlue = true;
        transform.position = startPosition;
        original = material.color;
        agent.speed = 0;
        yield return new WaitForSeconds(5f);
        StartCoroutine(WaitToGetOut());
    }

    IEnumerator WaitToGetOut()
    {
        if (transform.name.Equals("GreenGhost"))
            yield return new WaitForSeconds(3f);
        if (transform.name.Equals("DarkGreenGhost"))
            yield return new WaitForSeconds(5f);
        if (transform.name.Equals("PinkGhost"))
            yield return new WaitForSeconds(8f);
        if (transform.name.Equals("LightBlueGhost"))
            yield return new WaitForSeconds(10f);
        if (transform.name.Equals("YellowGhost"))
            yield return new WaitForSeconds(12f);
        if (transform.name.Equals("PurpleGhost"))
            yield return new WaitForSeconds(15f);
        agent.speed = 8;
    }

    void FixedUpdate()
    {
        FindRayCastHit();

        if (pacman.ghostBlue && oneTimeBlue)
            StartCoroutine(Blue());

        agent.SetDestination(target.position);
    }

    IEnumerator Blue()
    {
        oneTimeBlue = false;
        material.color = Color.blue;
        yield return new WaitForSeconds(3f);
        pacman.ghostBlue = false;
        material.color = original;
        oneTimeBlue = true;
    }

    void FindRayCastHit()
    {
        Debug.DrawRay(transform.position, Vector3.forward * 5f, Color.red);
        Debug.DrawRay(transform.position, Vector3.back * 5f, Color.red);
        Debug.DrawRay(transform.position, Vector3.right * 5f, Color.red);
        Debug.DrawRay(transform.position, Vector3.left * 5f, Color.red);
        if (Physics.Raycast(transform.position, Vector3.forward * 5f, out forward, 5, layermask))
        {
            Debug.Log(forward.collider.tag);
        }
    }
}
