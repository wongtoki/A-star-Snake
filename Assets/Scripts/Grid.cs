using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
 
    public int width;
    public int height;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 topLeft, topRight, bottomLeft, bottomRight;
        topLeft = Vector3.zero + Vector3.left * width + Vector3.up * height;
        topRight = Vector3.zero + Vector3.right * width + Vector3.up * height;
        bottomLeft = topLeft + Vector3.down * height * 2;
        bottomRight = topRight + Vector3.down * height * 2;

        Gizmos.DrawLine(topLeft,topRight);
        Gizmos.DrawLine(topLeft, bottomLeft);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomLeft, bottomRight);


    }

}
