using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectGate : MonoBehaviour
{
    [SerializeField] private int firstSceneIndex;
    [SerializeField] private GameObject completeText;
    [SerializeField] GameObject subLevelSelectScreen;
    [SerializeField] GameObject[] levels;

    void Awake()
    {
        if (PlayerPrefs.GetInt("maxSceneIndex", 3) >= firstSceneIndex - 1)
        {
            gameObject.SetActive(false);
            Instantiate(completeText, transform.position + transform.up * -4.5f + transform.right * -6f, transform.rotation);
        }
    }

    public void ToggleLevelSelectScreen()
    {
        subLevelSelectScreen.SetActive(!subLevelSelectScreen.activeSelf);
        print(PlayerPrefs.GetInt("maxSceneIndex"));
        print(firstSceneIndex);
        for (int i = 0; i < Mathf.Min(PlayerPrefs.GetInt("maxSceneIndex") - firstSceneIndex + 2, levels.Length); i++)
        {
            levels[i].SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ToggleLevelSelectScreen();
    }
}
