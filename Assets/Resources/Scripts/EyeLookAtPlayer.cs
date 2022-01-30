using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeLookAtPlayer : MonoBehaviour
{
    PlayerScript player;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private Bounds clampRange;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerScript>();
        clampRange.center = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 target = new Vector2(Mathf.Clamp(player.transform.position.x, clampRange.min.x, clampRange.max.x), Mathf.Clamp(player.transform.position.y, clampRange.min.y, clampRange.max.y));
        transform.position = Vector2.Lerp(transform.position, target, Time.deltaTime * speed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(clampRange.min, 0.1f);
        Gizmos.DrawSphere(clampRange.max, 0.1f);
    }
}
