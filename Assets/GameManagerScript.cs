using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Player playerScript;
    [SerializeField] private KeyCode restartKey;
    [SerializeField] private KeyCode pauseKey;

    private bool isMenuActive;
    private float restartHoldTime;

    public Image image;
    private bool fadeIn; //go from light to dark
    public bool fadeOut; //go from dark to light

    private void Awake()
    {
        Instance = this;
        isMenuActive = false;
        restartHoldTime = 0f;
    }

    private void Start()
    {
        StartFadeOut();
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

        //Logic to fade in/out image
        Color color = image.color;
        if (fadeIn && color.a < 1f)
        {
            color.a += 0.01f;

        }
        else if (fadeOut && color.a > 0f)
        {
            color.a -= 0.01f;
        }

        image.color = color;

    }

    public void StartFadeIn()
    {
        fadeIn = true;
        fadeOut = false;
    }

    public void StartFadeOut()
    {
        fadeIn = false;
        fadeOut = true;
    }

}
