using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Transform groundChecker;
    public LayerMask groundMask;
    public float radius = 0.1f;
    public float gravityScale = 1f;
    public float maxHeight = 1f;
    [Header("Animation")]
    public AnimationCurve accelerationAndDecelerationCurve;
    private Animator anim;

    private bool isGrounded;
    private bool isDropping;
    public bool accelerate;
    public bool decelerate;
    private bool previousGrounded;

    private Rigidbody rb;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        Collider[] colliders = FindObjectsOfType<Collider>();
        PhysicMaterial mat = GetComponent<Collider>().material;
        foreach (Collider c in colliders)
        {
            c.material = mat;
        }
        accelerationEnd = accelerationAndDecelerationCurve.keys[1].time;
        decelerationEnd = accelerationAndDecelerationCurve.keys[2].time;
        xTime = decelerationEnd;
    }

    //Curve Equation
    public float curveEquation(float maxHeight, float timeInterval)
    {
        float newHeight = -((timeInterval - maxHeight) * (timeInterval - maxHeight)) + (maxHeight * maxHeight);
        return newHeight;
    }

    public bool isJumping;

    private float yTime = 0f;

    private float previousX = 0f;

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, radius, groundMask);
        if (isGrounded && isJumping)
        {
            yTime = 0f;
        }
        else if (previousGrounded && !isGrounded && !isJumping)
        {
            yTime = maxHeight * 2f;
            isJumping = true;
        }
        if (isGrounded && !previousGrounded)
        {
            isJumping = false;
            anim.SetTrigger("Land");
        }
        if (isJumping)
        {
            yTime += Time.fixedDeltaTime * 9.8f * gravityScale;
            rb.velocity = (Vector3)new Vector2(rb.velocity.x, curveEquation(maxHeight, yTime));
        }
    }

    private float xTime = 0f;
    private float direction = 1f;
    private float xVelocity;

    private float accelerationEnd = 0f;
    private float decelerationEnd = 0f;

    void AccelerateAndDecelerate(float currentX)
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
        rb.velocity = (Vector3)new Vector2(xVelocity, rb.velocity.y);
    }

    private bool jumpStack;
    private float jumpTimer = 0.0f;

    // Update is called once per frame
    private void Update()
    {
        if (jumpStack && !isJumping)
        {
            anim.SetTrigger("Jump");
            jumpStack = false;
        }
        if(Input.GetButtonDown("Jump"))
        {
            jumpStack = true;
            jumpTimer = 0.5f;
        }
        if (jumpStack)
        {
            if(jumpTimer > 0.0f)
            {
                jumpTimer -= Time.deltaTime;
            }
            else
            {
                jumpStack = false;
            }
        }
        if (previousX > 0f)
        {
            if(direction == -1f)
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
        AccelerateAndDecelerate(Input.GetAxis("Horizontal"));
        previousX = Input.GetAxis("Horizontal");
        transform.right = (Vector3.right * rb.velocity.x).normalized;
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x / 3f));
    }

    private void LateUpdate()
    {
        previousGrounded = isGrounded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundChecker.position, radius);
    }
}
