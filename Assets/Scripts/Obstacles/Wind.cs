using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] private float windspeed = 2f;
    [SerializeField] private Vector2 direction;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private Rigidbody2D[] rb2D2;
    public List<Rigidbody2D> rb2DList = new List<Rigidbody2D>();

    private void FixedUpdate()
    {
        foreach (Rigidbody2D rb2D in rb2DList)
        {
            rb2D.AddForce(direction.normalized * windspeed, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb2D = collision.GetComponent<Rigidbody2D>();
        if (rb2D != null) 
        { 
            rb2DList.Add(rb2D);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Rigidbody2D rb2D = collision.GetComponent<Rigidbody2D>();
        if (rb2D != null)
        {
            rb2DList.Remove(rb2D);
        }
    }
}

