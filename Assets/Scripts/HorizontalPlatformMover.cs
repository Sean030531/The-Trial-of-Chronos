using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalPlatformMover : MonoBehaviour
{
    private float moveDistance = 10f; // Distance the platform will move back and forth from its starting position
    private float moveSpeed = 1f; // Speed of the platform

    private Vector3 startPosition; // Initial position of the platform

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position; // Store the initial position of the platform
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate new Z position
        float newZ = startPosition.z + Mathf.Sin(Time.time * moveSpeed) * moveDistance;

        // Apply the new position
        transform.position = new Vector3(startPosition.x, startPosition.y, newZ);
    }
}
