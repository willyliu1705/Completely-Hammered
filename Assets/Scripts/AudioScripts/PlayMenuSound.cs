using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMenuSound : MonoBehaviour
{
    public void playSound()
    {
        AudioManager.instance.Play("swingWeak");
    }

    public void playSoundStrong()
    {
        AudioManager.instance.Play("swingStrong");
    }
}
