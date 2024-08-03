using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] private string firstTutorialScenePath = "Assets/Scenes/Golden Master/Level 1/1-1.unity";
    [SerializeField] private string levelSelectSceneName = "LevelSelect";

    public void LoadFirstScene()
    {
        int firstTutorialSceneIndex = SceneUtility.GetBuildIndexByScenePath(firstTutorialScenePath);
        if (PlayerPrefs.GetInt("maxSceneIndex") >= firstTutorialSceneIndex)
        {
            Loader.Load(levelSelectSceneName);
        }
        else
        {
            Loader.Load(Path.GetFileNameWithoutExtension(firstTutorialScenePath));
        }
    }
}
