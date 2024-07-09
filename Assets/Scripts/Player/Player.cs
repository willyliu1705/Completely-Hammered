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

    private float moveAxis;
    private bool jump;
    private Vector2 aimAxes;
    private bool swing;

    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float groundDrag;

    [SerializeField] private float jumpForce;
    [SerializeField] private float hammerForce;
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
        if (swing)
        {
            Swing();
        }
        else if (jump)
        {
            Jump();
        }
        else if (IsGrounded)
        {
            ApplyGroundDrag();
        }

        if(!IsGrounded)
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
            print("ground drag");
            rb2D.drag = groundDrag;
        }
        else
        {
            rb2D.drag = 0f;
        }
    }

    private void ApplyAirDrag()
    {
        print("air drag");
        rb2D.drag = airDrag;
    }

    private void Jump()
    {
            ApplyAirDrag();
            rb2D.AddForce(rb2D.transform.up * jumpForce, ForceMode2D.Impulse);
            jump = false;
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
        Debug.Log(aimAxes);
    }

    public void OnSwing(InputAction.CallbackContext context)
    {
        swing = context.ReadValueAsButton() && IsGrounded;
    }

    private void Swing()
    {
            if (Physics2D.BoxCast(bc2D.bounds.center, bc2D.bounds.size, 0f, aimAxes, raycastBuffer + 2f, groundLayerMask))
            {
                ApplyAirDrag();
                rb2D.AddForce(hammerForce * -aimAxes, ForceMode2D.Impulse);
                Debug.Log("Hammer Collision");
            }
            swing = false;
    }
}
