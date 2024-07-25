using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour, IHammerable
{
    // Start is called before the first frame update

    [SerializeField] private float destroytime = 1f;
    [SerializeField] private float respawntime = 3f;
    [SerializeField] public SpriteRenderer sprite;
    [SerializeField] public Collider2D platform;
    private Vector2 center;
    private Vector2 size;


    private void Start()
    {
        center = platform.bounds.center;
        size = platform.bounds.size;
    }

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

        if (IsAreaClear())
        {
            Toggle(true);
        }
        else
        {
            StartCoroutine(CheckClear());
        }
    }

    private bool IsAreaClear()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(center, size, 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator CheckClear()
    {
        while (!IsAreaClear())
        {
            yield return new WaitForSeconds(0.5f);
        }
        Toggle(true);
    }

    private void Toggle(bool b)
    {
        if (!b && sprite.enabled && platform.enabled)
        {
            FindObjectOfType<AudioManager>().Play("platformCrumble");
        }
        sprite.enabled = b;
        platform.enabled = b;
    }

    public void OnHammer()
    {
        Toggle(false);
        StartCoroutine(RespawnPlatform());
    }
}
