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
    private bool isBreaking;
    private bool isRespawning;
    private float breakTimer;
    private float respawnTimer;


    private void Start()
    {
        center = platform.bounds.center;
        size = platform.bounds.size;
    }

    void Update()
    {
        if (isBreaking)
        {
            breakTimer += Time.deltaTime;
            if(breakTimer >= destroytime) { Crumble(); }
        }
        else if(isRespawning)
        {
            respawnTimer += Time.deltaTime;
            if (respawnTimer >= respawntime && IsAreaClear()) { Respawn(); }

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.transform.position.y > transform.position.y)
        {
            isBreaking = true;
        }
    }

    private void Crumble()
    {
        isBreaking = false;
        isRespawning = true;
        breakTimer = 0f;
        Toggle(false);
    }
    private void Respawn()
    {
        isRespawning = false;
        respawnTimer = 0f;
        Toggle(true);
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
        Crumble();
    }
}
