using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private LayerMask wallLayerMask;

    private float moveAxis;
    private bool jump;
    private Vector2 aimAxes;
    private bool canSwing;
    private float startTime;
    private float endTime;
    private float swingTime;


    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float groundDrag;

    [SerializeField] private float jumpForce;
    [SerializeField] private float weakHammerForce;
    [SerializeField] private float strongHammerForce;
    [SerializeField] private float airDrag;

    private bool IsGrounded => Physics2D.BoxCast(bc2D.bounds.center, bc2D.bounds.size, 0f, Vector2.down, raycastBuffer, groundLayerMask);
    private bool touchingWall => Physics2D.BoxCast(bc2D.bounds.center, bc2D.bounds.size, 0f, Vector2.left, raycastBuffer, wallLayerMask) ||
        Physics2D.BoxCast(bc2D.bounds.center, bc2D.bounds.size, 0f, Vector2.right, raycastBuffer, wallLayerMask);
    private bool IsChangingDirection => rb2D.velocity.x * moveAxis < 0;
    
    private void Awake()
    {
        controls = new Controls();
        controls.Player.AddCallbacks(this);
        controls.Player.Enable();
    }

    private void FixedUpdate()
    {
        Move();
        Swing();
        if (IsGrounded)
        {
            ApplyGroundDrag();
            Jump();
        }
        else
        {
            ApplyAirDrag();
        }
    }

    private void Move()
    {
        rb2D.AddForce(new Vector2(moveAxis, 0f) * acceleration);

        if (Mathf.Abs(rb2D.velocity.x) > maxSpeed)
        {
            rb2D.velocity = new Vector2(Mathf.Sign(rb2D.velocity.x) * maxSpeed, rb2D.velocity.y);
        }
    }

    private void ApplyGroundDrag()
    {
        if (rb2D.velocity.x != 0f && (moveAxis == 0f || IsChangingDirection))
        {
            rb2D.drag = groundDrag;
        }
        else
        {
            rb2D.drag = 0f;
        }
    }

    private void ApplyAirDrag()
    {
        rb2D.drag = airDrag;
    }

    private void Jump()
    {
        if (jump)
        {
            ApplyAirDrag();
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
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
            Debug.Log(aimAxes);
        }
        
    }

    public void OnSwing(InputAction.CallbackContext context)
    {
        if (context.started && (IsGrounded || touchingWall))
        {
            startTime = Time.time;
            canSwing = false;
        }
        else if (context.canceled)
        {
            endTime = Time.time;
            swingTime = Time.time - startTime;
            canSwing = true;
            Debug.Log(swingTime);
        }
    }
    
    private void Swing()
    {
        if (Time.time - endTime > 0.5)
        {
            canSwing = false;
        }

        if (swingTime > 0 && canSwing)
        {
            if (Physics2D.BoxCast(bc2D.bounds.center, bc2D.bounds.size, 0f, -aimAxes, raycastBuffer, groundLayerMask) ||
                Physics2D.BoxCast(bc2D.bounds.center, bc2D.bounds.size, 0f, -aimAxes, raycastBuffer, wallLayerMask))
            {
                // instant press of space bar leads to weak hammer force
                if (swingTime < 1)
                {
                    Debug.Log(Time.time - startTime);
                    ApplyAirDrag();
                    rb2D.AddForce(weakHammerForce * aimAxes, ForceMode2D.Impulse);
                    Debug.Log("Weak Hammer Collision");
                }
                // holding the bar for more than one second leads to strong hammer force
                else
                {
                    ApplyAirDrag();
                    rb2D.AddForce(strongHammerForce * aimAxes, ForceMode2D.Impulse);
                    Debug.Log("Strong Hammer Collision");
                }

                swingTime = 0;
            }
        }
    }

}
