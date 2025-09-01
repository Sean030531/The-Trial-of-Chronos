using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGenerator : MonoBehaviour
{
    public GameObject rockPrefab; // rock Prefab
    private float generateInterval = 5f; // Time between each generate

    // Track all spawned rocks and which ones are currently inside the trigger zone
    private List<GameObject> generatedRocks = new();
    private HashSet<GameObject> inside = new();
    private Collider zone;

    // Start is called before the first frame update
    void Start()
    {
        zone = GetComponent<Collider>(); // Cached collider
        StartCoroutine(GenerateLoop());
    }

    // Update is called once per frame
    void Update()
    {
        // Comtrol each rocks status
        RockStatusControl();
    }

    private IEnumerator GenerateLoop()
    {
        // Repeat forever
        while (true)
        {
            // Pause while any rock is inside or rewinding
            while (inside.Count > 0 || AnyRockRewinding())
            {
                yield return null;
            }

            // Generate a new rock at the spawner's position
            var rock = Instantiate(rockPrefab, rockPrefab.transform.position, rockPrefab.transform.rotation);
            generatedRocks.Add(rock);

            yield return new WaitForSeconds(generateInterval);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Get the root GameObject of the colliding object to handle child colliders
        var collidedObjectRoot = other.transform.root.gameObject;
        
        // If the collided object is one of our generated rocks, add it to the 'inside' set
        if (generatedRocks.Contains(collidedObjectRoot))
        {
            inside.Add(collidedObjectRoot);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Get the root GameObject of the colliding object
        var collidedObjectRoot = other.transform.root.gameObject;
        
        // Remove the object from the 'inside' set
        inside.Remove(collidedObjectRoot);
    }

    // Return true if any rock is currently rewinding
    private bool AnyRockRewinding()
    {
        foreach (var go in generatedRocks)
        {
            // Skip any null entries
            if (!go) continue;

            // Check for a RewindableObject component and its rewind state
            var ro = go.GetComponent<RewindableObject>();
            if (ro && !ro.IsRewindFinished) return true;
        }
        return false;
    }

    private void RockStatusControl()
    {
        foreach (var go in generatedRocks)
        {
            // Skip any null entries
            if (!go) continue;

            // Find the true center
            Renderer rend = go.GetComponentInChildren<Renderer>();
            float y = rend.bounds.center.y;  

            // Destroy if rock fell of world
            if (y < -20f)
            {
                Destroy(go);
            }

            // If rock is on upper platform, change it tag to Untagged, avoid stunning player
            if (y > 16f)
            {
                go.tag = "Untagged";
            }
            else
            {
                go.tag = "Rock";
            }
        }
    }
}
