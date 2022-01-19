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
    }

    public void ChangeName()
    {
        g.dataPath = text.text;
    }

    public void CreateWindow()
    {
        text.text = StandaloneFileBrowser.OpenFilePanel("Open", "", "txt", false)[0];
        g.LoadLevelFromFile();
    }

    public void SaveFile()
    {
        text.text = StandaloneFileBrowser.SaveFilePanel("Save", "", "Level", "txt");
        g.SaveLevelToFile();
    }
}
