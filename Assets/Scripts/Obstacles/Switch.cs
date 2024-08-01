using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Switch : MonoBehaviour
{

    [SerializeField] GameObject SwitchObj;
    [SerializeField] Transform[] TriggerDests;

    [SerializeField] Transform TriggeredObject;


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
            TriggeredObject.position = Vector2.MoveTowards(TriggeredObject.position, TriggerDests[1].transform.position, Time.deltaTime * speed);
        }
        else
        {
            TriggeredObject.position = Vector2.MoveTowards(TriggeredObject.position, TriggerDests[0].transform.position, Time.deltaTime * speed);
        }
    }
}

    


