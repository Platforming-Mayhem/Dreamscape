using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishScript : MonoBehaviour
{
    private CollectibleManagerScript collectible;
    [SerializeField] private GameObject FadeOut;
    // Start is called before the first frame update
    void Start()
    {
        collectible = FindObjectOfType<CollectibleManagerScript>();
    }

    IEnumerator loadLevel()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(3);
    }

    // Update is called once per frame
    void Update()
    {
        if(collectible.collected >= collectible.poemLineByLine.Length)
        {
            FadeOut.SetActive(true);
            StartCoroutine("loadLevel");
        }
    }
}
