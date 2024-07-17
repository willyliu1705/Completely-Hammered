using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjectSpawner : MonoBehaviour
{
    public GameObject fallingObjectPrefab; // Reference to the obstacle prefab
    public float spawnInterval = 3;  // Time between spawns
    private Vector2 spawnArea = new Vector2(0, 0); // Area within which obstacles will be spawned

    private float timer;

    void Start()
    {
        //spawnInterval = Random.Range(1, 10);
        timer = spawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SpawnObstacle();
            spawnInterval = Random.Range(0, 10);
            timer = spawnInterval;
        }
    }

    void SpawnObstacle()
    {
        Vector2 spawnPosition = new Vector3(
            Random.Range(-spawnArea.x / 2, spawnArea.x / 2),
            transform.position.y
        );

        Instantiate(fallingObjectPrefab, spawnPosition, Quaternion.identity);
    }

}
