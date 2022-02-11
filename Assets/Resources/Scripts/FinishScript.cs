using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishScript : MonoBehaviour
{
    private CollectibleManagerScript collectible;
    // Start is called before the first frame update
    void Start()
    {
        collectible = FindObjectOfType<CollectibleManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(collectible.collected >= collectible.poemLineByLine.Length)
        {
            SceneManager.LoadScene(2);
        }
    }
}
