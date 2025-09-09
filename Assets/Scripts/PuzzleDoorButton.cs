using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDoorButton : MonoBehaviour
{
    public string buttonID; // Unique button ID (1,2,3...)
    private Animator buttonAnimator; // Button animator
    private bool isClicked = false; // Check if clicked

    public AudioClip toggleSound; // Toggle sound effect
    private AudioSource audioSource; // Reference to the AudioSource component

    // Start is called before the first frame update
    void Start()
    {
        buttonAnimator = GetComponent<Animator>(); // Cached button animator

        // Get the AudioSource component on this GameObject
        audioSource = GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = SoundMixerManager.Instance.soundEffectGroup;
    }

    void OnMouseDown()
    {
        if (!isClicked) // Click button if have not clicked
        {
            buttonAnimator.SetBool("isClicked", true);  // Perform button toggle on animation
            isClicked = true;

            audioSource.PlayOneShot(toggleSound);
        }
        else // Release back
        {
            buttonAnimator.SetBool("isClicked", false);  // Perform button toggle off animation
            isClicked = false;
        }

        // Register buttonID to PuzzleDoor
        PuzzleDoor.Instance.RegisterInput(buttonID);
    }

    public void ResetPush()
    {
        buttonAnimator.SetBool("isClicked", false); // Perform button toggle off animation
        isClicked = false;
    }
}
