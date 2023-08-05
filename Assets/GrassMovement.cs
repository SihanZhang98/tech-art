using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GrassMovement : MonoBehaviour
{
   
    GameObject displacer;
    void Start()
    {

        displacer = GameObject.FindWithTag("Fox");
    }

    void Update()
    {
        Renderer r = GetComponent<Renderer>();
        r.material.SetVector("_PushOrigin", displacer.transform.position);
    }
}
