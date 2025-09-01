using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDoor : MonoBehaviour
{
    private Animator doorAnimator; // Door animator

    public static PuzzleDoor Instance { get; private set; } // Static instance
    private List<string> correctSequence = new List<string> { "8", "1", "6" }; // Correct Sequence 

    private List<string> inputSequence = new List<string>(3); // Input Sequence 

    // Start is called before the first frame update
    void Start()
    {
        doorAnimator = GetComponent<Animator>(); // Cache dooranimator

        // If no instance of SymbolDoorManager exists, set this as the instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterInput(string buttonID)
    {
        // If the buttonID is already in the input sequence, remove it
        if (inputSequence.Contains(buttonID))
        {
            inputSequence.Remove(buttonID);
        }
        else // Otherwise, add it
        {
            inputSequence.Add(buttonID);
        }

        // Return early if the inputSequence is shorter
        if (inputSequence.Count < correctSequence.Count) return;

        // Assume the sequences match initially
        bool isMatch = true;

        for (int i = 0; i < correctSequence.Count; i++)
        {
            // If elements does not match, set isMatch to false
            if (inputSequence[i] != correctSequence[i])
            {
                isMatch = false;
                break;
            }
        }

        // If the sequences match, open the door
        if (isMatch)
        {
            doorAnimator.SetTrigger("OpenDoor"); // Perform open door animation
        }
        else
        {
            StartCoroutine(ResetPushedButtons()); // Reset pushed buttons
        }
    }

    private IEnumerator ResetPushedButtons()
    {
        // A short delay for animation
        yield return new WaitForSeconds(0.5f);


        foreach (string pressedID in inputSequence)
        {
            // Find the GameObject of the button using its name
            GameObject cubeGO = GameObject.Find("Button_" + pressedID);
            if (cubeGO != null)
            {
                // Get the PuzzleDoorButton component from the found GameObject
                var pusher = cubeGO.GetComponent<PuzzleDoorButton>();
                if (pusher != null)
                {
                    // Call the ResetPush method on the button
                    pusher.ResetPush();
                }
            }
        }
        
        // Clear the input sequence
        inputSequence.Clear();
        yield return null;
    }
}
