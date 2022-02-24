using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private ParticleSystem dustKickUp;
    [SerializeField] private AudioClip step;
    [SerializeField] private Animator deathScreen;
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
    [SerializeField] private float animationSpeed = 0.25f;
    private Animator anim;

    private bool isGrounded;
    private bool isDropping;
    private bool previousGrounded;

    public bool accelerate;
    public bool decelerate;

    private Rigidbody2D rb;
    private CapsuleCollider2D capsule;
    private SpriteRenderer sprite;
    private RaRaScript raRa;
    private AudioSource audioSource;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        capsule = GetComponent<CapsuleCollider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        raRa = FindObjectOfType<RaRaScript>();
        accelerationEnd = accelerationAndDecelerationCurve.keys[1].time;
        decelerationEnd = accelerationAndDecelerationCurve.keys[2].time;
        xTime = decelerationEnd;
        gameObject.SetActive(true);
        audioSource = GetComponent<AudioSource>();
        ySize = capsule.size.y;
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

    public void PlayFootstep()
    {
        if (isGrounded)
        {
            audioSource.pitch = Random.Range(0.6f, 1.5f);
            audioSource.PlayOneShot(step);
        }
    }

    public void PlayDust()
    {
        if (isGrounded)
        {
            dustKickUp.Emit(5);
        }
    }

    public void Respawn()
    {
        health = 100;
        direction = 0.0f;
        hurtDirection = Vector2.zero;
        rb.velocity = Vector2.zero;
        try
        {
            spawnpoint = GameObject.FindGameObjectWithTag("Initial").transform;
        }
        catch
        {
            spawnpoint = GameObject.FindGameObjectWithTag("Spawnpoint").transform;
        }
        transform.position = spawnpoint.position;
        gameObject.SetActive(true);
        deathScreen.SetTrigger("close");
    }

    private void Death()
    {
        deathScreen.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private bool isJumping;

    private bool bounced = false;

    private float yTime = 0f;

    private float previousX = 0f;

    public void SetBounced(bool tf)
    {
        bounced = tf;
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundChecker.transform.position, radius, groundMask);
        if(previousGrounded && !isGrounded)
        {
            coyoteTimer = (float)(9.0f/60.0f);
        }
        if ((isGrounded && isJumping) || bounced && jumpStack)
        {
            yTime = 0f;
        }
        else if (previousGrounded && !isGrounded || !isGrounded)
        {
            if (!isJumping && coyoteTimer <= 0.0f && !bounced)
            {
                yTime = height * 2f;
                isJumping = true;
            }
        }
        if (isGrounded && !previousGrounded)
        {
            isJumping = false;
            if(coyoteTimer <= 0.0f || !bounced)
            {
                SetHeight(0.0f);
            }
            hurt = false;
            anim.SetTrigger("Land");
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
            SetHeight(maxHeight);
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
            if (!isJumping || bounced)
            {
                anim.SetTrigger("Jump");
                jumpStack = false;
            }
        }
        anim.SetBool("Crouch", Input.GetButton("Crouch"));
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
        if(rb.velocity.x > 0.0f)
        {
            transform.localScale = Vector3.one;
        }
        else if(rb.velocity.x < 0.0f)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
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
                xTime = accelerationEnd;
                decelerate = true;
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
        rb.velocity = transform.rotation * new Vector2(xVelocity, rb.velocity.y);
    }

    private float coyoteTimer = 0.0f;

    private void CoyoteTime()
    {
        if(coyoteTimer > 0.0f)
        {
            coyoteTimer -= Time.deltaTime;
        }
    }

    private float ySize;

    private void Crouch()
    {
        if (anim.GetBool("Crouch"))
        {
            horizontal = Mathf.Clamp(horizontal, 0.0f, 0.0f);
            capsule.size = (Vector2.up * ySize / 2.0f) + (Vector2.right * capsule.size.x);
            capsule.offset = new Vector2(0.1239169f, -0.17f);
        }
        else
        {
            capsule.size = (Vector2.up * ySize) + (Vector2.right * capsule.size.x);
            capsule.offset = new Vector2(0.1239169f, 0.2840676f);
        }
    }

    private bool jumpStack;
    private float jumpTimer = 0.0f;
    private float horizontal;

    // Update is called once per frame
    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        CoyoteTime();
        JumpUpdate();
        Crouch();
        AccelerateAndDecelerate(horizontal);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Death();
        }
        if (Input.GetButtonDown("Fire1"))
        {
            if (raRa.activate)
            {
                raRa.activate = false;
            }
            else
            {
                raRa.activate = true;
            }
        }
        /*if(isGrounded || previousGrounded)
        {
            sprite.transform.up = Vector3.Lerp(sprite.transform.up, Physics2D.Raycast(groundChecker.transform.position, Vector2.down, radius, groundMask).normal, Time.deltaTime * 10.0f);
        }
        else
        {
            sprite.transform.up = Vector3.Lerp(sprite.transform.up, Vector3.up, Time.deltaTime * 20.0f);
        }*/
        ChangeDirection();
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x * animationSpeed));
        previousX = horizontal;
        previousGrounded = isGrounded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundChecker.position, radius);
        Gizmos.DrawRay(groundChecker.transform.position, Vector2.down * radius);
    }
}
