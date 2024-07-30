using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{

    [SerializeField] private GameObject[] nodes;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private float speed;
    // Platform will return to Checkpoint A after reaching its last checkpoint
    [SerializeField] private bool loop;
    private int nextNode = 0;
    private bool isReturning = false;

    private Rigidbody2D playerRb;

    private void Awake()
    {
        Vector2 direction = nodes[nextNode].transform.position - transform.position;
        GetComponent<Rigidbody2D>().velocity = direction.normalized * speed;
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(nodes[nextNode].transform.position, transform.position) < speed * Time.deltaTime)
        {
            if (nextNode >= nodes.Length - 1) { isReturning = true; }
            else if (nextNode <= 0) { isReturning = false; }
            if (loop && isReturning)
            {
                isReturning = false;
                nextNode = 0;
            }
            else
            {
                nextNode = isReturning ? --nextNode : ++nextNode;
            }

            Vector2 previousVelocity = rb2D.velocity;
            rb2D.velocity = (nodes[nextNode].transform.position - transform.position).normalized * speed;
            Vector2 deltaVelocity = rb2D.velocity - previousVelocity;
            if (playerRb != null) { playerRb.velocity += deltaVelocity; }
        }
    }

    public void AttachRb(Rigidbody2D rb)
    {
        playerRb = rb;
    }

    public void DetachRb()
    {
        playerRb = null;
    }
}
