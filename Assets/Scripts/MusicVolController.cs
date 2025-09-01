using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class MusicVolController : MonoBehaviour
{
    public AudioSource musicSource; // Audio component

    private Slider slider; // Control slider

    void Awake()
    {
        slider = GetComponent<Slider>(); // Cache slider

        // Check if musicSource is assigned 
        if (musicSource == null)
        {
            var go = GameObject.FindWithTag("Music");
            if (go != null)
                musicSource = go.GetComponent<AudioSource>();
            else
                Debug.LogError("MusicVolumeController: No AudioSource provided and no GameObject tagged 'Music' found.");
        }

        // Initialize slider range 0–1
        slider.minValue = 0f;
        slider.maxValue = 1f;

        // Initialize slider value to match the current volume
        slider.value = (musicSource != null) ? musicSource.volume : AudioListener.volume;
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
        if (musicSource != null)
        {
            musicSource.volume = value; // Update musicSource volume
        }
        else
        {
            AudioListener.volume = value; // Update global volume if no AudioSource assigned
        }
    }
}
