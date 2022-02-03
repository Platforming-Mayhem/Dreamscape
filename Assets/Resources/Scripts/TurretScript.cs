using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] int numberOfBullets = 3;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (numberOfBullets > 0)
        {
            anim.SetTrigger("Shoot");
            numberOfBullets -= 1;
        }
        else
        {
            anim.SetTrigger("Reload");
        }
    }

    public void Shoot()
    {
        Instantiate(bullet, transform.position + transform.right, Quaternion.identity).transform.right = transform.right;
    }

    public void Reload()
    {
        numberOfBullets = 3;
    }
}
