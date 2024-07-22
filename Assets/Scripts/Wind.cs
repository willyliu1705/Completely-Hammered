using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] private float windspeed = 2f;
    [SerializeField] private Vector2 direction;
    [SerializeField] private Rigidbody2D rb2D;

    private void Start()
    {
        rb2D = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)  
    {
        rb2D.AddForce(direction.normalized * windspeed, ForceMode2D.Impulse);
    }
}
