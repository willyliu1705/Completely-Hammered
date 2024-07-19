using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionTriggerScript : MonoBehaviour
{
   
    [SerializeField] private string goScreen;
    [SerializeField] private Camera cam;
    [SerializeField] private float camSpeed = 1;
    private GameObject dest;
    //private float time=0;
    private bool trig = false;
    private void Start()
    {
        dest = GameObject.Find("camTransition");
    }
    private void Update()
    {
        // time = time + Time.deltaTime;
        if (trig)
        {
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, dest.transform.position, Time.deltaTime * camSpeed);
        }
        if (cam.transform.position == dest.transform.position)
            Loader.Load(goScreen);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        trig = true;
        //Loader.Load(goScreen);
    }
}

