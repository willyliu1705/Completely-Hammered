using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{

    [SerializeField] private GameObject[] Dests;
    private int DestIndex = 0;

    [SerializeField] private float speed = 2f;

    [SerializeField] Transform platform;

    // Update is called once per frame
    private void Update()
    {
        if (Vector2.Distance(Dests[DestIndex].transform.position, platform.position) < .1f)
        {
            DestIndex++;
            if (DestIndex >= Dests.Length)
            {
                DestIndex = 0;
            }
        }
        platform.position = Vector2.MoveTowards(platform.position, Dests[DestIndex].transform.position, Time.deltaTime * speed);
    }
}
