using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectGate : MonoBehaviour
{
    [SerializeField] private int firstSceneIndex;
    [SerializeField] private GameObject completeText;

    void Awake()
    {
        if (PlayerPrefs.GetInt("maxSceneIndex") >= firstSceneIndex - 1)
        {
            gameObject.SetActive(false);
            Instantiate(completeText, transform.position + transform.up * -4.5f + transform.right * -6f, transform.rotation);
        }
    }
}
