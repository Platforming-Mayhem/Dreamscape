using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoemCollectibleScript : MonoBehaviour
{
    public string lineData = "";
    private CollectibleManagerScript managerScript;
    
    // Start is called before the first frame update
    void Start()
    {
        managerScript = FindObjectOfType<CollectibleManagerScript>();
    }

    private void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            managerScript.collected++;
            lineData = managerScript.poemLineByLine[managerScript.collected - 1];
            managerScript.ChangeText(lineData);
            gameObject.SetActive(false);
        }
    }
}
