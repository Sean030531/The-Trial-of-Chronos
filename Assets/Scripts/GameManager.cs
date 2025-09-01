using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public string LastSceneName { get; private set; } // Hold scene the player was in last

    void Awake()
    {
        // Check if an instance of GameManager already exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Call this before load the Home scene
    public void SaveCurrentScene()
    {
        LastSceneName = SceneManager.GetActiveScene().name;
    }

    // Call this from “Resume” button
    public void ResumeLastScene()
    {
        // Check if a scene name has been stored
        if (!string.IsNullOrEmpty(LastSceneName))
        {
            SceneManager.LoadScene(LastSceneName); // Load the saved scene by its name
        }
        else
        {
            // Log a warning if the LastSceneName is null or empty
            Debug.LogWarning("GameManager: No scene saved to resume!");
        }
    }
}
