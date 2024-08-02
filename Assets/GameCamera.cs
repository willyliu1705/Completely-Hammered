using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField] private float defaultAspectRatio = 16f / 9f;
    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        float currentAspectRatio = (float) cam.pixelWidth / cam.pixelHeight;
        if (currentAspectRatio < defaultAspectRatio)
        {
            cam.orthographicSize *= defaultAspectRatio / currentAspectRatio;
        }
    }
}
