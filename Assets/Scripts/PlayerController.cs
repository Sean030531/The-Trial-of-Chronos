using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Transform cameraTransform;  // Player Camera

    private Rigidbody rb; // Player rigid body 
    private float movementSpeed = 5.0f; // Player movement speed
    private float turnSpeed = 720f; // Player rotate speed
    private bool canMove = true; // Flag to control if the player can move

    private Animator playerAnimator; // Player animator

    private Vector3 respawnPosition; // Respawn position

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Cached player rigid body 
        playerAnimator = GetComponent<Animator>(); // Cached player animator

        respawnPosition = transform.position; // Save start position to respawn position
    }

    // Update is called once per frame
    void Update()
    {
        // Only call MovePlayer if movement is enabled
        if (canMove)
        {
            MovePlayer();
        }

        // Respawn if player fell of world
        if (transform.position.y < -20f)
        {
            Respawn();
        }
    }

    void MovePlayer()
    {
        // Get user input from the horizontal and vertical axes
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Get the camera's forward and right vectors
        Vector3 camF = cameraTransform.forward; 
        camF.y = 0; 
        camF.Normalize();

        Vector3 camR = cameraTransform.right;  
        camR.y = 0; 
        camR.Normalize();

        // Calculate the combined movement direction vector
        Vector3 moveDir = (camF * v + camR * h);

        // Check if significant movement input
        if (moveDir.sqrMagnitude > 0.0001f)
        {
            // Player movement
            Vector3 delta = moveDir.normalized * movementSpeed * Time.deltaTime;
            transform.position += delta;

            // Player facing
            Quaternion target = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, turnSpeed * Time.deltaTime);

            playerAnimator.SetBool("Moving", true); // Perform player run animation
        }
        else
        {
            playerAnimator.SetBool("Moving", false); // Perform player idle animation
        }
    }

    private void Respawn()
    {
        // Reset physics
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Jump‐start a frame later to avoid teleport in mid‐physics step
        StartCoroutine(RespawnOnNextFrame());
    }

    private IEnumerator RespawnOnNextFrame()
    {
        yield return new WaitForFixedUpdate();
        canMove = true; // Allow player movement
        transform.position = respawnPosition; // Respawn on respawn position
    }

    void OnCollisionEnter(Collision collision)
    {
        // If collides with the rock, disable movement temporarily
        if (collision.gameObject.CompareTag("Rock"))
        {
            StartCoroutine(DisableMovementTemporarily());
        }
    }

    IEnumerator DisableMovementTemporarily()
    {
        playerAnimator.SetBool("Moving", false); // Perform player idle animation
        canMove = false; // Flag player cannot move

        yield return new WaitForSeconds(2.0f); // Disable movement for 2 seconds

        canMove = true; // Flag player can move
    }

    public void OnTriggerEnter(Collider other)
    {
        // Change to Rolling Ball Puzzle
        if (other.tag == "ToRollingBallsPuzzle")
        {
            SceneManager.LoadScene("Level 1 Rolling Balls Puzzle");
        }

        // Change to Timed Door Puzzle
        if (other.tag == "ToTimedDoorPuzzle")
        {
            SceneManager.LoadScene("Level 1 Timed Door Puzzle");
        }

        // Change to Platform Movement Puzzle
        if (other.tag == "ToPlatformMovementPuzzle")
        {
            SceneManager.LoadScene("Level 1 Platform Movement Puzzle");
        }

        // Change to Elevator Puzzle
        if (other.tag == "ToElevatorPuzzle")
        {
            SceneManager.LoadScene("Level 2 Elevator Puzzle");
        }

        // Change to Mural Puzzle
        if (other.tag == "ToMuralPuzzle")
        {
            SceneManager.LoadScene("Level 2 Mural Puzzle");
        }

        // Change to Game Complete page
        if (other.tag == "ToGameComplete")
        {
            SceneManager.LoadScene("UI Game Complete Page");
        }
    }
}