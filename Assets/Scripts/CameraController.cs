using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private GameObject cameraManager;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        Camera.main.orthographicSize = 7f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
