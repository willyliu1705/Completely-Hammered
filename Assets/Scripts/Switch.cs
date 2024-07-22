using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Switch : MonoBehaviour
{

    [SerializeField] GameObject SwitchObj;

    public bool On = false;

    
    private void OnTriggerStay2D(Collider2D collision)
    {
        On = true;
    }
}
    


