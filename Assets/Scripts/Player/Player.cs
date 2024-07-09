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

    public float speed;
    public float jumpForce;
    
    private void Awake() {
        controls = new Controls();
        controls.Player.AddCallbacks(this);
        controls.Player.Enable();
    }

    private void FixedUpdate() {
        rb2D.transform.position += rb2D.transform.right * moveAxis * speed * Time.deltaTime;
        if (jump && IsGrounded()) {
            rb2D.AddForce(rb2D.transform.up * jumpForce, ForceMode2D.Impulse);
            jump = false;
        }
    }
    
    private bool IsGrounded()
    {
        return Physics2D.Raycast(rb2D.transform.position, -rb2D.transform.up, rb2D.transform.localScale.y / 2 + 0.02f);
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
