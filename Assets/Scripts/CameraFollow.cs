using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Player Position
    private Vector3 offset = new Vector3(0,3,-3); // Camera offset
    private float rotationSpeed = 7.0f; // Mouse Sensitivity
    private float minCameraHeight = 1.0f; // The minimum camera height
    private LayerMask occlusionMask; // Camera occluders' layer
    private float collisionBuffer = 0.2f; // Collision Buffer
    private float lookAtHeight = 3f; // Look amount of height higher than player

    private float pitch = 0f, yaw = 0f;

    void Start()
    {
        // Set occlusion mask
        occlusionMask = LayerMask.GetMask("CameraOccluders");

        // Initializing pitch and yaw
        Vector3 angles = transform.eulerAngles;
        pitch = angles.x;
        yaw = angles.y;
    }

    void Update()
    {
        // Handle rotation input
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        yaw  += mouseX;
        pitch = Mathf.Clamp(pitch - mouseY, -80f, 80f);

        Quaternion rot = Quaternion.Euler(pitch, yaw, 0);

        // Compute the ideal camera position
        Vector3 idealCamPos = player.position + rot * offset;

        // Raycast from the player toward that position
        Vector3 dir = idealCamPos - player.position;
        float dist = dir.magnitude;
        Ray ray = new Ray(player.position, dir.normalized);

        Vector3 finalPosition;

        if (Physics.Raycast(ray, out RaycastHit hit, dist, occlusionMask))
        {
            // Obstacle in the way: place camera just before it
            float hitDist = hit.distance - collisionBuffer;
            hitDist = Mathf.Max(hitDist, 0.5f); // Never go inside the player
            finalPosition = player.position + dir.normalized * hitDist;
        }
        else
        {
            // Clear line-of-sight: go to ideal position
            finalPosition = idealCamPos;
        }

        // Ensure the camera doesn't go below the minimum height
        finalPosition.y = Mathf.Max(finalPosition.y, player.position.y + minCameraHeight);
        transform.position = finalPosition;

        // Always look at the player
        transform.LookAt(player.position + Vector3.up * lookAtHeight);
    }
}