using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{

    public int fireSpeed;
    private NavMeshGhost3DScript ghost;    
    private GameObject fireGhostPoints;
    // Update is called once per frame
    void Update()
    {
        float amoutToMove = fireSpeed * Time.deltaTime;
        transform.Translate(Vector3.forward * amoutToMove);

        if (transform.position.z > 10)
            Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ghost3D")
        {
            PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + 500);
            other.transform.position = ghost.startPosition;
            ghost.InitGhost();
            StartCoroutine(WaitForCoupleSeconds());
            Destroy(gameObject);
        }
    }

    IEnumerator WaitForCoupleSeconds()
    {
        fireGhostPoints.SetActive(true);
        yield return new WaitForSeconds(1f);
        fireGhostPoints.SetActive(false);
    }
}
