using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class ScreenEdgeScript : MonoBehaviour
{
    [SerializeField] private string goScreen;
    // Add Desired Scenes into Build Settings for this to function

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Loader.Load(goScreen);
    }
}
