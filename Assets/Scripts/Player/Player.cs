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
    private Vector2 aimAxes;
    private bool swing;

    [SerializeField] private float maxSpeedX;
    [SerializeField] private float maxSpeedY;
    [SerializeField] private float acceleration;
    [SerializeField] private float dragCoefficient;
    [SerializeField] private float jumpForce;
    [SerializeField] private float hammerForce;

    private bool IsGrounded => Physics2D.BoxCast(bc2D.bounds.center, bc2D.bounds.size, 0f, Vector3.down, raycastBuffer, groundLayerMask);
    private bool IsTouchingLeftWall => Physics2D.BoxCast(bc2D.bounds.center, bc2D.bounds.size, 0f, Vector3.left, raycastBuffer, groundLayerMask);
    private bool IsTouchingRightWall => Physics2D.BoxCast(bc2D.bounds.center, bc2D.bounds.size, 0f, Vector3.right, raycastBuffer, groundLayerMask);
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
            if (swing && aimAxes.y < 0f)
            {
                Swing();
            }
            else if (jump)
            {
                Jump();
            }
            else
            {
                ApplyDrag();
            }
        }
        else if (swing && (IsTouchingLeftWall && aimAxes.x < 0f || IsTouchingRightWall && aimAxes.x > 0f))
        {
            Swing();
        }
        LimitVelocity();
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
        jump = false;
    }

    private void Swing()
    {
        sprite.color = Color.red;
        rb2D.AddForce(hammerForce * -aimAxes, ForceMode2D.Impulse);
        swing = false;
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
