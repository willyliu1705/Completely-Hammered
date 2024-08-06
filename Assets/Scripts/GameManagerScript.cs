using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance;
    [SerializeField] private GameObject pause;
    public Player player;
    [SerializeField] private KeyCode restartKey;
    [SerializeField] private KeyCode pauseKey;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject timer;
    [SerializeField] private TextMeshProUGUI timerText;

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
        player = FindFirstObjectByType<Player>();
        StartFadeOut();
        timer.SetActive(PlayerPrefs.GetInt("TimerToggle") == 1);
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
            pause.SetActive(isMenuActive);
            pauseMenu.SetActive(isMenuActive);

            if (isMenuActive)
            {
                Time.timeScale = 0f;
                player.DisablePlayerInput();
            }
            else
            {
                optionsMenu.SetActive(false);
                Time.timeScale = 1f;
                player.EnablePlayerInput();
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
        PlayerPrefs.SetFloat("timePlayed", PlayerPrefs.GetFloat("timePlayed") + Time.deltaTime);
        if(PlayerPrefs.GetInt("TimerToggle") == 1)
        {
            TimeSpan time = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("timePlayed"));
            if (PlayerPrefs.GetFloat("timePlayed") < 3600) {
                timerText.text = time.ToString("m\\:ss\\.ff");
            }
            else
            {
                timerText.text = time.ToString("h\\:mm\\:ss\\.ff");
            }
        }
    }

    public void ToggleGameTimer()
    {
        timer.SetActive(!timer.activeSelf);
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

    public void continuePressed()
    {
        Time.timeScale = 1f;
        isMenuActive = false;
        pauseMenu.SetActive(isMenuActive);
        pause.SetActive(isMenuActive);
        player.EnablePlayerInput();

    }

    public void quitPressed()
    {
        player.EnablePlayerInput();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");  //Quit button in pause menu returns to main menu
    }

    public void optionsPressed()
    {
        optionsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void exitOptions()
    {
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

}
