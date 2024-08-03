using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] private SpriteRenderer button;
    [SerializeField] private Sprite buttonOff;
    [SerializeField] private Sprite buttonOn;
    [SerializeField] private Transform[] triggerDests;
    [SerializeField] private Transform triggeredObject;

    [SerializeField] private float speed = 2f;

    public bool On = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        On = !On;
        FindObjectOfType<AudioManager>().Play("switchClick");
    }
    void Update()
    {
        if (On)
        {
            if (button != null) { button.sprite = buttonOn; }
            triggeredObject.position = Vector2.MoveTowards(triggeredObject.position, triggerDests[1].transform.position, Time.deltaTime * speed);
        }
        else
        {
            if (button != null) { button.sprite = buttonOff; }
            triggeredObject.position = Vector2.MoveTowards(triggeredObject.position, triggerDests[0].transform.position, Time.deltaTime * speed);
        }
    }
}

    


