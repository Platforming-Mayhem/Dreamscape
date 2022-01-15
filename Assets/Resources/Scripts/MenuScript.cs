using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private PlayerScript player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetActive(int type)
    {
        if(type == 0)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void ReloadLevel()
    {
        player.Respawn();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
