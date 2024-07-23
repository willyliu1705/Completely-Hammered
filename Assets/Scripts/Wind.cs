using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] private float windspeed = 2f;
    [SerializeField] private Vector2 direction;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private Rigidbody2D rb2D2;


    private void Start()
    {
        rb2D = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        rb2D2 = GameObject.FindGameObjectWithTag("Obstacle").GetComponent<Rigidbody2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)  
    {
        rb2D.AddForce(direction.normalized * windspeed, ForceMode2D.Impulse);
        rb2D2.AddForce(direction.normalized * windspeed, ForceMode2D.Impulse);
    }
}

