using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDoorButton : MonoBehaviour
{
    public string buttonID; // Unique button ID (1,2,3...)
    private Animator buttonAnimator; // Button animator
    private bool isClicked = false; // Check if clicked

    // Start is called before the first frame update
    void Start()
    {
        buttonAnimator = GetComponent<Animator>(); // Cached button animator
    }

    void OnMouseDown()
    {
        if (!isClicked) // Click button if have not clicked
        {
            buttonAnimator.SetBool("isClicked", true);  // Perform button toggle on animation
            isClicked = true;
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
