using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Player playerScript;

    private bool isMenuActive;
    private bool isRestarting;

    private void Awake()
    {
        isMenuActive = false;
        isRestarting = false;
    }

    //public void OnPause(InputAction.CallbackContext context)
    //{
    //    if (context.started)
    //    {
    //        if (pauseMenu == null)
    //        {
    //            Debug.Log("pause menu does not exist!");
    //            return;
    //        }
    //        else
    //        {
    //            Debug.Log("Exists!");
    //            isMenuActive = !isMenuActive;
    //            pauseMenu.SetActive(isMenuActive);

    //            if (isMenuActive)
    //            {
    //                Time.timeScale = 0f;
    //            }
    //            else
    //            {
    //                Time.timeScale = 1f;
    //            }
    //        }

    //    }
    //}

    //public void OnRestart(InputAction.CallbackContext context)
    //{
    //    if (context.performed && context.duration >= 1f && !isRestarting)
    //    {
    //        isRestarting = true;
    //        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //    }
    //}

}
