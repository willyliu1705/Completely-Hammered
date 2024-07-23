using UnityEngine;

public class WindWhirlScript : MonoBehaviour
{
    public float whirlForce = 10f;
    public Vector2 whirlDirection = Vector2.right;
    public float whirlSpeed = 2f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("whirlWind")) {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0f;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                //rb.velocity = whirlDirection * whirlSpeed;
                rb.AddForce(whirlDirection * whirlForce, ForceMode2D.Impulse);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("whirlWind"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 1f;
            }
        }
        
    }

}
