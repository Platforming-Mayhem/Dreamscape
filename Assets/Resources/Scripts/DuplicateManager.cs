using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicateManager : MonoBehaviour
{
    Transform[] transforms;
    // Start is called before the first frame update
    void Start()
    {
        transforms = FindObjectsOfType<Transform>();
    }

    public void RemoveDuplicates()
    {
        foreach(Transform transform in transforms)
        {
            foreach(Transform nextTransform in transforms)
            {
                if(nextTransform.position == transform.position)
                {
                    Destroy(nextTransform.gameObject);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
