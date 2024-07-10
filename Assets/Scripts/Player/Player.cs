using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
    private bool swingIsHeld;
    private bool swingJustReleased;
    private Vector2 aimAxes;
    private float startTime;
    private float swingTime;
    private float hammerDuration;

    [SerializeField] private float maxSpeedX;
    [SerializeField] private float maxSpeedY;
    [SerializeField] private float acceleration;
    [SerializeField] private float dragCoefficient;
    [SerializeField] private float jumpForce;
    [SerializeField] private float strongThreshold;
    [SerializeField] private float swingCooldown;

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
        Debug.DrawRay(bc2D.bounds.center, aimAxes * bc2D.bounds.size);
        hammerDuration = Time.time - startTime;
        Move();
        if (swingIsHeld && hammerDuration >= strongThreshold)
        {
            sprite.color = Color.gray;
        }
        if (IsTouching(-rb2D.transform.up))
        {
            if (jump)
            {
                Jump();
            }
            else
            {
                ApplyDrag();
            }
        }
        if(swingJustReleased && IsTouching(aimAxes) && hammerDuration > swingCooldown)
        {
            Swing();
        }
        swingJustReleased = false;
        // LimitVelocity();
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
    }

    private void ApplyDrag()
    {
        if (moveAxis == 0f || IsChangingDirection)
        {
            rb2D.AddForce(new Vector2(-rb2D.velocity.x * dragCoefficient * Time.deltaTime, 0f));
        }
    }

    private void LimitVelocity()
    {
        if (Mathf.Abs(rb2D.velocity.x) > maxSpeedX)
        {
            rb2D.velocity = new Vector2(Mathf.Sign(rb2D.velocity.x) * maxSpeedX, rb2D.velocity.y);
        }
        if (Mathf.Abs(rb2D.velocity.y) > maxSpeedY)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, Mathf.Sign(rb2D.velocity.y) * maxSpeedY);
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
        // instant press of space bar leads to weak hammer force
        if (swingTime < strongThreshold)
        {
            sprite.color = Color.yellow;
            rb2D.AddForce(-aimAxes * weakHammerForce, ForceMode2D.Impulse);
            // Debug.Log("Weak Hammer Collision");
        }
        // holding the bar for more than one second leads to strong hammer force
        else
        {
            sprite.color = Color.red;
            rb2D.AddForce(-aimAxes * strongHammerForce, ForceMode2D.Impulse);
            // Debug.Log("Strong Hammer Collision");
        }

        swingTime = 0f;
    }
}
