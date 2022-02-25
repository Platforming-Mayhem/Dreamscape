using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SpawnpointScript : MonoBehaviour
{
    private bool isActive = false;

    [SerializeField] private Color activatedColour;

    [SerializeField] private Color deactivatedColour;

    private SpriteRenderer sRenderer;

    // Start is called before the first frame update
    void Start()
    {
        sRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            gameObject.tag = "Spawnpoint";
            sRenderer.color = activatedColour;
        }
        else
        {
            gameObject.tag = "Untagged";
            sRenderer.color = deactivatedColour;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach(GameObject spawnpoint in GameObject.FindGameObjectsWithTag("Spawnpoint"))
            {
                try
                {
                    SpawnpointScript sp = spawnpoint.GetComponent<SpawnpointScript>();
                    if (sp.isActive)
                    {
                        sp.isActive = false;
                    }
                }
                catch
                {
                    spawnpoint.SetActive(false);
                }
            }
            isActive = true;
        }
    }
}
