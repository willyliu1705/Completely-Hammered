using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseRestartScript : MonoBehaviour
{

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Player playerScript;
    [SerializeField] private KeyCode restartKey;
    [SerializeField] private KeyCode pauseKey;

    private bool isMenuActive;
    private float restartHoldTime;

    private void Awake()
    {
        isMenuActive = false;
        restartHoldTime = 0f;
    }

    private void Update()
    {
        if (Input.GetKey(restartKey))
        {
            restartHoldTime += Time.deltaTime;

            if (restartHoldTime >= 1f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        else
        {
            restartHoldTime = 0f;
        }

        if (Input.GetKeyDown(pauseKey))
        {
            isMenuActive = !isMenuActive;
            pauseMenu.SetActive(isMenuActive);

            if (isMenuActive)
            {
                Time.timeScale = 0f;
                playerScript.DisablePlayerInput();
            }
            else
            {
                Time.timeScale = 1f;
                playerScript.EnablePlayerInput();
            }
        }

    }

}
