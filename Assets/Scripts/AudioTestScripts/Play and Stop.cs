using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayandStop : MonoBehaviour
{
    private int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("FullCharge");
    }

    // Update is called once per frame
    void Update()
    {

        if (i == 99)
            FindObjectOfType<AudioManager>().Stop("FullCharge");
        i++;
        if (i == 100) i = 0;

    }
}
