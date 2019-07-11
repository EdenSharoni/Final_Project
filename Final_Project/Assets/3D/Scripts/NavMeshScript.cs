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
    public bool isGhostDead;
    private Vector3 initialGhostPosition = Vector3.zero;
    private bool ifDied;
    public LayerMask layerMask;

    RaycastHit forward;
    RaycastHit backwards;
    RaycastHit left;
    RaycastHit right;

    Vector3[] directions = new Vector3[4];
    private int random = 0;
    private bool goingOnce;
    private bool finished;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        pacman3d = GameObject.Find("Pacman3D").GetComponent<PacmanControllerScript>();
        timeTicking = Time.time;
        material = GetComponent<Renderer>().material;
        original = material.color;
        initialGhostPosition = transform.position;

        directions[0] = Vector3.forward;
        directions[1] = Vector3.right;
        directions[2] = Vector3.back;
        directions[3] = Vector3.left;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (goingOnce)
            random = Random.Range(0, 3);

        if (agent.name == "Yellowy")
        {
            //agent.SetDestination(target.position);
            //agent.transform.Translate(Vector3.forward);
            Algorithm();           
        }
        if (agent.name == "Pinky" && Time.time - timeTicking > 5f)
            agent.SetDestination(target.position);
        if (agent.name == "Redmy" && Time.time - timeTicking > 10f)
            agent.SetDestination(target.position);
        if (agent.name == "Greeny" && Time.time - timeTicking > 15f)
            agent.SetDestination(target.position);

        
        if (isGhostDead)
        {
            ifDied = true;
            agent.gameObject.transform.position = initialGhostPosition;
            material.color = original;
            agent.SetDestination(target.position);
            isGhostDead = false;
        }

        if (pacman3d.blue3DGhost)
        {
            if (!ifDied)
                material.color = Color.blue;
        }
        else
        {
            material.color = original;
            ifDied = false;
        }
    }

    void Algorithm()
    {
        if (finished)
        {
            finished = false;

            FindRayCastHit();

            StartCoroutine(Wait());
        }

    }

    IEnumerator Wait()
    {
        goingOnce = false;
        yield return new WaitForSeconds(.5f);
        finished = true;
        goingOnce = true;
    }

    void FindRayCastHit()
    {
        if (Physics.CapsuleCast(transform.position, Vector3.forward, 4f, Vector3.forward, out forward, 8f, layerMask))
        {
            agent.gameObject.transform.Translate(directions[random] * agent.speed * Time.deltaTime);
            Debug.DrawRay(transform.position, Vector3.forward);
        }
        else if (Physics.CapsuleCast(transform.position, Vector3.back, 4f, Vector3.back, out backwards, 8f, layerMask))
        {
            agent.gameObject.transform.Translate(directions[random] * agent.speed * Time.deltaTime);
            Debug.DrawRay(transform.position, Vector3.back);
        }
        else if (Physics.CapsuleCast(transform.position, Vector3.left, 4f, Vector3.left, out left, 8f, layerMask))
        {
            agent.gameObject.transform.Translate(directions[random] * agent.speed * Time.deltaTime);
            Debug.DrawRay(transform.position, Vector3.left);
        }
        else if (Physics.CapsuleCast(transform.position, Vector3.right, 4f, Vector3.right, out right, 8f, layerMask))
        {
            agent.gameObject.transform.Translate(directions[random] * agent.speed * Time.deltaTime);
            Debug.DrawRay(transform.position, Vector3.right);
        }
    }
}
