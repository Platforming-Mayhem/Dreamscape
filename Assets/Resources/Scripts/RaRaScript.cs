using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaRaScript : MonoBehaviour
{
    public bool activate;

    private bool isEnabled = false;

    PlayerScript player;

    Vector2 direction;

    public void activateRaRa()
    {
        isEnabled = true;
        player = FindObjectOfType<PlayerScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnabled)
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(player.transform.position.x, transform.position.y), Time.deltaTime);
        }
        if (activate)
        {
            activateRaRa();
        }
    }
}
