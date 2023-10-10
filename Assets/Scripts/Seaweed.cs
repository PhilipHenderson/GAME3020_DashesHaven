using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seaweed : MonoBehaviour
{
    private PlayerFishController playerFishController; // Reference to the player's controller.
    private float originalSpeed; // Store the original player speed.

    private void Start()
    {
        playerFishController = FindObjectOfType<PlayerFishController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerFishController != null)
            {
                // Store the player's original speed.
                originalSpeed = playerFishController.moveSpeed;

                // Slow down the player by half.
                playerFishController.moveSpeed /= 2;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerFishController != null)
            {
                // Restore the player's original speed when leaving the seaweed collider.
                playerFishController.moveSpeed = originalSpeed;
            }
        }
    }
}
