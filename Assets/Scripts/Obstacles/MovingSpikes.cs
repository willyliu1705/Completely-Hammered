using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpikes : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private Transform[] points;
    [SerializeField] private GameObject spike;
    private int index = 0; 

    // Update is called once per frame
    void Update()
    {
        spike.transform.position = Vector2.MoveTowards(spike.transform.position, points[index].transform.position, Time.deltaTime * speed);
        if (Vector2.Distance(spike.transform.position, points[index].position) < 0.1f)
        {
            index = (index + 1) % points.Length;
        }
    }
}
