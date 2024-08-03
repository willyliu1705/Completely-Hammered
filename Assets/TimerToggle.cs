using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerToggle : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;
    private int timerToggle => PlayerPrefs.GetInt("TimerToggle");

    public void Awake()
    {
        if (timerToggle == 1)
        {
            buttonText.text = "Disable Timer";
        }
        else
        {
            buttonText.text = "Enable Timer";
        }
    }

    public void ToggleTimer()
    {
        if (timerToggle == 1)
        {
            PlayerPrefs.SetInt("TimerToggle", 0);
            buttonText.text = "Enable Timer";
        }
        else
        {
            PlayerPrefs.SetInt("TimerToggle", 1);
            buttonText.text = "Disable Timer";
        }
    }
}
