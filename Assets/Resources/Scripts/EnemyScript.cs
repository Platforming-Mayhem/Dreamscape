using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EnemyScript : MonoBehaviour
{
    private enum EnemyType { Ranged, CloseCombat};
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private float radius = 0.1f;
    [SerializeField] private float moveRange = 1.0f;
    [SerializeField] private float attackRange = 1.0f;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask playerMask;

    [Header("Animation")]
    [SerializeField] private AnimationCurve accelerationAndDecelerationCurve;

    private float accelerationTime = 0.0f;

    private bool attack;
    private bool previousAttack;
    public bool accelerate;
    public bool decelerate;
    private bool isGrounded;
    private bool previousGrounded;
    private bool seePlayer;
    private bool L;
    private bool R;

    private SpriteRenderer sprite;
    private PlayerScript playerScript;
    private Rigidbody2D rb;
    private Animator anim;

    public Vector2 lockPoint;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = FindObjectOfType<PlayerScript>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        accelerationEnd = accelerationAndDecelerationCurve.keys[1].time;
        decelerationEnd = accelerationAndDecelerationCurve.keys[2].time;
        xTime = decelerationEnd;
        Vector2 direction = (playerScript.transform.position - transform.position);
        lockPoint = Vector2.zero;
    }

    private void RangedEnemy()
    {

    }

    private void Attack(int hurtAmount)
    {
        if(Physics2D.OverlapCircle(transform.position, attackRange, playerMask))
        {
            playerScript.SetHeight(playerScript.GetMaxHeight());
            playerScript.HurtPlayer(hurtAmount, transform.position);
        }
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
            anim.SetBool("Walking", true);
        }
        else if (decelerate)
        {
            xTime += Time.deltaTime;
            xTime = Mathf.Clamp(xTime, accelerationEnd, decelerationEnd);
            anim.SetBool("Walking", false);
        }
        xVelocity = accelerationAndDecelerationCurve.Evaluate(xTime) * direction;
        if (hurt)
        {
            xVelocity = hurtDirection.x;
        }
        rb.velocity = new Vector2(xVelocity, rb.velocity.y);
    }

    private bool followPlayer = false;

    private void CloseCombatEnemy()
    {
        if (seePlayer)
        {
            followPlayer = true;
        }
        if (followPlayer)
        {
            RaycastHit2D a = Physics2D.Raycast(transform.position, playerScript.transform.position - transform.position, (playerScript.transform.position - transform.position).magnitude, groundMask);
            if (!a && playerScript.gameObject.activeInHierarchy)
            {
                lockPoint = (playerScript.transform.position - transform.position).normalized;
            }
            else if(a || !playerScript.gameObject.activeInHierarchy)
            {
                lockPoint = Vector2.zero;
                followPlayer = false;
            }
        }
        if (attack && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "BloodBoneAttack")
        {
            lockPoint = Vector2.zero;
            followPlayer = false;
            anim.SetTrigger("Attack");
        }
        AccelerateAndDecelerate(lockPoint.x);
    }

    private bool jump;
    private bool isJumping;
    private float yTime = 0.0f;
    [SerializeField] private float height = 1.6f;

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundChecker.transform.position, radius, groundMask);
        L = Physics2D.Raycast(groundChecker.position, Vector2.down + Vector2.left, 1.0f, groundMask);
        R = Physics2D.Raycast(groundChecker.position, Vector2.down + Vector2.right, 1.0f, groundMask);
        seePlayer = Physics2D.OverlapCircle(transform.position, moveRange, playerMask);
        attack = Physics2D.OverlapCircle(transform.position, attackRange, playerMask);
        if (!L && R || !R && L)
        {
            jump = true;
        }
        else if (L && R)
        {
            jump = false;
        }
        if (isGrounded && previousGrounded && !jump)
        {
            isJumping = false;
        }
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
        if (isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, curveEquation(height, yTime));
            yTime += Time.fixedDeltaTime * 9.8f;
        }
        else if (!isGrounded && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, curveEquation(0.0f, yTime));
            Debug.Log("FALL");
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
        }
        ChangeDirection();
    }
    private void LateUpdate()
    {
        previousX = lockPoint.x;
        previousGrounded = isGrounded;
        previousAttack = attack;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundChecker.position, radius);
        Gizmos.DrawRay(groundChecker.transform.position, Vector2.down * radius);
        Gizmos.DrawRay(transform.position, playerScript.transform.position - transform.position);
        Gizmos.DrawWireSphere(transform.position, moveRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
