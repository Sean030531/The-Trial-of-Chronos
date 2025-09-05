using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuralExplosion : MonoBehaviour
{
    // The force and radius of the explosion
    private float explosionForce = 70f;
    private float explosionRadius = 12f;

    // The mural pieces' Rigidbodies
    private Rigidbody[] muralPieces;

    void Start()
    {
        // Get all Rigidbody components from the children of this object
        muralPieces = GetComponentsInChildren<Rigidbody>();

        // Explode
        ExplodeMural();
    }

    public void ExplodeMural()
    {
        // The central position of the explosion
        Vector3 explosionPosition = transform.position;

        // Loop through all mural pieces and add the explosion force
        foreach (Rigidbody rb in muralPieces)
        {
            if (rb != null)
            {
                rb.AddExplosionForce(-explosionForce, explosionPosition, explosionRadius, 1f, ForceMode.Impulse);
            }
        }
    }
}
