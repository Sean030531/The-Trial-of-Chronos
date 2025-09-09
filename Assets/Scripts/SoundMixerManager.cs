using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    public static SoundMixerManager Instance;

    public AudioMixer audioMixer;
    public AudioMixerGroup soundEffectGroup;

    void Awake()
    {
        // Check if an instance of MusicManager already exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20f);
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20f);
    }

    public void SetSoundEffectVolume(float value)
    {
        audioMixer.SetFloat("SoundEffectVolume", Mathf.Log10(value) * 20f);
    }
}
