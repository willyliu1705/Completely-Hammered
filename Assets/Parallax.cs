using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float lengthX, lengthY, startposX, startposY;
    public GameObject cam;
    public float parallaxEffect;
    public float starLength;
    

    void Start()
    {
        startposX = transform.position.x;
        startposY = transform.position.y;
        if (GetComponent<SpriteRenderer>() != null)
        {
            lengthX = GetComponent<SpriteRenderer>().bounds.size.x;
            lengthY = GetComponent<SpriteRenderer>().bounds.size.y;

        }
        else {
            lengthX = starLength;
            lengthY = starLength;
        }
    } 

    void Update()
    {
        float tempX = (cam.transform.position.x * (1-parallaxEffect/10));
        float distX = (cam.transform.position.x * parallaxEffect/10);

        float tempY = (cam.transform.position.y * (1-parallaxEffect/10));
        float distY = (cam.transform.position.y * parallaxEffect/10);

        transform.position = new Vector3(startposX + distX, startposY + distY, transform.position.z);

        if (tempX > startposX + lengthX)
        {
            startposX += lengthX;
        }
        else if (tempX < startposX - lengthX)
        {
            startposX -= lengthX;
        }

        if (tempY > startposY + lengthY)
        {
            startposY += lengthY;
        }
        else if (tempY < startposY - lengthY)
        {
            startposY -= lengthY;
        }
    }

}