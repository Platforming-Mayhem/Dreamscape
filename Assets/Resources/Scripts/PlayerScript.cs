using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private Animator DeathScreen;
    public Transform spawnpoint;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float radius = 0.1f;
    [SerializeField] private float gravityScale = 1.0f;
    [SerializeField] private float maxHeight = 1.0f;
    private float height = 0.0f;

    int health = 100;

    [Header("Animation")]
    [SerializeField] private AnimationCurve accelerationAndDecelerationCurve;
    private Animator anim;

    private bool isGrounded;
    private bool isDropping;
    private bool previousGrounded;

    public bool accelerate;
    public bool decelerate;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        accelerationEnd = accelerationAndDecelerationCurve.keys[1].time;
        decelerationEnd = accelerationAndDecelerationCurve.keys[2].time;
        xTime = decelerationEnd;
        gameObject.SetActive(true);
        spawnpoint = GameObject.FindGameObjectWithTag("Spawnpoint").transform;
    }

    public float GetMaxHeight()
    {
        return maxHeight;
    }

    public void SetHeight(float newHeight)
    {
        height = newHeight;
    }

    //Curve Equation
    private float curveEquation(float maxHeight, float timeInterval)
    {
        float newHeight = -((timeInterval - maxHeight) * (timeInterval - maxHeight)) + (maxHeight * maxHeight);
        return newHeight;
    }

    public void Jump()
    {
        isJumping = true;
    }

    private bool hurt = false;

    private Vector2 hurtDirection = Vector2.right;

    public void HurtPlayer(int amount, Vector2 enemyPosition)
    {
        hurtDirection = Vector2.Scale((Vector2)transform.position - enemyPosition, Vector3.right).normalized * maxHeight * 2.0f;
        health -= amount;
        Jump();
        hurt = true;
        if (health <= 0)
        {
            Death();
        }
    }

    public void Respawn()
    {
        health = 100;
        direction = 1.0f;
        rb.velocity = Vector2.zero;
        transform.position = spawnpoint.position;
        gameObject.SetActive(true);
        DeathScreen.SetTrigger("close");
    }

    private void Death()
    {
        DeathScreen.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private bool isJumping;

    private float yTime = 0f;

    private float previousX = 0f;

    private void FixedUpdate()
    {
        isGrounded = Physics2D.Raycast(groundChecker.transform.position, Vector2.down, radius, groundMask);
        if(previousGrounded && !isGrounded)
        {
            coyoteTimer = (float)(9.0f/60.0f);
        }
        if (isGrounded && isJumping)
        {
            yTime = 0f;
        }
        else if (previousGrounded && !isGrounded || !isGrounded)
        {
            if (!isJumping && coyoteTimer <= 0.0f)
            {
                yTime = height * 2f;
                isJumping = true;
            }
        }
        if (isGrounded && !previousGrounded)
        {
            isJumping = false;
            anim.SetTrigger("Land");
            if(coyoteTimer <= 0.0f)
            {
                SetHeight(0.0f);
            }
            hurt = false;
        }
        if (isJumping)
        {
            yTime += Time.fixedDeltaTime * 9.8f * gravityScale;
            rb.velocity = new Vector2(rb.velocity.x, curveEquation(height, yTime));
        }
    }

    private void JumpUpdate()
    {
        if (Input.GetButton("Jump"))
        {
            if (Input.GetButtonDown("Jump"))
            {
                jumpStack = true;
                jumpTimer = 0.5f;
            }
            if (isGrounded && previousGrounded || coyoteTimer > 0.0f)
            {
                SetHeight(Mathf.Clamp(height + Time.deltaTime * 20.0f, 0.0f, maxHeight));
            }
        }
        if (jumpStack)
        {
            if (jumpTimer > 0.0f)
            {
                jumpTimer -= Time.deltaTime;
            }
            else
            {
                jumpStack = false;
            }
            if (!isJumping)
            {
                anim.SetTrigger("Jump");
                jumpStack = false;
            }
        }
    }

    private float xTime = 0f;
    private float direction = 1f;
    private float xVelocity;

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
        if(rb.velocity.x != 0.0f)
        {
            transform.right = (Vector3.right * rb.velocity.x).normalized;
        }
    }

    private float accelerationEnd = 0f;
    private float decelerationEnd = 0f;

    private void AccelerateAndDecelerate(float currentX)
    {
        if(currentX != 0f)
        {
            accelerate = true;
            if (previousX == 0f)
            {
                if(xTime > accelerationEnd)
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

    private float coyoteTimer = 0.0f;

    private void CoyoteTime()
    {
        if(coyoteTimer > 0.0f)
        {
            coyoteTimer -= Time.deltaTime;
        }
    }

    private bool jumpStack;
    private float jumpTimer = 0.0f;

    // Update is called once per frame
    private void Update()
    {
        CoyoteTime();
        JumpUpdate();
        ChangeDirection();
        AccelerateAndDecelerate(Input.GetAxis("Horizontal"));
    }

    private void LateUpdate()
    {
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x / 3f));
        previousX = Input.GetAxis("Horizontal");
        previousGrounded = isGrounded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundChecker.position, groundChecker.position + Vector3.down * radius);
    }
}
