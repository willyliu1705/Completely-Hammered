using UnityEngine;

public class WindWhirl : MonoBehaviour
{
    public float whirlForce = 10f;
    public Vector2 whirlDirection = Vector2.right;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(whirlDirection * whirlForce, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(whirlDirection * whirlForce * Time.deltaTime, ForceMode2D.Force);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Optionally, you can add logic for when the object leaves the whirl
    }
}
