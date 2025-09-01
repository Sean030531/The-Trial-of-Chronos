using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class CloneMechanic : MonoBehaviour
{
    private Rigidbody rb; // Player
    public GameObject cloneMarkerPrefab; // Clone prefab
    public GameObject flameEffectPrefab; // Flame effect prefab
    private Vector3 flameLocalOffset = new Vector3(0, 1f, 0); // Flame prefab offset

    public Image cloneIconUI; // Icon that switches between sprites
    public Sprite cloneSprite; // Default icon sprite
    public Sprite teleportSprite; // Sprite shown after clone point is set
    public TextMeshProUGUI keyHintText;  // Key hint text ("E" or "Q")

    private Vector3 savedPosition; // Saved player position
    private Quaternion savedRotation; // Saved player rotation
    private bool hasClonePoint = false; // If a clone point has been set

    private GameObject currentMarkerInstance; // Instance of the visual clone marker

    // Start is called before the first frame update
    void Start()
    {
        // Cached player rigid body
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Press E: Save clone point
        if (Input.GetKeyDown(KeyCode.E))
        {
            SaveClonePoint();
        }

        // Press Q: Teleport to clone
        if (Input.GetKeyDown(KeyCode.Q) && hasClonePoint)
        {
            TeleportToClone();
        }
    }

    // Save the player's current position and spawn marker
    private void SaveClonePoint()
    {
        savedPosition = transform.position;
        savedRotation = transform.rotation;
        hasClonePoint = true;

        // Replace any existing marker
        if (currentMarkerInstance != null)
            Destroy(currentMarkerInstance);

        // Instantiate new marker
        if (cloneMarkerPrefab != null)
        {
            currentMarkerInstance = Instantiate(cloneMarkerPrefab, savedPosition, savedRotation);

            var flame = Instantiate(flameEffectPrefab, currentMarkerInstance.transform);
            flame.transform.localPosition= flameLocalOffset;
        }

        // Update UI to reflect "teleport" state
        UpdateUI(teleportSprite, "Q");
    }

    // Teleport the player to the saved position
    private void TeleportToClone()
    {
        // Temporarily disable physics
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        // Move and rotate player
        transform.position = savedPosition;
        transform.rotation = savedRotation;

        rb.isKinematic = false;

        // Destroy marker and reset state
        if (currentMarkerInstance != null)
        {
            Destroy(currentMarkerInstance);
        }

        hasClonePoint = false;

        // Update UI to reflect "clone" state
        UpdateUI(cloneSprite, "E");
    }

    // Updates the UI icon and key hint
    private void UpdateUI(Sprite icon, string hintText)
    {
        if (cloneIconUI != null && icon != null)
            cloneIconUI.sprite = icon;

        if (keyHintText != null)
            keyHintText.text = hintText;
    }
}