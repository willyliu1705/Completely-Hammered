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

    private float moveAxis;
    private bool jump;
    private Vector2 aimAxes;
    private bool swing;

    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float groundDrag;

    [SerializeField] private float jumpForce;
    [SerializeField] private float airDrag;

    private bool IsGrounded => Physics2D.BoxCast(bc2D.bounds.center, bc2D.bounds.size, 0f, Vector3.down, raycastBuffer, groundLayerMask);
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
