using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seaweed : MonoBehaviour
{
    private PlayerController playerController; // Reference to the player's controller.
    private float originalSpeed; // Store the original player speed.

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerController != null)
            {
                // Store the player's original speed.
                originalSpeed = playerController.moveSpeed;

                // Slow down the player by half.
                playerController.moveSpeed /= 2;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerController != null)
            {
                // Restore the player's original speed when leaving the seaweed collider.
                playerController.moveSpeed = originalSpeed;
            }
        }
    }
}
