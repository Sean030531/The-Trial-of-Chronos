using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButtonFollow : MonoBehaviour
{
    public GameObject elevator; // Elevator
    private Vector3 offset; // Relative offset (button to elevator)

    void Start()
    {
        // Cached relative offset
        Transform elevTransform = elevator.transform;
        offset = elevTransform.InverseTransformPoint(transform.position);
    }

    void Update()
    {
        Transform elevTransform = elevator.transform;

        // Desired position from cached offset
        Vector3 targetPos = elevTransform.TransformPoint(offset);

        // Follow Y
        Vector3 p = transform.position;
        p.y = targetPos.y;
        transform.position = p;
    }
}
