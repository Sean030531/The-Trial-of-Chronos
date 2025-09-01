using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    // Change to Game Start page
    public void GoToGameStart()
    {
        SceneManager.LoadScene("UI Game Start Page");
    }

    // Change to Puzzle Selection page
    public void GoToPuzzleSelection()
    {
        SceneManager.LoadScene("UI Puzzle Selection Page");
    }

    // Change to Setting page
    public void GoToSettings()
    {
        SceneManager.LoadScene("UI Settings Page");
    }

    // Change to Home page
    public void GoToHome()
    {
        // Save current scene to GameManager
        GameManager.Instance.SaveCurrentScene();

        SceneManager.LoadScene("UI Home Page");
    }

    // Change to saved page
    public void OnResumeClicked()
    {
        GameManager.Instance.ResumeLastScene();
    }

    // New Game
    public void GoToLevel1()
    {
        SceneManager.LoadScene("Level 1 Gear Puzzle");
    }

    // Change to Gear Puzzle
    public void GoToGearPuzzle()
    {
        SceneManager.LoadScene("Level 1 Gear Puzzle");
    }

    // Change to Rolling Ball Puzzle
    public void GoToRollingBallsPuzzle()
    {
        SceneManager.LoadScene("Level 1 Rolling Balls Puzzle");
    }

    // Change to Timed Door Puzzle
    public void GoToTimedDoorPuzzle()
    {
        SceneManager.LoadScene("Level 1 Timed Door Puzzle");
    }

    // Change to Platform Movement Puzzle
    public void GoToPlatformMovementPuzzle()
    {
        SceneManager.LoadScene("Level 1 Platform Movement Puzzle");
    }

    // Change to Elevator Puzzle
    public void GoToElevatorPuzzle()
    {
        SceneManager.LoadScene("Level 2 Elevator Puzzle");
    }

    // Change to Mural Puzzle
    public void GoToMuralPuzzle()
    {
        SceneManager.LoadScene("Level 2 Mural Puzzle");
    }
}