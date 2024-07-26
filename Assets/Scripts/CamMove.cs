using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class CamMove : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Camera cam;
    [SerializeField] private float camSpeed = 30;
    private GameObject dest;
    private GameObject ld;
    private bool done= false;
    void Start()
    {
        dest = GameObject.Find("camSet");
        ld = GameObject.Find("LoadMangement");
    }

    // Update is called once per frame
    void Update()
    {   

        if ((cam.transform.position != dest.transform.position) && !done)
        {
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, dest.transform.position, Time.deltaTime * camSpeed);
        }
        if(cam.transform.position == dest.transform.position)
        {
            done = true;
        }
    }
}
