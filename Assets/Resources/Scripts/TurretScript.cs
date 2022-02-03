using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    int numberOfBullets = 3;
    float coolDown = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(coolDown > 0.0f)
        {
            if(numberOfBullets > 0)
            {
                coolDown -= Time.deltaTime;
            }
            else
            {
                coolDown = 4.0f;
                numberOfBullets = 3;
            }
        }
        else
        {
            Instantiate(bullet, transform.position + transform.right, Quaternion.identity).transform.right = transform.right;
            if(numberOfBullets > 0)
            {
                coolDown = 1.5f;
                numberOfBullets -= 1;
            }
            else
            {
                coolDown = 4.0f;
                numberOfBullets = 3;
            }
        }
    }
}
