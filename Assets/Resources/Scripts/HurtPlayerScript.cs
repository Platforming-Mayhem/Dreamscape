using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayerScript : MonoBehaviour
{
    [SerializeField]private int amount = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerScript player = collision.collider.GetComponent<PlayerScript>();
            player.SetHeight(player.GetMaxHeight());
            player.HurtPlayer(amount, transform.position);
        }
    }
}
