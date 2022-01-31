using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManagerScript : MonoBehaviour
{
    [SerializeField] private TextAsset poemData;
    [SerializeField] private int line = 0;
    // Start is called before the first frame update
    void Start()
    {
        List<PoemCollectibleScript> poems = new List<PoemCollectibleScript>();
        PoemCollectibleScript[] poemCollectibleScripts = FindObjectsOfType<PoemCollectibleScript>();
        poems.AddRange(poemCollectibleScripts);
        /*for (int i = 0; i < poemCollectibleScripts.Length; i++)
        {
            Debug.Log(poemCollectibleScripts[i].name);
        }*/


        Debug.Log(poems);
        poems.Sort(delegate (PoemCollectibleScript poem1, PoemCollectibleScript poem2)
        {
            return poem1.transform.position.x.CompareTo(poem2.transform.position.x);
        });
        for (int i = 0; i < poems.Count - 1; i++)
        {
            Debug.Log(poems[i].name);
        }
        foreach (PoemCollectibleScript p in poems)
        {
            if (p.line > previousLine)
            {
                previousLine = p.line;
            }
        }
        line = previousLine + 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
