using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{
    Material mat;
    float speed = 0.05f;
    
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        Vector2 sky = mat.mainTextureOffset;
        sky.x += speed * Time.deltaTime;

        mat.mainTextureOffset = sky;
    }
}
