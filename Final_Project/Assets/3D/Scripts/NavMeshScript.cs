using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshScript : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    private float timeTicking;
    private PacmanControllerScript pacman3d;
    private Material material;
    private Color original;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        pacman3d = GameObject.Find("Pacman3D").GetComponent<PacmanControllerScript>();
        timeTicking = Time.time;
        material = GetComponent<Renderer>().material;
        original = material.color;
    }

    // Update is called once per frame
    void Update()
    {       
        if (agent.name == "Yellowy")
            agent.SetDestination(target.position);
        if (agent.name == "Pinky" && Time.time - timeTicking > 5f)
            agent.SetDestination(target.position);
        if (agent.name == "Redmy" && Time.time - timeTicking > 10f)
            agent.SetDestination(target.position);
        if (agent.name == "Greeny" && Time.time - timeTicking > 15f)
            agent.SetDestination(target.position);

        if (pacman3d.blue3DGhost)
            material.color = Color.blue;
        else
            material.color = original;
    }
}
