using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform playerObject;

    private Vector3 cameraOffset;

    [Range(0.01f, 1.0f)]
    public float smoothFactor = .5f;

    void Start()
    {
        cameraOffset = transform.position - playerObject.position;
    }

    // Update is called once per frame
    void LateUpdate () {
        Vector3 newPosition = playerObject.position + cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);
	}
}
