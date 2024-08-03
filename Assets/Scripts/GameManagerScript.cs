using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
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
    [SerializeField] private Button continueButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject options;
    [SerializeField] private Options opt;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject soundPanel;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private Text timePlayedText;

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
        continueButton.onClick.AddListener(continuePressed);
        quitButton.onClick.AddListener(quitPressed);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex > PlayerPrefs.GetInt("maxSceneIndex"))
        {
            PlayerPrefs.SetInt("maxSceneIndex", currentSceneIndex);
        }
        PlayerPrefs.Save();
        optionsButton.onClick.AddListener(optionsPressed);
        backButton.onClick.AddListener(exitOptions);
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
                options.SetActive(false);
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
        PlayerPrefs.SetFloat("timePlayed", PlayerPrefs.GetFloat("timePlayed") + Time.deltaTime);
        if(PlayerPrefs.GetInt("TimerToggle") == 1)
        {
            timePlayedText.text = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("timePlayed")).ToString("m\\:ss\\.ff");
        }
        else
        {
            timePlayedText.text = "";
        }
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
        playerScript.EnablePlayerInput();

    }

    public void quitPressed()
    {
        playerScript.EnablePlayerInput();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");  //Quit button in pause menu returns to main menu
    }

    private void optionsPressed()
    {
        options.SetActive(true);
        showOptionsPanel();
        opt.DisplayVolume();
        pauseMenu.SetActive(false);
    }

    private void exitOptions()
    {
        options.SetActive(false);
        pauseMenu.SetActive(true);
    }
    private void showOptionsPanel()
    {
        optionsPanel.SetActive(true);
        soundPanel.SetActive(false);
        controlsPanel.SetActive(false);
    }

}
