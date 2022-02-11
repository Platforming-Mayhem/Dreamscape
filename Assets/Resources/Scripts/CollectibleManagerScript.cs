using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectibleManagerScript : MonoBehaviour
{
    [SerializeField] private TextAsset poemData;
    [SerializeField] private TMP_Text text;
    [SerializeField] private AudioClip collectPoem;

    public string[] poemLineByLine;

    public int collected = 0;

    private Animator anim;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        poemLineByLine = poemData.text.Split('\n');
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void ChangeText(string data)
    {
        text.text = data;
        anim.SetTrigger("Entrance");
        audioSource.PlayOneShot(collectPoem);
    }
}
