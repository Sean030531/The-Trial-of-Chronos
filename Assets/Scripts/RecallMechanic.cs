using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class RecallMechanic : MonoBehaviour
{
    public GameObject crosshairUI; // UI crosshair object to toggle
    public Image recallIconUI; // Icon that switches between sprites
    public Sprite crosshairSprite; // Default icon sprite
    public Sprite timeSandSprite; // Sprite shown during recall
  
    public Image keyHintIcon; // Key hint icon that switches between sprites
    public Sprite rightClickSprite; // Default key hint icon sprite    
    public Sprite leftClickSprite; // Key hintsprite shown during recall   

    private bool isVisible = false; // Tracks if crosshair is active
    private bool isRewinding = false; // Tracks if rewinding is active

    // Helper struct to store a material and its original state for fading effects
    private struct MatInfo { public Material mat; public Material original; }
    private List<MatInfo> mats;


    // Start is called before the first frame update
    void Start()
    {
        // Initially hide crosshair UI
        crosshairUI.SetActive(false);

        // Initialize the list to cache player's material information.
        mats = new List<MatInfo>();
        // Iterate through all Renderers in children
        foreach(var rend in GetComponentsInChildren<Renderer>())
        {
            // Iterate through its materials
            foreach(var mat in rend.materials)
            {
                // Cache a reference to the current material
                mats.Add(new MatInfo { mat = mat, original = new Material(mat) });
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // When right-click is pressed, show/hide crosshair and change hint sprite
        if (Input.GetMouseButtonDown(1) && !isRewinding)
        {
            isVisible = !isVisible;

            // Change based on isVisible
            crosshairUI.SetActive(isVisible);

            recallIconUI.sprite = isVisible ? timeSandSprite : crosshairSprite;
            keyHintIcon.sprite = isVisible ? leftClickSprite : rightClickSprite;
        }

        // When left-click is pressed, shoot a ray
        if (isVisible && !isRewinding && Input.GetMouseButtonDown(0))
        {
            // Convert mouse position to a viewport point 
            Vector3 viewportPoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            // Create a ray from viewport point 
            Ray ray = Camera.main.ViewportPointToRay(viewportPoint);
            RaycastHit hit;

            // Perform the raycast
            if (Physics.Raycast(ray, out hit, 100f))
            {
                // Try to get a RewindableObject component from the hit collider or its parents
                var rewindObj = hit.collider.GetComponentInParent<RewindableObject>();
                if (rewindObj != null)
                {
                    // If rewindable object is found, start rewind
                    isRewinding = true;
                    StartCoroutine(HandleRewind(rewindObj));
                }
                // If it's not RewindableObject, check if it's a RewindableMural
                else if (hit.collider.GetComponentInParent<RewindableMural>() is RewindableMural piece)
                {
                    // If a mural piece is hit, rewind all pieces of mural
                    isRewinding = true;

                    // Get all RewindableMural components in children of that parent
                    var parent = piece.transform.parent;
                    var allPieces = parent.GetComponentsInChildren<RewindableMural>();
                    StartCoroutine(HandleRewindMural(allPieces));
                }
            }

            // After attempting recall, reset the UI
            ResetRecallUI();
        }

        // Apply player fade effect
        SetPlayerFade(isVisible);

        // Apply RewindableObject glow effect
        GlowRewindableObject(isVisible);
    }
    
    IEnumerator HandleRewind(RewindableObject rewindScript)
    {
        // Start rewind
        rewindScript.StartRewind();
        yield return new WaitUntil(() => rewindScript.IsRewindFinished); // Wait for rewind to complete
        isRewinding = false;
    }

    private IEnumerator HandleRewindMural(RewindableMural[] pieces)
    {
        // Start rewind
        foreach (var p in pieces)
        {
            p.StartRewind();
        }
        // Wait until every piece reports done
        yield return new WaitUntil(() =>
        {
            foreach (var p in pieces)
            {
                if (!p.IsRewindFinished) return false;
            }
            return true;
        });

        isRewinding = false;
    }

    // Helper function for reset UI to default
    private void ResetRecallUI()
    {
        recallIconUI.sprite = crosshairSprite;
        keyHintIcon.sprite = rightClickSprite;
        isVisible = false;
        crosshairUI.SetActive(false);
    }

    // Glow effect on all active RewindableObject
    private void GlowRewindableObject(bool on)
    {
        foreach (var o in FindObjectsOfType<RewindableObject>())
            o.SetGlow(on);

        foreach (var m in FindObjectsOfType<RewindableMural>())
            m.SetGlow(on);
    }

    // Applies fade effect
    void SetPlayerFade(bool on)
    {
        foreach(var info in mats)
        {
            var m = info.mat;
            if (on)
            {
                // Set render mode to "Fade"
                m.SetFloat("_Mode", 3); 
                // Set blend modes for alpha blending
                m.SetInt  ("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                m.SetInt  ("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                m.SetInt  ("_ZWrite", 0);
                // Disable/enable keywords to ensure correct shader variant for transparency
                m.DisableKeyword("_ALPHATEST_ON");
                m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                m.EnableKeyword("_ALPHABLEND_ON");
                // Set render queue to ensure transparent objects are rendered after opaque ones
                m.renderQueue = 3000;

                // Apply the desired fade alpha
                var c = m.color; 
                c.a = 0.5f; 
                m.color = c;
            }
            else
            {
                // Restore properties from saved copy
                m.CopyPropertiesFromMaterial(info.original);
            }
        }
    }
}
