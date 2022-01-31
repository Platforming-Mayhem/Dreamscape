using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectibleManagerScript : MonoBehaviour
{
    [SerializeField] private TextAsset poemData;
    [SerializeField] private TMP_Text text;

    private string[] poemLineByLine;

    

    // Start is called before the first frame update
    void Start()
    {
        poemLineByLine = poemData.text.Split('\n');
        PoemCollectibleScript[] poems = FindObjectsOfType<PoemCollectibleScript>();
        System.Array.Reverse(poems);
        for (int i = 0; i < poems.Length; i++)
        {
            poems[i].poemData = poemLineByLine[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
