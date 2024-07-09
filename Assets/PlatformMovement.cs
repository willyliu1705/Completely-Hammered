using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{

    [SerializeField] private GameObject[] Dests;
    private int DestIndex = 0;

    [SerializeField] private float speed = 2f;

    // Update is called once per frame
    private void Update()
    {
        if (Vector2.Distance(Dests[DestIndex].transform.position, transform.position) < .1f)
        {
            DestIndex++;
            if (DestIndex >= Dests.Length)
            {
                DestIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, Dests[DestIndex].transform.position, Time.deltaTime * speed);
    }
}
