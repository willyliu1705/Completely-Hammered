using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    AudioManager audioManager;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private Collider2D bc2D;
    [SerializeField] private Animator anim2D;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask hammerOnlyLayerMask;
    [SerializeField] private float raycastGroundLength;
    [SerializeField] private float raycastHammerLength;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float groundAcceleration;
    [SerializeField] private float airAcceleration;
    [SerializeField] private float dragCoefficient;
    [SerializeField] private float maxHorizontalSpeed;
    [SerializeField] private float strongThreshold;
    [SerializeField] private float weakHammerForce;
    [SerializeField] private float strongHammerForce;
    [SerializeField] private float hammerCooldown;
    [SerializeField] private float coyoteTimeBuffer;
    [SerializeField] private float aimBuffer;
    [SerializeField] private float timeToApplyDrag;

    [SerializeField] private KeyCode moveLeft;
    [SerializeField] private KeyCode moveRight;
    [SerializeField] private KeyCode swingLeft;
    [SerializeField] private KeyCode swingRight;
    [SerializeField] private KeyCode swingDown;
    [SerializeField] private KeyCode swingUp;

    private float moveAxis;
    private bool swing;
    private bool isCharging;
    private float chargeStartTime;
    private float previousSwingTime;
    private float chargeDuration;
    private float timeSinceLastSwing;
    private bool isGroundedFloor;
    private bool wasGroundedFloor;

    private float aimBufferTime;
    private Vector2 aimAxes;
    private Vector2 bufferedAimAxes;

    private bool isAlive;
    private bool inputActive;

    //the velocity at which the hard impact sound (groundHit, wallHit) plays
    [SerializeField] private float smackVelocity;

    private void Awake()
    {
        audioManager = FindFirstObjectByType<AudioManager>().GetComponent<AudioManager>();
        isAlive = true;
        inputActive = true;
        isCharging = false;
    }

    private void Update()
    {
        if (!isAlive || !inputActive)
        {
            return;
        }

        moveAxis = 0f;
        if (Input.GetKey(moveLeft))
        {
            moveAxis = -1f;
        }
        if (Input.GetKey(moveRight))
        {
            moveAxis = 1f;
        }

        if ((Input.GetKeyDown(swingLeft) || Input.GetKeyDown(swingRight) || Input.GetKeyDown(swingDown) || Input.GetKeyDown(swingUp)) && !isCharging)
        {
            aimAxes = Vector2.zero;
            isCharging = true;
            chargeStartTime = Time.time;
            audioManager.Play("swingCharge");
        }

        if ((Input.GetKey(swingLeft) || Input.GetKey(swingRight) || Input.GetKey(swingDown) || Input.GetKey(swingUp)) && isCharging)
        {
            if (Input.GetKeyDown(swingLeft)) aimAxes += Vector2.left;
            if (Input.GetKeyDown(swingRight)) aimAxes += Vector2.right;
            if (Input.GetKeyDown(swingDown)) aimAxes += Vector2.down;
            if (Input.GetKeyDown(swingUp)) aimAxes += Vector2.up;

            if (Input.GetKeyUp(swingLeft)) aimAxes -= Vector2.left;
            if (Input.GetKeyUp(swingRight)) aimAxes -= Vector2.right;
            if (Input.GetKeyUp(swingDown)) aimAxes -= Vector2.down;
            if (Input.GetKeyUp(swingUp)) aimAxes -= Vector2.up;

            if (Input.GetKey(swingLeft) && Input.GetKey(swingDown))
            {
                if (aimAxes.x * aimAxes.y != 0)
                {
                    bufferedAimAxes = aimAxes;
                }
                aimBufferTime = Time.time;
            }
            else if (Input.GetKey(swingRight) && Input.GetKey(swingDown))
            {
                if (aimAxes.x * aimAxes.y != 0)
                {
                    bufferedAimAxes = aimAxes;
                }
                aimBufferTime = Time.time;
            }
            else if (Input.GetKey(swingLeft) && Input.GetKey(swingRight))
            {
                if (aimAxes.x * aimAxes.y != 0)
                {
                    bufferedAimAxes = aimAxes;
                }
                aimBufferTime = Time.time;
            }
        }

        if (!Input.GetKey(swingLeft) && !Input.GetKey(swingRight) && !Input.GetKey(swingDown) && !Input.GetKey(swingUp) &&
            (Input.GetKeyUp(swingLeft) || Input.GetKeyUp(swingRight) || Input.GetKeyUp(swingDown) || Input.GetKeyUp(swingUp)))
        {
            isCharging = false;
            swing = true;
            audioManager.Stop("swingCharge");
        }
        anim2D.SetBool("isCharging", isCharging);

        chargeDuration = Time.time - chargeStartTime;
        timeSinceLastSwing = Time.time - previousSwingTime;

        isGroundedFloor = IsGrounded(-rb2D.transform.up);
        if (isGroundedFloor)
        {
            wasGroundedFloor = true;
        }
        else
        {
            StartCoroutine(SetWasGroundedAfterDelay(false));
        }

        if (swing && timeSinceLastSwing >= hammerCooldown)
        {
            if (Time.time - aimBufferTime <= aimBuffer)
            {
                aimAxes = bufferedAimAxes;
            }

            if (CanHammer(aimAxes) || wasGroundedFloor && aimAxes.y < 0)
            {
                Swing();
            }
            else
            {
                audioManager.Play("swingMiss");
            }
            aimAxes = Vector2.zero;
        }
        swing = false;

        LimitSpeed();
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(rb2D.position, aimAxes * bc2D.bounds.size);

        if (!isAlive || !inputActive)
        {
            return;
        }

        Move();

        if (timeSinceLastSwing >= timeToApplyDrag && isGroundedFloor)
        {
            ApplyDrag();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isAlive && collision.gameObject.layer == LayerMask.NameToLayer("Lethal"))
        {
            sprite.color = Color.grey;
            audioManager.Stop("swingCharge");
            audioManager.Play("death");
            rb2D.velocity = Vector2.zero;
            isAlive = false;

            GameManagerScript.Instance.StartFadeIn();

            StartCoroutine(ReloadSceneAfterDelay());
        }
        //code for hard impact sound
        if (collision.transform.position.y < transform.position.y && (rb2D.velocity.y > smackVelocity || rb2D.velocity.x > smackVelocity))
        {
            audioManager.Play("wallHit");
        }
    }

    private IEnumerator ReloadSceneAfterDelay()
    {
        if (moveAxis != 0)
        {
            // Set the localScale based on moveAxis direction
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Mathf.Sign(moveAxis), transform.localScale.y, transform.localScale.z);
            anim2D.SetBool("isIdle", false);
        }
        else
        {
            anim2D.SetBool("isIdle", true);
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator SetWasGroundedAfterDelay(bool grounded)
    {
        yield return new WaitForSeconds(coyoteTimeBuffer);
        wasGroundedFloor = grounded;
    }

    private void Move()
    {
        float acceleration = isGroundedFloor ? groundAcceleration : airAcceleration;
        if (walkSpeed - rb2D.velocity.x * moveAxis < 0) { acceleration = 0; }
        rb2D.AddForce(new Vector2(moveAxis, 0f) * acceleration);

        if (moveAxis != 0)
        {
            // Set the localScale based on moveAxis direction
            sprite.transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Mathf.Sign(moveAxis), transform.localScale.y, transform.localScale.z);
            anim2D.SetBool("isIdle", false);
        }
        else
        {
            anim2D.SetBool("isIdle", true);
        }
    }

    private void Swing()
    {
        previousSwingTime = Time.time;

        Vector2 swingAxes = aimAxes;
        if (swingAxes.x * swingAxes.y != 0)
        {
            swingAxes = (2 * Mathf.Sign(swingAxes.y) * Vector2.up + Mathf.Sign(swingAxes.x) * Vector2.right).normalized;
        }

        // Set apposing velocity to 0
        if (swingAxes.x * rb2D.velocity.x > 0)
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
        }
        if (swingAxes.y * rb2D.velocity.y > 0)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
        }

        // holding the bar for more than `strongThreshold` seconds leads to strong hammer force
        if (chargeDuration < strongThreshold)
        {
            rb2D.AddForce(-swingAxes * weakHammerForce, ForceMode2D.Impulse);
            audioManager.Play("swingWeak");
        }
        else
        {
            rb2D.AddForce(-swingAxes * strongHammerForce, ForceMode2D.Impulse);
            audioManager.Play("swingStrong");
        }

        anim2D.SetBool("shouldSwing", true);
    }

    public void SwingFinished()
    {
        anim2D.SetBool("shouldSwing", false);
    }

    private void ApplyDrag()
    {
        if (rb2D.velocity.x * moveAxis <= 0)
        {
            rb2D.AddForce(new Vector2(-rb2D.velocity.x * dragCoefficient, 0f));
        }
        else if (walkSpeed - rb2D.velocity.x * moveAxis < 0)
        {
            rb2D.AddForce(new Vector2(-rb2D.velocity.x * dragCoefficient / 5, 0f));
        }
    }

    private void LimitSpeed()
    {
        float maxSpeed = maxHorizontalSpeed;
        if (Mathf.Abs(rb2D.velocity.x) >= maxSpeed)
        {
            rb2D.velocity = new Vector2(Mathf.Sign(rb2D.velocity.x) * maxSpeed, rb2D.velocity.y);
        }
    }

    private bool IsGrounded(Vector3 direction)
    {
        return Physics2D.BoxCast(bc2D.bounds.center, bc2D.bounds.size, 0f, direction, raycastGroundLength, groundLayerMask);
    }

    private bool CanHammer(Vector3 direction)
    {
        RaycastHit2D collision = Physics2D.BoxCast(bc2D.bounds.center, bc2D.bounds.size, 0f, direction, raycastHammerLength, groundLayerMask | hammerOnlyLayerMask);
        if (collision)
        {
            GameObject hitObject = collision.collider.gameObject;
            if (hitObject.TryGetComponent(out IHammerable hammerableObject))
            {
                hammerableObject.OnHammer();
            }
        }
        return (collision);
    }

    public void DisablePlayerInput()
    {
        inputActive = false;
    }

    public void EnablePlayerInput()
    {
        inputActive = true;
    }

}
