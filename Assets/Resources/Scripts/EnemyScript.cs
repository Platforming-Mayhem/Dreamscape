using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EnemyScript : MonoBehaviour
{
    private enum EnemyType { Ranged, CloseCombat};
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private float radius = 0.1f;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private LayerMask groundMask;

    [Header("Animation")]
    [SerializeField] private AnimationCurve accelerationAndDecelerationCurve;

    private float accelerationTime = 0.0f;
    private bool accelerate;
    private bool decelerate;
    private bool isGrounded;
    private SpriteRenderer sprite;
    private PlayerScript playerScript;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = FindObjectOfType<PlayerScript>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        accelerationEnd = accelerationAndDecelerationCurve.keys[1].time;
        decelerationEnd = accelerationAndDecelerationCurve.keys[2].time;
        xTime = decelerationEnd;
        Vector2 direction = (playerScript.transform.position - transform.position);
    }

    private void RangedEnemy()
    {

    }

    private float curveEquation(float maxHeight, float timeInterval)
    {
        float newHeight = ((-Mathf.Pow(((timeInterval - maxHeight)), 2.0f) + Mathf.Pow(maxHeight, 2.0f)) * Mathf.Pow(-timeInterval + 2.0f * maxHeight, 2.0f)) / 1.688f;
        return newHeight;
    }

    private float previousX = 0f;
    private float xTime = 0f;
    private float direction = 1f;
    private float xVelocity;
    private bool hurt = false;
    private Vector2 hurtDirection = Vector2.right;

    private void ChangeDirection()
    {
        if (previousX > 0f)
        {
            if (direction == -1f)
            {
                xTime = 0f;
            }
            direction = 1f;
        }
        else if (previousX < 0f)
        {
            if (direction == 1f)
            {
                xTime = 0f;
            }
            direction = -1f;
        }
        if (rb.velocity.x > 0.0f)
        {
            sprite.flipX = false;
        }
        else if (rb.velocity.x < 0.0f)
        {
            sprite.flipX = true;
        }
    }

    private float accelerationEnd = 0f;
    private float decelerationEnd = 0f;

    private void AccelerateAndDecelerate(float currentX)
    {
        if (currentX != 0f)
        {
            accelerate = true;
            if (previousX == 0f)
            {
                if (xTime > accelerationEnd)
                {
                    xTime = (decelerationEnd - xTime);
                }
                else
                {
                    xTime = (accelerationEnd - xTime);
                }
                decelerate = false;
            }
        }
        else
        {
            accelerate = false;
            if (previousX != 0f)
            {
                decelerate = true;
                xTime = accelerationEnd;
            }
        }
        if (accelerate)
        {
            xTime += Time.deltaTime;
            xTime = Mathf.Clamp(xTime, 0f, accelerationEnd);
            //anim.SetBool("Walking", true);
        }
        else if (decelerate)
        {
            xTime += Time.deltaTime;
            xTime = Mathf.Clamp(xTime, accelerationEnd, decelerationEnd);
            //anim.SetBool("Walking", false);
        }
        xVelocity = accelerationAndDecelerationCurve.Evaluate(xTime) * direction;
        if (hurt)
        {
            xVelocity = hurtDirection.x;
        }
        rb.velocity = new Vector2(xVelocity, rb.velocity.y);
    }

    private void CloseCombatEnemy()
    {
        
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundChecker.transform.position, radius, groundMask);
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
            AccelerateAndDecelerate((playerScript.transform.position - transform.position).x);
        }
        ChangeDirection();
    }
    private void LateUpdate()
    {
        previousX = (playerScript.transform.position - transform.position).x;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundChecker.position, radius);
        Gizmos.DrawRay(groundChecker.transform.position, Vector2.down * radius);
    }
}
