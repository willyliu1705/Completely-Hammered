using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    [SerializeField] public List<AudioClip> SFXClips = new List<AudioClip>();
    [SerializeField] public List<AudioClip> MusicClips = new List<AudioClip>();

    // Start is called before the first frame update
    void Awake()
    {
        
        if (instance == null)
        { 
            instance = this;
            DontDestroyOnLoad(gameObject);
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
        }
        else
        {
            Destroy(gameObject);
            return;
        }
       LoadVolume();
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        if (s != null)
        {
            s.source.Stop();
        }
    }
    Sound bgm;
    public void PlayMusic(string name)
    {
        if(bgm != null){
            if (bgm.name == name){
                return;
            }
        }
        foreach (Sound s in sounds){
            if (s.name == name){
                bgm = s;
            }
        }
        StopMusic();
        Play(name);
    }

    private void StopMusic()
    {
        foreach (Sound s in sounds)
        {
            if (s.name.StartsWith("music"))
            {
                s.source.Stop();
            }
        }
    }
    public void LoadVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicKey", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXKey", 1f);

        foreach(Sound s in sounds)
        {
            if (MusicClips.Contains(s.clip))
            {
                s.source.volume = musicVolume * s.volume;
            }
            else
            {
                s.source.volume = sfxVolume * s.volume;
            }
        }

    }



}
