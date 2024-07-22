using UnityEngine;

public class WindWhirlScript : MonoBehaviour
{
    public float whirlForce = 1f;
    public Vector2 whirlDirection = Vector2.right;
    public float whirlSpeed = 2f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f;
            //rb.AddForce(whirlDirection * whirlForce/100, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.velocity = whirlDirection * whirlSpeed;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 1f;
        }
    }

}
