using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] LayerMask groundLayerMask;
    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;
    private bool onGround => OnGround();

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        Walk();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W) && OnGround()) {
            float horiztonalSpeed = rigidBody.velocity.y;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("wall"))
            Debug.Log("Wall Collision");
    }

    private void Walk()
    {
        float verticalSpeed = rigidBody.velocity.y;
        if (Input.GetKey (KeyCode.A)) {
            rigidBody.velocity = new Vector2(-walkSpeed, verticalSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rigidBody.velocity = new Vector2(walkSpeed, verticalSpeed);
        }
        else
        {
            rigidBody.velocity = new Vector2(0, verticalSpeed);
        }
    }

    private bool OnGround()
    {
        // Checks for object on the ground layer directly below the player
        float raycastBuffer = 0.1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector3.down, raycastBuffer, groundLayerMask);
        return raycastHit.collider != null;
    }
}
