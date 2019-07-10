﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBetweenCamerasScript : MonoBehaviour
{
    public GameObject aboveCamera;
    public GameObject angleCamera;
    public GameObject followCamera;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("1"))
        {
            aboveCamera.SetActive(true);
            angleCamera.SetActive(false);
            followCamera.SetActive(false);
        }

        if (Input.GetKey("2"))
        {
            aboveCamera.SetActive(false);
            angleCamera.SetActive(true);
            followCamera.SetActive(false);
        }

        if (Input.GetKey("3"))
        {
            aboveCamera.SetActive(false);
            angleCamera.SetActive(false);
            followCamera.SetActive(true);
        }
    }
}
