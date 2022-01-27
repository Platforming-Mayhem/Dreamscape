using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexScript : MonoBehaviour
{
    [SerializeField] private Rigidbody[] rbs;
    [SerializeField] private float swirlStrength = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Rigidbody rb in rbs)
        {
            rb.AddForce(rb.transform.position - transform.position, ForceMode.Impulse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Rigidbody rb in rbs)
        {
            rb.velocity = Vector2.Perpendicular(rb.transform.position.normalized) * swirlStrength;
            rb.AddForce(Vector2.Perpendicular(rb.transform.position.normalized) * 100.0f * Time.deltaTime, ForceMode.Impulse);
        }
        swirlStrength += Time.deltaTime;
        swirlStrength = Mathf.Clamp(swirlStrength, 0.0f, 50.0f);
    }
}
