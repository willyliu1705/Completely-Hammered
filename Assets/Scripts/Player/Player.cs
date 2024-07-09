using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

public class Player : MonoBehaviour, IPlayerActions
{
    private Controls controls;
    public Rigidbody2D rb2D;

    private float moveAxis;
    private bool jump;
    private Vector2 aimAxes;
    private bool swing;

    public float maxSpeed;
    public float acceleration;
    public float groundDrag;

    public float jumpForce;
    public float airDrag;

    private bool IsGrounded => Physics2D.Raycast(rb2D.transform.position, -rb2D.transform.up, rb2D.transform.localScale.y / 2 + 0.02f);
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
        if (moveAxis == 0f || IsChangingDirection)
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
            rb2D.AddForce(rb2D.transform.up * jumpForce, ForceMode2D.Impulse);
            jump = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveAxis = context.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jump = context.ReadValueAsButton();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        aimAxes = context.ReadValue<Vector2>();
    }

    public void OnSwing(InputAction.CallbackContext context)
    {
        swing = context.ReadValueAsButton();
    }
}
