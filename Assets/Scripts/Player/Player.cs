using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static Controls;

public class Player : MonoBehaviour, IPlayerActions
{
    private Controls controls;
    AudioManager audioManager;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private BoxCollider2D bc2D;
    [SerializeField] private Animator anim2D;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float raycastGroundLength;
    [SerializeField] private float raycastHammerLength;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float groundAcceleration;
    [SerializeField] private float airAcceleration;
    [SerializeField] private float dragCoefficient;
    [SerializeField] private float strongThreshold;
    [SerializeField] private float weakHammerForce;
    [SerializeField] private float strongHammerForce;
    [SerializeField] private float hammerCooldown;
    [SerializeField] private float coyoteBuffer;
    [SerializeField] private float aimBuffer;

    private float moveAxis;
    private bool swing;
    private bool isCharging;
    private float chargeStartTime;
    private float previousSwingTime;
    private float chargeDuration;
    private float timeSinceLastSwing;
    private float timeToApplyDrag = 0.2f;
    private float initialSwingSpeed;
    private bool isGroundedFloor;
    private bool wasGroundedFloor;
    private bool isAirborneAfterSwing;

    private float aimBufferTime;
    private Vector2 aimAxes;
    private Vector2 bufferedAimAxes;

    private bool isAlive;

    private void Awake()
    {
        audioManager = GameObject.FindFirstObjectByType<AudioManager>().GetComponent<AudioManager>();
        controls = new Controls();
        controls.Player.AddCallbacks(this);
        controls.Player.Enable();
        isAlive = true;
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(rb2D.position, aimAxes * bc2D.bounds.size);

        if (!isAlive)
        {
            return;
        }

        chargeDuration = Time.time - chargeStartTime;
        isGroundedFloor = IsGrounded(-rb2D.transform.up);
        if (isGroundedFloor)
        {
            wasGroundedFloor = true;
        }
        else
        {
            StartCoroutine(SetWasGroundedAfterDelay(false));
        }

        if (isGroundedFloor || IsGrounded(-rb2D.transform.right) || IsGrounded(rb2D.transform.right))
        {
            isAirborneAfterSwing = false;
        }

        Move();

        if (isCharging)
        {
            if (chargeDuration >= strongThreshold)
            {
                sprite.color = Color.cyan;
            }
        }

        if (swing && timeSinceLastSwing >= hammerCooldown)
        {
            if (Time.time - aimBufferTime <= aimBuffer)
            {
                aimAxes = bufferedAimAxes;
            }

            if (wasGroundedFloor && aimAxes.y < 0 || CanHammer(aimAxes))
            {
                Swing();
            }
            else
            {
                audioManager.Play("swingMiss");
            }
            aimAxes = Vector2.zero;
        }

        timeSinceLastSwing = Time.time - previousSwingTime;
        if (timeSinceLastSwing >= timeToApplyDrag && isGroundedFloor)
        {
            ApplyDrag();
        }

        LimitSpeed();
        swing = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Lethal"))
        {
            sprite.color = Color.grey;
            audioManager.Stop("swingCharge");
            audioManager.Play("death");
            rb2D.velocity = Vector2.zero;
            isAlive = false;
            StartCoroutine(ReloadSceneAfterDelay());
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
        yield return new WaitForSeconds(coyoteBuffer);
        wasGroundedFloor = grounded;
    }

    private void Move()
    {
        sprite.color = Color.white;
        float acceleration = isGroundedFloor ? groundAcceleration : airAcceleration;
        rb2D.AddForce(new Vector2(moveAxis, 0f) * acceleration);
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
            sprite.color = Color.yellow;
            initialSwingSpeed = rb2D.velocity.x - swingAxes.x * weakHammerForce / rb2D.mass;
            rb2D.AddForce(-swingAxes * weakHammerForce, ForceMode2D.Impulse);
            audioManager.Play("swingWeak");
        }
        else
        {
            sprite.color = Color.red;
            initialSwingSpeed = rb2D.velocity.x - swingAxes.x * strongHammerForce / rb2D.mass;
            rb2D.AddForce(-swingAxes * strongHammerForce, ForceMode2D.Impulse);
            audioManager.Play("swingStrong");
        }

        isAirborneAfterSwing = true;
    }

    private void ApplyDrag()
    {
        if (moveAxis == 0f || rb2D.velocity.x * moveAxis < 0)
        {
            rb2D.AddForce(new Vector2(-rb2D.velocity.x * dragCoefficient, 0f));
        }
    }

    private void LimitSpeed()
    {
        if (isAirborneAfterSwing || timeSinceLastSwing <= timeToApplyDrag && isGroundedFloor)
        {
            float maxSwingSpeed = Mathf.Max(Mathf.Abs(initialSwingSpeed), walkSpeed);
            if (Mathf.Abs(rb2D.velocity.x) >= maxSwingSpeed)
            {
                rb2D.velocity = new Vector2(Mathf.Sign(rb2D.velocity.x) * maxSwingSpeed, rb2D.velocity.y);
            }
        }
        else if (Mathf.Abs(rb2D.velocity.x) >= walkSpeed)
        {
            rb2D.velocity = new Vector2(Mathf.Sign(rb2D.velocity.x) * walkSpeed, rb2D.velocity.y);
        }
    }

    private bool IsGrounded(Vector3 direction)
    {
        return Physics2D.BoxCast(bc2D.bounds.center, bc2D.bounds.size, 0f, direction, raycastGroundLength, groundLayerMask);
    }

    private bool CanHammer(Vector3 direction)
    {
        return Physics2D.BoxCast(bc2D.bounds.center, bc2D.bounds.size, 0f, direction, raycastHammerLength, groundLayerMask);
    }

    public void OnMove(InputAction.CallbackContext context)
    {

        if (!isAlive) {
            return;
        }

        moveAxis = context.ReadValue<float>();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if(!isAlive) {
            return;
        }

        if (context.started)
        {
            isCharging = true;
            chargeStartTime = Time.time;
            audioManager.Play("swingCharge");
        }
        else if (context.performed)
        {
            aimAxes = context.ReadValue<Vector2>();
            if (aimAxes.x * aimAxes.y != 0)
            {
                bufferedAimAxes = aimAxes;
            }
            aimBufferTime = Time.time;
        }
        else if (context.canceled)
        {
            isCharging = false;
            swing = true;
            audioManager.Stop("swingCharge");
        }
    }

}
