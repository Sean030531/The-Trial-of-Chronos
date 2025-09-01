using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class CoinCollector : MonoBehaviour
{
    private string coinTag = "Coin"; // Collectable's Tag
    public GameObject box; // Collecter
    private string correctBoxTag = "CorrectBox"; // Correct Box Tag
    public Animator doorAnimator; // Door's Animator
    private int requiredCount = 5; // Required Coin Amount

    public TMP_Text countdownText = null; // Countdown Text

    private int collectedCount = 0; 
    private bool isCorrectBox = false;

    // Start is called before the first frame update
    void Start()
    {
        // Check if is correct box
        isCorrectBox = box.CompareTag(correctBoxTag);

        // Initialize the UI text to the required count
        if (countdownText != null)
        {
            countdownText.text = requiredCount.ToString();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Early exit if the object is not a coin
        if (!other.CompareTag(coinTag)) return;

        collectedCount++;
        Destroy(other.gameObject);

        // Update the UI text to show the remaining count
        if (countdownText != null)
        {
            int remainingCount = Mathf.Max(0, requiredCount - collectedCount);
            countdownText.text = remainingCount.ToString();
        }

        // Open door when this is the correct box and enough balls are collected
        if (isCorrectBox && collectedCount >= requiredCount)
        {
            OpenDoor();
        }
    }

    // Perform open door animation
    private void OpenDoor()
    {
        if (doorAnimator) 
        {
            doorAnimator.SetTrigger("OpenDoor");
        }
    }
}