using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockStatusControl : MonoBehaviour
{
    private string originalTag;
    private Coroutine destroyTimerCoroutine; // Reference to the coroutine

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NoStunZone"))
        {
            // If the rock enters the NoStunZone, change its tag and start the timer
            originalTag = gameObject.tag;
            gameObject.tag = "Untagged";

            // Stop any existing coroutine to prevent multiple timers from running
            if (destroyTimerCoroutine != null)
            {
                StopCoroutine(destroyTimerCoroutine);
            }

            // Start the coroutine to destroy the rock after 15 seconds
            destroyTimerCoroutine = StartCoroutine(DestroyAfterTime());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("NoStunZone"))
        {
            // If the rock exits the NoStunZone, stop the timer and revert the tag
            if (destroyTimerCoroutine != null)
            {
                StopCoroutine(destroyTimerCoroutine);
            }

            if (!string.IsNullOrEmpty(originalTag))
            {
                gameObject.tag = originalTag;
            }
        }
    }

    // Coroutine to wait for 15 seconds and then destroy the object
    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(15f);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Renderer rend = GetComponentInChildren<Renderer>();
        float y = rend.bounds.center.y;  

        // Destroy if rock fell of world
        if (y < -20f)
        {
            Destroy(gameObject);
        }
    }
}
