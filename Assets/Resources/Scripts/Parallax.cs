using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float Distance;
    public Vector3 offset;
    [SerializeField] private bool freezeX;
    [SerializeField] private bool freezeY;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(!freezeX && !freezeY)
        {
            transform.position = (Vector3.Scale(cam.transform.position + offset, Vector3.one - Vector3.forward) / Distance);
        }
        else if (freezeX)
        {
            transform.position = (Vector3.Scale(cam.transform.position + offset, Vector3.one - Vector3.forward - Vector3.right) / Distance) + Vector3.Scale(transform.position, Vector3.right);
        }
        else if (freezeY)
        {
            transform.position = (Vector3.Scale(cam.transform.position + offset, Vector3.one - Vector3.forward - Vector3.up) / Distance) + Vector3.Scale(transform.position, Vector3.up);
        }
        else if(freezeX && freezeY)
        {
            transform.position = transform.position;
        }
    }
}
