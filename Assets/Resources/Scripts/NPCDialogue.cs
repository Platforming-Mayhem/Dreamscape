using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    public string text;

    private int pageNumber = 1;

    [SerializeField] private TMP_Text tmpText;

    [SerializeField] private int previousPageNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private bool IsOverFlowing()
    {
        return tmpText.GetPreferredValues().y > 53.7f;
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
        
    }

    private void LateUpdate()
    {
        
    }
}
