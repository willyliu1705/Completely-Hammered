using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinalTImeScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private void Awake()
    {
        text.text = "Time: " + TimeSpan.FromSeconds(PlayerPrefs.GetFloat("timePlayed")).ToString("hh\\:mm\\:ss\\.ff");
    }

    public void ResetTime()
    {
        PlayerPrefs.SetFloat("timePlayed", 0);
    }
}
