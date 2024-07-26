using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObstacleScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject); //play the animation!
        }

    }
}
