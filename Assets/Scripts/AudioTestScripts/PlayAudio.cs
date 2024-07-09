using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    private int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("Bubbles");
    }

    // Update is called once per frame
    void Update()
    {
        if (i == 0)
            FindObjectOfType<AudioManager>().Play("Bubbles");
        i++;
        if (i == 100) i = 0;
    }
}
