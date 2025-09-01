using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButton : MonoBehaviour
{
    public GameObject elevator;                      
    private Animator buttonAnimator;             

    private float levitateHeight = 10f;                
    private float levitateSpeed  = 2f;                          

    private Vector3   elevatorStartPos;
    private Rigidbody rb;
    private bool elevatorBusy;

    void Start()
    {
        // Cache button animator 
        buttonAnimator = GetComponent<Animator>();

        // Cache elevator rigid body and start position
        rb = elevator.GetComponent<Rigidbody>();
        elevatorStartPos = elevator.transform.position;
    }

    void OnMouseDown()
    {
        // After clicked on button, if elevator is not busy, start it
        if (!elevatorBusy)
        {
            StartCoroutine(DriveElevator());
        }
    }

    IEnumerator DriveElevator()
    {
        buttonAnimator.SetTrigger("Toggle"); // Perform toggle button animation
        elevatorBusy = true; // Mark elevator as busy
        float targetY = elevatorStartPos.y + levitateHeight; // Get target height

        // Levitate up (kinematic, no gravity)
        SetPhysics(kinematic: true, useGravity: false);
        while (elevator.transform.position.y < targetY - 0.01f)
        {
            elevator.transform.position = Vector3.MoveTowards(
                elevator.transform.position, 
                new Vector3(elevatorStartPos.x, targetY, elevatorStartPos.z), 
                levitateSpeed * Time.deltaTime
            );
            yield return null;
        }

        // Hang at top for 5 second
        yield return new WaitForSeconds(5f);

        // Fall back (dynamic, gravity on)
        SetPhysics(kinematic: false, useGravity: true);
        while (elevator.transform.position.y > elevatorStartPos.y + 0.01f)
        {
            yield return null;
        }

        elevatorBusy = false;
    }
    
    // Helper function for set physics
    private void SetPhysics(bool kinematic, bool useGravity)
    {
        rb.isKinematic = kinematic;
        rb.useGravity  = useGravity;
    }
}
