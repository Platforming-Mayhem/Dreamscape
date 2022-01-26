using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EnemyScript : MonoBehaviour
{
    private enum EnemyType { Ranged, CloseCombat};
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private Vector2 offset;
    [SerializeField] private float distanceToAttack = 1.0f;

    private Vector3 finalPosition;
    private PlayerScript playerScript;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = FindObjectOfType<PlayerScript>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void RangedEnemy()
    {

    }

    private void CloseCombatEnemy()
    {
        Vector2 direction = Vector2.zero;
        if((playerScript.transform.position - finalPosition).magnitude <= distanceToAttack)
        {
            direction = Vector2.Scale((playerScript.transform.position - transform.position).normalized, Vector2.right);
            rb.velocity = direction;
        }
        else
        {
            direction = Vector2.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyType == EnemyType.Ranged)
        {
            RangedEnemy();
        }
        else if(enemyType == EnemyType.CloseCombat)
        {
            CloseCombatEnemy();
        }
        finalPosition = transform.position + (Vector3)offset;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(finalPosition, distanceToAttack);
    }
}
