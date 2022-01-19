using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraWithMouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButton(1))
        {
            transform.position += Vector3.right * Input.GetAxis("Mouse X") * Time.unscaledDeltaTime * 10.0f;
            transform.position += Vector3.up * Input.GetAxis("Mouse Y") * Time.unscaledDeltaTime * 10.0f;
        }
        Camera.main.orthographicSize -= Input.GetAxisRaw("Mouse ScrollWheel");
    }
}
