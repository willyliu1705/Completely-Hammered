using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatStick : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.SetParent(transform);
        }
        print("Enter");
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.SetParent(null);
        }
        print("Exit");
    }  
    
}
