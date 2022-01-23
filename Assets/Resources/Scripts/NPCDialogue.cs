using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SFB;

public class NPCDialogue : MonoBehaviour
{
    public string text;

    private int pageNumber = 1;

    [SerializeField] private TMP_Text tmpText;

    [SerializeField] private int previousPageNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        ReadFromFile();
    }

    private bool IsOverFlowing()
    {
        return tmpText.GetPreferredValues().y > 53.7f;
    }

    void ReadFromFile()
    {
        string destination = StandaloneFileBrowser.OpenFilePanel("Open", "", "txt", false)[0];
        string text = System.IO.File.ReadAllText(@destination);
        string[] splitText = text.Split(':', '\n');
        for(int i = 0; i < splitText.Length; i++)
        {
            if(i % 2 == 0)
            {
                Debug.Log("Name: " + splitText[i]);
                foreach(GameObject gameObject in FindObjectsOfType<GameObject>())
                {
                    if(gameObject.name == splitText[i])
                    {
                        Debug.Log("Found!");
                    }
                }
            }
            else
            {
                Debug.Log("Text: " + splitText[i]);
            }
        }
    }

    IEnumerator TextAppear()
    {
        tmpText.text = "";
        foreach (char a in text)
        {
            previousPageNumber = tmpText.textInfo.pageCount;
            if (Input.GetButton("Submit"))
            {
                tmpText.text += a;
                yield return new WaitForSeconds(0.01f);
            }
            else
            {
                tmpText.text += a;
                yield return new WaitForSeconds(0.1f);
            }
            if (tmpText.textInfo.pageCount - previousPageNumber > 0)
            {
                pageNumber++;
            }
            else if(tmpText.textInfo.pageCount == 1)
            {
                pageNumber = 1;
            }
            tmpText.pageToDisplay = pageNumber;
        }
    }

    private void OnEnable()
    {
        StartCoroutine("TextAppear");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            tmpText.text = "";
            tmpText.pageToDisplay = 1;
            pageNumber = 1;
            gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        
    }
}
