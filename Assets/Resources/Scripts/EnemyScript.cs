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
    private bool previousGrounded;
    private bool L;
    private bool R;
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

    private bool jump;
    private bool isJumping;
    private float yTime = 0.0f;
    private float height = 1.6f;

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundChecker.transform.position, radius, groundMask);
        L = Physics2D.Raycast(groundChecker.transform.position, (Vector2.down + Vector2.left) * 0.5f, 1.0f, groundMask);
        R = Physics2D.Raycast(groundChecker.transform.position, (Vector2.down + Vector2.right) * 0.5f, 1.0f, groundMask);
        if (jump && isGrounded)
        {
            isJumping = true;
            yTime = 0.0f;
            jump = false;
        }
        if(!isGrounded && previousGrounded && !isJumping)
        {
            yTime = 0.0f;
        }
        if (!isGrounded && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, curveEquation(0.0f, yTime));
            yTime += Time.fixedDeltaTime * 9.8f;
        }
        if(isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, curveEquation(height, yTime));
            yTime += Time.fixedDeltaTime * 9.8f;
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
            if(L && R)
            {
                AccelerateAndDecelerate((playerScript.transform.position - transform.position).x);
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
        ChangeDirection();
    }
    private void LateUpdate()
    {
        previousX = (playerScript.transform.position - transform.position).x;
        previousGrounded = isGrounded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundChecker.position, radius);
        Gizmos.DrawRay(groundChecker.transform.position, Vector2.down * radius);
        Gizmos.DrawRay(groundChecker.transform.position, (Vector2.down + Vector2.left) * 0.5f);
        Gizmos.DrawRay(groundChecker.transform.position, (Vector2.down + Vector2.right) * 0.5f);
    }
}
