using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingScript : MonoBehaviour
{
    private Camera cam;
    private SpriteRenderer s_Renderer;
    private Parallax current;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        s_Renderer = GetComponent<SpriteRenderer>();
        current = GetComponent<Parallax>();
    }

    // Update is called once per frame
    void Update()
    {
        if(s_Renderer.bounds.max.x < cam.ScreenToWorldPoint(Vector3.right * cam.pixelWidth).x)
        {
            Parallax instance = Instantiate(gameObject).GetComponent<Parallax>();
            instance.offset = current.offset + (s_Renderer.bounds.extents * 4.0f);
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
