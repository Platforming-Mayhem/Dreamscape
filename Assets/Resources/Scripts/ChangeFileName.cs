using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
}
