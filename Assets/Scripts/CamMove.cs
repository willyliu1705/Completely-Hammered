using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class CamMove : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Camera cam;
    [SerializeField] private float camSpeed = 1;
    private GameObject dest;
    void Start()
    {
        dest = GameObject.Find("camTransition");
    }

    // Update is called once per frame
    void Update()
    {
        if (cam.transform.position != dest.transform.position)
        {
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, dest.transform.position, Time.deltaTime * camSpeed);
        }
        if(cam.transform.position == dest.transform.position)
        {
            print("Equal");
        }
    }
}
