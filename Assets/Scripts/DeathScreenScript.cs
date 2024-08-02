using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreenScript : MonoBehaviour
{
    public Image image;
    private bool fadeIn; //go from light to dark
    public bool fadeOut; //go from dark to light

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Color color = image.color;
        if (fadeIn && color.a < 1f)
        {
            color.a += 0.01f;
            
        } else if(fadeOut && color.a > 0f)
        {
            color.a -= 0.01f;
        }

        image.color = color;
    }

    public void StartFadeIn()
    {
        fadeIn = true;
        fadeOut = false;
    }

    public void StartFadeOut()
    {
        fadeIn = false;
        fadeOut = true;
    }
}
