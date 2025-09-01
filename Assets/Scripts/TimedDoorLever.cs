using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDoorLever : MonoBehaviour
{
    private Animator leverAnimator; // Lever animator

    public GameObject doorObject; // Door
    public Animator doorAnimator; // Door animator
    private bool isDoorOpen = false; // Check if door is open

    public Transform playerTransform; // Player
    private float interactionDistance = 5f; // Distance within which the player can interact
    private bool isPlayerInRange = false; // Check if player is around

    // Start is called before the first frame update
    void Start()
    {
        // Cache animator
        leverAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if player is within interaction range
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance <= interactionDistance)
        {
            isPlayerInRange = true;
        }
        else
        {
            isPlayerInRange = false;
        }
    }

    void OnMouseDown()
    {
        // Open the door if the door is not opened and player is in range
        if (!isDoorOpen && isPlayerInRange)
        {
            StartCoroutine(OpenAndCloseDoor());
        }
    }

    IEnumerator OpenAndCloseDoor()
    {
        isDoorOpen = true; // Mark door as open
        leverAnimator.SetTrigger("Toggle"); // Perform toggle lever animation
        doorAnimator.SetTrigger("OpenDoor"); // Perform open door animation

        yield return new WaitForSeconds(5f); // Open the door for 5 seconds

        isDoorOpen = false; // Mark door as close
        doorAnimator.SetTrigger("CloseDoor"); // Perform close door animation
    }
}
