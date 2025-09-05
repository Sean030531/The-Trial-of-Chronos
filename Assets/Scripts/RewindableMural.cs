using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindableMural : MonoBehaviour
{
    // Struct to store an object's position and rotation at a moment in time
    private struct ObjectState
    {
        public Vector3 position;
        public Quaternion rotation;

        public ObjectState(Vector3 pos, Quaternion rot)
        {
            position = pos;
            rotation = rot;
        }
    }

    private List<ObjectState> states = new List<ObjectState>(); // History of past states
    private bool isRecording = true; // Flag to track if the object is recording
    private bool isRewinding = false; // Flag to track if the object is rewinding

    private float rewindDuration = 5f; // Rewind time in seconds
    private float recordInterval = 0.02f; // Time between each recorded state

    private Rigidbody rb; // Reference to the rigid body
    
    // A public property that allows other scripts to check if the rewind process has finished
    public bool IsRewindFinished { get; private set; } = true;

    public Color glowColor = Color.blue; // Glow effect color

    // Dictionary to store the original emission colors of all renderers
    private Dictionary<Renderer, Color> originalEmissions = new Dictionary<Renderer, Color>();
    // Shader property ID for efficiency
    private static readonly int EmCol = Shader.PropertyToID("_EmissionColor");

    // Start is called before the first frame update
    void Start()
    {
        // Cache renderer components from this object and its children
        var renderers = GetComponentsInChildren<Renderer>();
        foreach (var rend in renderers)
        {
            var mat = rend.material;
            // If the material has an emission color property, store its original color
            if (mat.HasProperty(EmCol))
            {
                originalEmissions[rend] = mat.GetColor(EmCol);
            }
        }

        rb = GetComponent<Rigidbody>(); // Cache the rigid body

        StartCoroutine(RecordStates()); // Start recording states
    }

    // Call on start, record once
    IEnumerator RecordStates()
    {
        float elapsed = 0f;
        while (elapsed < rewindDuration)
        {
            // Add the current position and rotation to the state history list
            states.Add(new ObjectState(transform.position, transform.rotation));

            // Pause the coroutine for the record interval
            yield return new WaitForSeconds(recordInterval);
            elapsed += recordInterval;
        }

        isRecording = false; // After the recording duration is over, stop recording
        rb.isKinematic = true; // Make the object not affected by physics
    }

    // Called externally to trigger rewind
    public void StartRewind()
    {
        // Ensure the object is not currently recording or rewinding, and that it has recorded states
        if (!isRecording && !isRewinding && states.Count > 0)
        {
            StartCoroutine(PerformRewindAndPlayback());
        }
    }

    private IEnumerator PerformRewindAndPlayback()
    {
        isRewinding = true;
        IsRewindFinished = false;
        SetGlowInternal(true);

        // Rewind phase, iterate through the recorded states in reverse (end to start)
        for (int i = states.Count - 1; i >= 0; i--)
        {
            transform.position = states[i].position;
            transform.rotation = states[i].rotation;
            yield return new WaitForSeconds(recordInterval);
        }

        // Wait for 5 second at the end of the rewind
        SetGlowInternal(false);
        yield return new WaitForSeconds(5f);

        // Playback phase, iterate through the recorded states forward (start to end)
        for (int i = 0; i < states.Count; i++)
        {
            transform.position = states[i].position;
            transform.rotation = states[i].rotation;
            yield return new WaitForSeconds(recordInterval);
        }

        // The rewind and playback cycle is complete
        isRewinding = false;
        IsRewindFinished  = true;
    }

    // A private method for setting the glow internally, bypassing the external check
    private void SetGlowInternal(bool on)
    {
        // Iterate through all stored renderers and their original emission colors
        foreach (var kv in originalEmissions)
        {
            var rend = kv.Key;
            var orig = kv.Value;
            var mat = rend.material;
            if (on)
            {
                // Enable the emission keyword and set the glow color
                mat.EnableKeyword("_EMISSION");
                mat.SetColor(EmCol, glowColor);
            }
            else
            {
                // Restore the original emission color
                mat.SetColor(EmCol, orig);
            }
        }
    }

    public void SetGlow(bool on)
    {
        if (isRewinding)
        {
            // If the mural is currently rewinding, ignore external glow commands
            return;
        }

        // Apply the glow based on the external command
        SetGlowInternal(on);
    }
}
