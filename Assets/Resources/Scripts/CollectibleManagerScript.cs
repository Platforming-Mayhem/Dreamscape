using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectibleManagerScript : MonoBehaviour
{
    [SerializeField] private TextAsset poemData;
    [SerializeField] private TMP_Text text;

    public string[] poemLineByLine;

    public int collected = 0;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        poemLineByLine = poemData.text.Split('\n');
        anim = GetComponent<Animator>();
    }

    public void ChangeText(string data)
    {
        text.text = data;
        anim.SetTrigger("Entrance");
    }
}
