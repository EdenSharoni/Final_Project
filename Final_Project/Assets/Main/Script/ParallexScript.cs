using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallexScript : MonoBehaviour
{
    public float speed;

    void Update()
    {
        GetComponent<Image>().materialForRendering.mainTextureOffset = new Vector2(Time.time * speed, 0);
    }
}