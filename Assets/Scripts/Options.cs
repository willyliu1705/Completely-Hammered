using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Options : MonoBehaviour
{
    [SerializeField] private TMP_Text volumeNumber = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;

    private float appliedVolume = 1.0f;
    private float tempVolume = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        appliedVolume = PlayerPrefs.GetFloat("masterVolume", defaultVolume);
        tempVolume = appliedVolume;
        DisplayVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayVolume()
    {
        volumeSlider.value = appliedVolume;
        volumeNumber.text = appliedVolume.ToString("0.0");
    }

    public void ChangeVolume(float v)
    {
        tempVolume = v;
        volumeNumber.text = v.ToString("0.0");
    }

    public void ApplyVolume()
    {
        AudioListener.volume = tempVolume;
        appliedVolume = tempVolume;
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        PlayerPrefs.Save();
        volumeNumber.text = PlayerPrefs.GetFloat("masterVolume").ToString("0.0");
    }

    public void DefaultVolume()
    {
        tempVolume = defaultVolume;
        volumeSlider.value = defaultVolume;
        volumeNumber.text = defaultVolume.ToString("0.0");
        ApplyVolume();
    }

}
