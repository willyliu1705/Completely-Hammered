using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelTitle : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private int completeSceneIndex;
    [SerializeField] private int winSceneIndex;
    void Awake()
    {
        if (PlayerPrefs.GetInt("maxSceneIndex") >= completeSceneIndex - 1)
        {
            text.color = Color.green;
        }
        if (PlayerPrefs.GetInt("maxSceneIndex") >= winSceneIndex - 1)
        {
            text.color = Color.yellow;
        }
    }
}
