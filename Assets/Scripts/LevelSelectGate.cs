using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (subLevelSelectScreen.activeSelf)
        {
            GameManagerScript.Instance.player.DisablePlayerInput();
            Debug.Log("disabling player input");
        }
        else
        {
            GameManagerScript.Instance.player.EnablePlayerInput();
            Debug.Log("enabling player input");
        }
        for (int i = 0; i < Mathf.Min(PlayerPrefs.GetInt("maxSceneIndex") - firstSceneIndex + 2, levels.Length); i++)
        {
            levels[i].SetActive(true);
        }
    }

    public void OnGateTriggerCollision()
    {
        if (PlayerPrefs.GetInt("maxSceneIndex") < firstSceneIndex)
        {
            SceneManager.LoadScene(firstSceneIndex);
        }
        else
        {
            ToggleLevelSelectScreen();
        }
    }
}
