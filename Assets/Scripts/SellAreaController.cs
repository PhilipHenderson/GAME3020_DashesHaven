using UnityEngine;

public class SellAreaController : MonoBehaviour
{
    public Collider sellAreaCollider; // Reference to the trigger collider.
    public GameObject popUpWindow; // Reference to your UI pop-up window.
    public bool playerInRange;

    private void Start()
    {
        // Ensure the pop-up window is initially disabled.
        if (popUpWindow != null)
        {
            playerInRange = false;
            popUpWindow.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player Is In Range
            if (popUpWindow != null)
            {
                playerInRange = true;
            }
        }
    }
}
