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
    //bool back;
    private void Start()
    {
        //back = true;
        backCameraBool = false;
    }

    //IEnumerator BackToBackCamera()
    //{
    //     yield return new WaitForSeconds(5f);
    //     back = true;
    // }

    void Update()
    {
        if (Input.GetKey("1"))
        {
            //StartCoroutine(BackToBackCamera());
            backCameraBool = false;
            aboveCamera.SetActive(true);
            angleCamera.SetActive(false);
            followCamera.SetActive(false);
            backCamera.SetActive(false);
        }

        if (Input.GetKey("2"))
        {
            //StartCoroutine(BackToBackCamera());
            backCameraBool = false;
            aboveCamera.SetActive(false);
            angleCamera.SetActive(true);
            followCamera.SetActive(false);
            backCamera.SetActive(false);
        }

        if (Input.GetKey("3"))
        {
            //StartCoroutine(BackToBackCamera());
            backCameraBool = false;
            aboveCamera.SetActive(false);
            angleCamera.SetActive(false);
            followCamera.SetActive(true);
            backCamera.SetActive(false);
        }
        if (Input.GetKey("4"))//|back
        {
            //back = false;
            backCameraBool = true;
            aboveCamera.SetActive(false);
            angleCamera.SetActive(false);
            followCamera.SetActive(false);
            backCamera.SetActive(true);

        }
    }
}
