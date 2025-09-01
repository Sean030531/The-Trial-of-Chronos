using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuralExplosion : MonoBehaviour
{
    private Vector3 force; // The force of the explosion
    private ForceMode forceMode = ForceMode.Impulse; // Impulse force Mode
    private Rigidbody rb; // Mural piece

    // Start is called before the first frame update
    void Start()
    {
        // Cached mural piece
        rb = GetComponent<Rigidbody>();

        // Randomize explode direction
        float forceX  = Random.Range(-5f, 5f);
        float forceY  = Random.Range(-5f, 5f);
        float forceZ  = Random.Range(5f, 10f);
        force = new Vector3(forceX, forceY, forceZ);

        // Add force to mural piece
        rb.AddForce(force, forceMode);
    }
}
