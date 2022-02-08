using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float Distance;
    public Vector3 offset;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = ((cam.transform.position + offset) / Distance) + Vector3.forward;
    }
}
