using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using Unity.VisualScripting;

public class Options : MonoBehaviour
{
    private AudioManager audioManager;

    [SerializeField] private float defaultVolume = 1.0f;

    [SerializeField] private Slider musicSlider = null;
    [SerializeField] private Slider sfxSlider = null;
    [SerializeField] private TMP_Text musicNumber = null;
    [SerializeField] private TMP_Text sfxNumber = null;


    private float tempMusicVol = 1.0f;
    private float tempSFXVol = 1.0f;

    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";

    // Start is called before the first frame update
    void Start()
    {
        DisplayVolume();
    }

    public void DisplayVolume()
    {
        float musicvol = PlayerPrefs.GetFloat("MusicKey", 1f);
        float sfxvol = PlayerPrefs.GetFloat("SFXKey", 1f);

        musicSlider.value = musicvol;
        int musicPercentage = Mathf.RoundToInt(musicvol * 100);
        musicNumber.text = musicPercentage.ToString();
        sfxSlider.value = sfxvol;
        int sfxPercentage = Mathf.RoundToInt(sfxvol * 100);
        sfxNumber.text = sfxPercentage.ToString();
    }

    public void DefaultVolume()
    {
        tempMusicVol = defaultVolume;
        tempSFXVol = defaultVolume;

        musicSlider.value = defaultVolume;
        sfxSlider.value = defaultVolume;

        int defaultPercentage = Mathf.RoundToInt(defaultVolume * 100);
        musicNumber.text = defaultPercentage.ToString();
        sfxNumber.text = defaultPercentage.ToString();

        ApplyMusicVolume();
        ApplySFXVolume();

    }

    public void ChangeMusicVolume(float v)
    {
        tempMusicVol = v;
        int musicPercentage = Mathf.RoundToInt(v * 100);
        musicNumber.text = musicPercentage.ToString();
    }

    public void ChangeSFXVolume(float v)
    {
        tempSFXVol = v;
        int sfxPercentage = Mathf.RoundToInt(v * 100);
        sfxNumber.text = sfxPercentage.ToString();
    }

    public void ApplyMusicVolume()
    {
        PlayerPrefs.SetFloat("MusicKey", tempMusicVol);
        PlayerPrefs.Save();
        UpdateVolumeLevels();
    }

    public void ApplySFXVolume()
    {
        PlayerPrefs.SetFloat("SFXKey", tempSFXVol);
        PlayerPrefs.Save();
        UpdateVolumeLevels();
    }

    private void UpdateVolumeLevels()
    {
        if (audioManager == null)
        {
            audioManager = AudioManager.instance; 
        }

        float musicVolume = PlayerPrefs.GetFloat("MusicKey", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXKey", 1f);

        foreach (Sound s in audioManager.sounds)
        {
            if (audioManager.MusicClips.Contains(s.clip))
            {
                s.source.volume = musicVolume;
            }
            else
            {
                s.source.volume = sfxVolume;
            }
        }
    }

    public void returnMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
