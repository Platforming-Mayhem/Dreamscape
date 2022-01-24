using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    public string text;

    private int pageNumber = 1;

    [SerializeField] private TMP_Text tmpText;

    private Animator anim;

    private int previousPageNumber = 1;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
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
            previousPageNumber = tmpText.textInfo.pageCount;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBecameInvisible()
    {
        tmpText.text = "";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine("TextAppear");
            anim.SetTrigger("Open");
            anim.ResetTrigger("Close");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopCoroutine("TextAppear");
            anim.SetTrigger("Close");
            anim.ResetTrigger("Open");
        }
    }
}
