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
    }

    public void ChangeText(string data)
    {
        text.text = data;
    }

    void AssignPoemLine()
    {
        PoemCollectibleScript[] poems = Resources.FindObjectsOfTypeAll<PoemCollectibleScript>();
        List<PoemCollectibleScript> collectibleScripts = new List<PoemCollectibleScript>();
        for (int i = 0; i < poems.Length; i++)
        {
            if(poems[i].gameObject != null)
            {
                collectibleScripts.Add(poems[i]);
            }
        }
        collectibleScripts.Reverse();
        collectibleScripts.RemoveAt(0);
        poems = collectibleScripts.ToArray();
        collectibleScripts.Clear();
        for (int j = 0; j < poems.Length; j++)
        {
            poems[j].lineData = poemLineByLine[j];
        }
    }

    // Update is called once per frame
    void Update()
    {
        AssignPoemLine();
    }
}
