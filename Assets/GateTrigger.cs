using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    [SerializeField] LevelSelectGate gate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gate.OnGateTriggerCollision();
    }
}
