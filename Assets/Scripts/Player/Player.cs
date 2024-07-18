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
    [SerializeField] private float raycastBuffer;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float dragCoefficient;
    [SerializeField] private float strongThreshold;
    [SerializeField] private float weakHammerForce;
    [SerializeField] private float strongHammerForce;
    [SerializeField] private float aimBuffer;
    [SerializeField] private GameObject pauseMenu;

    private float moveAxis;
    private bool swingIsHeld;
    private bool swingJustReleased;
    private bool isAirborneAfterSwing;
    private float swingStartTime;
    private float swingDuration;
    private float swingReleaseTime;
    private float postSwingDuration;
    private float timeToApplyDrag = 0.2f;
    private bool isAlive;
    private bool isMenuActive;

    private float initialSwingSpeed;
    private float aimBufferTime;
    private Vector2 aimAxes;
    private Vector2 aimAxesBuffer;

    int counter;

    private void Awake()
    {
        audioManager = GameObject.FindFirstObjectByType<AudioManager>().GetComponent<AudioManager>();
        controls = new Controls();
        controls.Player.AddCallbacks(this);
        controls.Player.Enable();
        isAlive = true;
        isMenuActive = false;
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(rb2D.position, aimAxes * bc2D.bounds.size);

        if (!isAlive)
        {
            return;
        }
        Move();

        swingDuration = Time.time - swingStartTime;
        postSwingDuration = Time.time - swingReleaseTime;

        if (swingIsHeld)
        {
            if (swingDuration >= strongThreshold)
            {
                sprite.color = Color.cyan;
            }
        }

        if (IsGrounded(-rb2D.transform.up) || IsGrounded(-rb2D.transform.right) || IsGrounded(rb2D.transform.right))
        {
            isAirborneAfterSwing = false;
        }

        if (postSwingDuration <= timeToApplyDrag)
        {
            if (Time.time - aimBufferTime <= aimBuffer)
            {
                aimAxes = aimAxesBuffer;
            }

            if (swingJustReleased)
            {
                if (IsGrounded(aimAxes))
                {
                    Swing();
                }
                else
                {
                    audioManager.Play("swingMiss");
                }
            }

            aimAxes = Vector2.zero;
        }
        else if (IsGrounded(-rb2D.transform.up))
        {
            ApplyDrag();
        }

        LimitSpeed();
        swingJustReleased = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Lethal"))
        {
            sprite.color = Color.grey;
            audioManager.Stop("swingCharge");
            rb2D.velocity = Vector2.zero;
            isAlive = false;
            StartCoroutine(ReloadSceneAfterDelay());
        }
    }

    private IEnumerator ReloadSceneAfterDelay()
    {
        sprite.color = Color.white;
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
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Move()
    {
        rb2D.AddForce(new Vector2(moveAxis, 0f) * acceleration);
        sprite.color = Color.white;
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
        Vector2 swingAxes = aimAxes;
        if (swingAxes.x * swingAxes.y != 0)
        {
            swingAxes = (2 * Mathf.Sign(swingAxes.y) * Vector2.up + Mathf.Sign(swingAxes.x) * Vector2.right).normalized;
        }

        if (swingDuration < strongThreshold)
        {
            sprite.color = Color.yellow;
            initialSwingSpeed = rb2D.velocity.x - swingAxes.x * weakHammerForce / rb2D.mass;
            rb2D.AddForce(-swingAxes * weakHammerForce, ForceMode2D.Impulse);
            audioManager.Play("swingWeak");
        }
        // holding the bar for more than `strongThreshold` seconds leads to strong hammer force
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
        if (isAirborneAfterSwing || postSwingDuration <= timeToApplyDrag)
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
        return Physics2D.BoxCast(bc2D.bounds.center, bc2D.bounds.size, 0f, direction, raycastBuffer, groundLayerMask);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (isAlive && !isMenuActive)
        {
            moveAxis = context.ReadValue<float>();
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {

        if (isAlive && !isMenuActive)
        {
            if (context.started)
            {
                swingIsHeld = true;
                swingStartTime = Time.time;
                audioManager.Play("swingCharge");
            }
            else if (context.performed)
            {
                aimAxes = context.ReadValue<Vector2>();
                if (aimAxes != Vector2.zero && Math.Abs(aimAxes.x / aimAxes.y) == 1)
                {
                    aimAxesBuffer = aimAxes;
                }
                aimBufferTime = Time.time;
            }
            else if (context.canceled)
            {
                swingIsHeld = false;
                swingJustReleased = true;
                audioManager.Stop("swingCharge");
                swingReleaseTime = Time.time;
            }
        }

    }

    public void OnSwing(InputAction.CallbackContext context)
    {
        if (isAlive && !isMenuActive)
        {
            if (context.started)
            {
                swingIsHeld = true;
                swingStartTime = Time.time;
                audioManager.Play("swingCharge");
            }
            else if (context.canceled)
            {
                swingIsHeld = false;
                swingJustReleased = true;
                audioManager.Stop("swingCharge");
                swingReleaseTime = Time.time;
            }
        }
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if (context.performed && !isMenuActive)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (pauseMenu == null)
                return;
            isMenuActive = !isMenuActive;
            pauseMenu.SetActive(isMenuActive);

            if (isMenuActive)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }

}
