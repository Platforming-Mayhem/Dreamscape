using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaRaScript : MonoBehaviour
{
    public bool activate;

    private bool isEnabled = false;

    PlayerScript player;

    private bool accelerate;
    private bool decelerate;
    [Header("Animation")]
    [SerializeField] private AnimationCurve accelerationAndDecelerationCurve;

    private bool isGrounded;
    private bool previousGrounded;
    private bool seePlayer;
    private bool L;
    private bool R;

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

    // Update is called once per frame
    void Update()
    {
        if (isEnabled)
        {
            AccelerateAndDecelerate((playerScript.transform.position - transform.position).normalized.x);
            Debug.Log((playerScript.transform.position - transform.position).normalized.x);
        }
        if (activate)
        {
            activateRaRa();
        }
        ChangeDirection();
    }

    private void LateUpdate()
    {
        previousX = (playerScript.transform.position - transform.position).normalized.x;
    }
}
