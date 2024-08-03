using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerToggle : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    private int timerToggle => PlayerPrefs.GetInt("TimerToggle");

    public void Awake()
    {
        if (timerToggle == 1)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
    }

    public void ToggleTimer()
    {
        if (timerToggle == 1)
        {
            PlayerPrefs.SetInt("TimerToggle", 0);
        }
        else
        {
            PlayerPrefs.SetInt("TimerToggle", 1);
        }
    }
}
