using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string city; // The city scene name.
    public string seabed; // The seabed scene name.

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Adjust the tag as per your player's tag.
        {
            // Check the current scene name.
            string currentScene = SceneManager.GetActiveScene().name;

            // Determine which scene to load based on the current scene.
            string sceneToLoad = currentScene == "City" ? seabed : city;

            // Load the specified scene.
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}