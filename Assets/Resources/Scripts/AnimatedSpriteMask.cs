using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedSpriteMask : MonoBehaviour
{
    SpriteRenderer s_Renderer;
    SpriteMask mask;
    // Start is called before the first frame update
    void Start()
    {
        s_Renderer = GetComponent<SpriteRenderer>();
        mask = GetComponent<SpriteMask>();
    }

    // Update is called once per frame
    void Update()
    {
        mask.sprite = s_Renderer.sprite;
    }
}
