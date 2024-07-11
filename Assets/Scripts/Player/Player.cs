using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static Controls;

public class Player : MonoBehaviour, IPlayerActions
{
    private Controls controls;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private BoxCollider2D bc2D;
    [SerializeField] private float raycastBuffer;
    [SerializeField] private LayerMask groundLayerMask;

    // For debug indication of swinging & jumping
    [SerializeField] private SpriteRenderer sprite;

    private float moveAxis;
    private bool jump;
    // private bool isJumping;
    private bool swingIsHeld;
    private bool swingJustReleased;
    private bool isSwingingWeak;
    private bool isSwingingStrong;
    private Vector2 aimAxes;
    private float startTime;
    private float hammerDuration;

    [SerializeField] private float walkSpeed;
    [SerializeField] private float maxJumpSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float dragCoefficient;
    [SerializeField] private float jumpForce;
    [SerializeField] private float strongThreshold;

    [SerializeField] private float weakHammerForce;
    [SerializeField] private float strongHammerForce;

    private bool IsChangingDirection => rb2D.velocity.x * moveAxis < 0;

    private void Awake()
    {
        controls = new Controls();
        controls.Player.AddCallbacks(this);
        controls.Player.Enable();
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(rb2D.position, aimAxes * bc2D.bounds.size);
    
        Move();

        hammerDuration = Time.time - startTime;
        if (swingIsHeld && hammerDuration >= strongThreshold)
        {
            if (hammerDuration < strongThreshold + 0.1f)
            {
                sprite.color = Color.white;
            }
            else
            {
                sprite.color = Color.gray;
            }
        }

        if (IsTouching(-rb2D.transform.up) || IsTouching(-rb2D.transform.right) || IsTouching(-rb2D.transform.right))
        {
            // isJumping = false;
            isSwingingWeak = false;
            isSwingingStrong = false;
        }
        if (swingJustReleased && IsTouching(aimAxes))
        {
            Swing();
        }
        else if (IsTouching(-rb2D.transform.up))
        {
            if (jump)
            {
                Jump();
            }
            else
            {
                LimitWalkSpeed();
                ApplyDrag();
            }
        }

        Debug.Log("Weak: " + isSwingingWeak);
        Debug.Log("Strong: " + isSwingingWeak);

        LimitAirSpeed();
        swingJustReleased = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("lethal"))
        {
            rb2D.velocity = Vector2.zero;
            controls.Player.Disable();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void Move()
    {
        sprite.color = Color.black;
        rb2D.AddForce(new Vector2(moveAxis, 0f) * acceleration);
    }

    private void Jump()
    {
        sprite.color = Color.green;
        rb2D.AddForce(rb2D.transform.up * jumpForce, ForceMode2D.Impulse);
        // isJumping = true;
        // revert back to single tap jumping if necessary
        // jump = false;
    }

    private void ApplyDrag()
    {
        if (moveAxis == 0f || IsChangingDirection)
        {
            rb2D.AddForce(new Vector2(-rb2D.velocity.x * dragCoefficient * Time.deltaTime, 0f));
        }
    }

    private void LimitWalkSpeed()
    {
        if (Mathf.Abs(rb2D.velocity.x) >= walkSpeed)
        {
            rb2D.velocity = new Vector2(Mathf.Sign(rb2D.velocity.x) * walkSpeed, rb2D.velocity.y);
        }
    }

    private void LimitAirSpeed()
    {
        if (!isSwingingWeak && !isSwingingStrong && Mathf.Abs(rb2D.velocity.x) >= maxJumpSpeed)
        {
            rb2D.velocity = new Vector2(Mathf.Sign(rb2D.velocity.x) * maxJumpSpeed, rb2D.velocity.y);
        }
    }

    private bool IsTouching(Vector3 direction)
    {
        return Physics2D.BoxCast(bc2D.bounds.center, bc2D.bounds.size, 0f, direction, raycastBuffer, groundLayerMask);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveAxis = context.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jump = true;
        }
        else if (context.canceled)
        {
            jump = false;
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            aimAxes = context.ReadValue<Vector2>();
            Debug.Log(aimAxes);
        }
        else if (context.canceled)
        {
            aimAxes = Vector2.zero;
        }
    }

    public void OnSwing(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            swingIsHeld = true;
            startTime = Time.time;
        }
        else if (context.canceled)
        {
            swingIsHeld = false;
            swingJustReleased = true;
        }
    }

    private void Swing()
    {
        if (hammerDuration < strongThreshold)
        {
            sprite.color = Color.yellow;
            rb2D.AddForce(-aimAxes * weakHammerForce, ForceMode2D.Impulse);
            isSwingingWeak = true;
        }
        // holding the bar for more than `strongThreshold` seconds leads to strong hammer force
        else
        {
            sprite.color = Color.red;
            rb2D.AddForce(-aimAxes * strongHammerForce, ForceMode2D.Impulse);
            isSwingingStrong = true;
        }
    }
}
