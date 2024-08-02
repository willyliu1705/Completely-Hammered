using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectGate : MonoBehaviour
{
    [SerializeField] private int firstSceneIndex;

    void Awake()
    {
        if (PlayerPrefs.GetInt("maxSceneIndex") >= firstSceneIndex - 1)
        {
            gameObject.SetActive(false);
        }
    }
}
