using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaRaScript : MonoBehaviour
{
    public bool activate;

    private bool isEnabled = false;

    PlayerScript player;

    [SerializeField] private float radius = 0.1f;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private LayerMask groundMask;

    private bool accelerate;
    private bool decelerate;
    [Header("Animation")]
    [SerializeField] private AnimationCurve accelerationAndDecelerationCurve;

    private bool isGrounded;
    private bool previousGrounded;
    private bool seePlayer;

    private SpriteRenderer sprite;
    private PlayerScript playerScript;
    private Rigidbody2D rb;
    private Animator anim;

    public void activateRaRa()
    {
        isEnabled = true;
        player = FindObjectOfType<PlayerScript>();
    }

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
    }

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

    private float curveEquation(float maxHeight, float timeInterval)
    {
        float newHeight = ((-Mathf.Pow(timeInterval - maxHeight, 2.0f) + Mathf.Pow(maxHeight, 2f)) * (timeInterval + maxHeight)) * (Mathf.Pow(timeInterval - (maxHeight * 2.0f), 2.0f));
        float newNewHeight = -Mathf.Pow(timeInterval - (maxHeight * 2.0f), 2.0f) * 2.0f;
        if (newHeight <= 0.0f)
        {
            return newNewHeight;
        }
        else
        {
            return newHeight;
        }
    }

    private float previousX = 0f;
    private float xTime = 0f;
    private float direction = 1f;
    private float xVelocity;
    private bool hurt = false;
    private Vector2 hurtDirection = Vector2.right;
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

    [SerializeField] private bool jump;
    private bool isJumping;
    private float yTime = 0.0f;
    [SerializeField] private float height = 1.6f;

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundChecker.transform.position, radius, groundMask);
        bool LD = Physics2D.Raycast(groundChecker.position, Vector2.down + Vector2.left, 1.0f, groundMask);
        bool RD = Physics2D.Raycast(groundChecker.position, Vector2.down + Vector2.right, 1.0f, groundMask);
        bool L = Physics2D.Raycast(groundChecker.position, Vector2.left, 1.0f, groundMask);
        bool R = Physics2D.Raycast(groundChecker.position, Vector2.right, 1.0f, groundMask);
        if (!LD || !RD)
        {
            jump = true;
        }
        else if (LD && RD)
        {
            jump = false;
        }
        if (L || R)
        {
            jump = true;
        }
        else if (!L && !R)
        {
            jump = false;
        }
        if (jump && isGrounded)
        {
            isJumping = true;
            yTime = 0.0f;
            jump = false;
        }
        if (!isGrounded && previousGrounded && !isJumping)
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
            yTime += Time.fixedDeltaTime * 9.8f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnabled)
        {
            AccelerateAndDecelerate((playerScript.transform.position - transform.position).normalized.x);
        }
        if (activate)
        {
            activateRaRa();
        }
        else
        {
            isEnabled = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
        ChangeDirection();
    }

    private void LateUpdate()
    {
        previousX = (playerScript.transform.position - transform.position).normalized.x;
        previousGrounded = isGrounded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundChecker.position, radius);
        Gizmos.DrawRay(groundChecker.transform.position, Vector2.down * radius);
        Gizmos.DrawRay(groundChecker.position, Vector2.down + Vector2.left);
        Gizmos.DrawRay(groundChecker.position, Vector2.down + Vector2.right);
        Gizmos.DrawRay(groundChecker.position, Vector2.left);
        Gizmos.DrawRay(groundChecker.position, Vector2.right);
    }
}
