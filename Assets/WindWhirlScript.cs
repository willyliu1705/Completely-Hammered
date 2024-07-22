using UnityEngine;

public class WindWhirl : MonoBehaviour
{
    public float whirlForce = 100f;
    public Vector2 whirlDirection = Vector2.right;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.AddForce(whirlDirection * whirlForce, ForceMode2D.Impulse);
        }
    }

    //private void OnCollisionEnter2D(Collider2D other)
    //{
    //    Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
    //    if (rb != null)
    //    {
    //        rb.gravityScale = 0f;
    //        rb.AddForce(whirlDirection * whirlForce, ForceMode2D.Impulse);
    //    }
    //}

    private void OnCollisionStay2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.AddForce(whirlDirection * whirlForce * Time.deltaTime, ForceMode2D.Force);
        }
    }

    //private void OnTriggerStay2D(Collider2D other)
    //{
    //    Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
    //    if (rb != null)
    //    {
    //        rb.gravityScale = 0f;
    //        rb.AddForce(whirlDirection * whirlForce * Time.deltaTime, ForceMode2D.Force);
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D other)
    //{
    //}
}
