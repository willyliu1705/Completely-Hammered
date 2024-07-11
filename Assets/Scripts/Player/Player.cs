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
    private bool swingIsHeld;
    private bool swingJustReleased;
    private Vector2 aimAxes;
    private float startTime;
    private float hammerDuration;

    [SerializeField] private float walkSpeed;
    [SerializeField] private Vector2 maxVelocity;
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

        if (!controls.Player.enabled) { return; }

        hammerDuration = Time.time - startTime;
        Move();
        if (swingIsHeld && hammerDuration >= strongThreshold)
        {
            sprite.color = Color.gray;
        }
        if (swingJustReleased && IsTouching(aimAxes) && hammerDuration > swingCooldown)
        {
            Swing();
        }
        else if (IsTouching(-rb2D.transform.up))
        {
            if (jump)
            {
                Debug.Log("Can I jump: " + jump);
                Jump();
            }
            else
            {
                LimitWalkSpeed();
                ApplyDrag();
            }
        }
        
        LimitVelocity();
        swingJustReleased = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("lethal"))
        {
            Debug.Log("Did I collide?: " + collision.gameObject.CompareTag("lethal"));
            rb2D.velocity = Vector2.zero;
            controls.Player.Disable();
            Debug.Log("Controls status:" + controls.Player.enabled);
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

    private void LimitVelocity()
    {
        // If jumping, limit velocity to a lower value
        // If swinging, don't limit? or limit to a higher value
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
        if (hammerDuration < strongThreshold)
        {
            sprite.color = Color.yellow;
            rb2D.AddForce(-aimAxes * weakHammerForce, ForceMode2D.Impulse);
        }
        // holding the bar for more than `strongThreshold` seconds leads to strong hammer force
        else
        {
            sprite.color = Color.red;
            rb2D.AddForce(-aimAxes * strongHammerForce, ForceMode2D.Impulse);
        }
    }
}
