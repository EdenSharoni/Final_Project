using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshScript : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    private float timeTicking;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timeTicking = Time.time;
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
    }
}
