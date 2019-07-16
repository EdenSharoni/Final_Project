using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerScript : MonoBehaviour
{
    public GameObject aboveCamera;
    public GameObject angleCamera;
    public GameObject followCamera;
    public GameObject backCamera;
    public bool backCameraBool;

    private void Start()
    {
        backCameraBool = false;
    }
    void Update()
    {
        if (Input.GetKey("1"))
        {
            backCameraBool = false;
            aboveCamera.SetActive(true);
            angleCamera.SetActive(false);
            followCamera.SetActive(false);
            backCamera.SetActive(false);
        }

        if (Input.GetKey("2"))
        {
            backCameraBool = false;
            aboveCamera.SetActive(false);
            angleCamera.SetActive(true);
            followCamera.SetActive(false);
            backCamera.SetActive(false);
        }

        if (Input.GetKey("3"))
        {
            backCameraBool = false;
            aboveCamera.SetActive(false);
            angleCamera.SetActive(false);
            followCamera.SetActive(true);
            backCamera.SetActive(false);
        }
        if (Input.GetKey("4"))
        {
            backCameraBool = true;
            aboveCamera.SetActive(false);
            angleCamera.SetActive(false);
            followCamera.SetActive(false);
            backCamera.SetActive(true);

        }
    }
}
