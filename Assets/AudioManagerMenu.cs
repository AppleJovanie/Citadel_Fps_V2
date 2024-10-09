using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManagerMenu : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    public AudioClip background;
    [SerializeField] Slider volumeSlider;  // Reference to the UI Slider

    private void Start()
    {
        // Set the audio clip to play
        musicSource.clip = background;
        musicSource.Play();  // Play the background music

        // Set initial volume based on slider value and listen for changes
        musicSource.volume = volumeSlider.value;
        volumeSlider.onValueChanged.AddListener(AdjustVolume);
    }

    // This method is called whenever the slider value changes
    private void AdjustVolume(float volume)
    {
        musicSource.volume = volume;
    }
}
