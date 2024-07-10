using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    //Will be used to Manage screen Transitions (Cam Movement, Scene mangement, etc.)
    // Add Desired Scenes into Build Settings for this to function
    public static void Load(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}