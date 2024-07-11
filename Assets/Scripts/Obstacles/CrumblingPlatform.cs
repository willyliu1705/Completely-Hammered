using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;

public class CrumblingPlatform : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private float destroytime = 1f;
    [SerializeField] private float respawntime = 3f;
    [SerializeField] public SpriteRenderer sprite;
    [SerializeField] public Collider2D platform; 

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.transform.position.y > transform.position.y)
        {

            StartCoroutine(RespawnPlatform());

        }
    }

    IEnumerator RespawnPlatform()
    {
        yield return new WaitForSeconds(destroytime);
        Toggle(false);
        yield return new WaitForSeconds(respawntime);
        Toggle(true);
    }

    private void Toggle(bool b)
    {
        sprite.enabled = b;
        platform.enabled = b;
    }
}
