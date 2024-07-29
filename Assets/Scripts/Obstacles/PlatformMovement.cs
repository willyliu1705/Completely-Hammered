using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{

    [SerializeField] private GameObject[] nodes;
    [SerializeField] private GameObject platform;
    [SerializeField] private float speed = 1f;
    private int nextNode = 0;
    private bool isReturning = false;

    private void Awake()
    {
        Vector2 direction = nodes[nextNode].transform.position - platform.transform.position;
        platform.GetComponent<Rigidbody2D>().velocity = direction.normalized * speed;
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(nodes[nextNode].transform.position, platform.transform.position) < speed * Time.deltaTime)
        {
            if (nextNode >= nodes.Length - 1) { isReturning = true; }
            else if (nextNode <= 0) { isReturning = false; }

            nextNode = isReturning ? nextNode - 1 : nextNode + 1;
            Vector2 direction = nodes[nextNode].transform.position - platform.transform.position;
            platform.GetComponent<Rigidbody2D>().velocity = direction.normalized * speed;
        }
    }
}
