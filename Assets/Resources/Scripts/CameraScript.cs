using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-10.25f, -1.0f, -10.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset - (Vector3.forward * 10.0f), Time.deltaTime * speed);
    }
}
