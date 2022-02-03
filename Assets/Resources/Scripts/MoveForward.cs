using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    float timeAlive = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
        if(timeAlive > 0.0f)
        {
            timeAlive -= Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
