using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindableObject : MonoBehaviour
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
    private bool isRewinding = false; // Flag to track if the object is rewinding

    private float rewindDuration = 5f; // Rewind time in seconds
    private float recordInterval = 0.02f; // Time between each recorded state
    private int maxFrames; // The maximum number of frames to store in the history buffer

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

        // Cache the rigid body
        rb = GetComponent<Rigidbody>();

        // Calculate the maximum number of frames to store based on duration and interval
        maxFrames = Mathf.CeilToInt(rewindDuration / recordInterval);

        // Start recording states continuously
        StartCoroutine(RecordStates());
    }

    // Continuously record object state while not rewinding
    IEnumerator RecordStates()
    {
        while (true)
        {
            // Only record states if the object is not currently rewinding
            if (!isRewinding)
            {
                // If the state list is full, remove the oldest state
                if (states.Count >= maxFrames)
                {
                    states.RemoveAt(states.Count - 1);
                }

                // Insert newest state at the beginning
                states.Insert(0, new ObjectState(transform.position, transform.rotation));
            }

            // Pause the coroutine for the record interval
            yield return new WaitForSeconds(recordInterval);
        }
    }

    // Called externally to trigger rewind
    public void StartRewind()
    {
        // Ensure the object is not currently rewinding and that it has recorded states
        if (states.Count > 0 && !isRewinding)
        {
            StartCoroutine(PerformRewind());
        }
    }

    private IEnumerator PerformRewind()
    {
        // Set flags to indicate the start of the rewind process
        isRewinding = true;
        IsRewindFinished = false;
        SetGlowInternal(true);

        rb.isKinematic = true; // Disable physics during rewind

        // Iterate through the recorded states and apply them to the object
        foreach (var state in states)
        {
            transform.position = state.position;
            transform.rotation = state.rotation;
            yield return new WaitForSeconds(recordInterval);
        }

        // The rewind is complete. Restore the physics and reset flags
        rb.isKinematic = false;
        isRewinding = false;
        IsRewindFinished = true;
        SetGlowInternal(false);

        // Clear the state history to prepare for a new recording session
        states.Clear();
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
