using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ContinueButtonController : MonoBehaviour
{
    private Button continueButton; // Reference to the continue button

    // Start is called before the first frame update
    void Start()
    {
        continueButton = GetComponent<Button>();
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        // The button is interactable only if a scene name has been saved
        continueButton.interactable = !string.IsNullOrEmpty(GameManager.Instance.LastSceneName);
    }
}
