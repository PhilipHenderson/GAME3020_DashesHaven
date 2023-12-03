using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string destinationSceneName; // The destination scene name set in the Inspector.

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoadDestinationScene();
        }
    }

    private void LoadDestinationScene()
    {
        SceneManager.LoadScene(destinationSceneName);
    }
}
