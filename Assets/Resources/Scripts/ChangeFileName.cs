using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SFB;

public class ChangeFileName : MonoBehaviour
{
    GenerateLevelFromFile g;
    [SerializeField] private TMP_InputField text;
    private void Start()
    {
        g = FindObjectOfType<GenerateLevelFromFile>();
        CreateWindow();
    }

    public void ChangeName()
    {
        g.dataPath = text.text;
    }

    public void CreateWindow()
    {
        StandaloneFileBrowser.OpenFilePanel("Open", "C:", ".txt", false);
    }
}
