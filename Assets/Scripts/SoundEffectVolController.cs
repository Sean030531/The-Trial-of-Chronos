using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using UnityEngine.Audio;

public class SoundEffectVolController : MonoBehaviour
{
    private Slider slider; // Control slider

    void Awake()
    {
        slider = GetComponent<Slider>(); // Cache slider
        
        // Initialize the slider's value to the current volume from the Audio Mixer
        if (SoundMixerManager.Instance != null && SoundMixerManager.Instance.audioMixer != null)
        {
            float currentVolumeDb;
            // Get the current volume from the mixer
            SoundMixerManager.Instance.audioMixer.GetFloat("SoundEffectVolume", out currentVolumeDb);
            // Convert dB back to a linear value for the slider (0-1)
            slider.value = Mathf.Pow(10, currentVolumeDb / 20f);
        }
    }

    void OnEnable()
    {
        // Called every time the slider's value changes
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnDisable()
    {
        // Called when the GameObject becomes disabled or inactive
        slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        SoundMixerManager.Instance.SetSoundEffectVolume(value); // Update volume
    }
}
