using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartNewGame()
    {
        // Load the main scene when the "New Game" button is pressed.
        SceneManager.LoadScene("Instructional");

    }

    public void Instructional()
    {
        // Load the main scene when the "New Game" button is pressed.
        SceneManager.LoadScene("City");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
