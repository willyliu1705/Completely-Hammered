using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] string musicName;
    // Start is called before the first frame update
    void Start()
    {
    AudioManager audi = FindObjectOfType<AudioManager>();
    audi.PlayMusic(musicName);
    }
}
