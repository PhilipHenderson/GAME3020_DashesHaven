using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f; // Player movement speed.
    public float rotationSpeed = 90.0f; // Rotation speed in degrees per second.

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Handle player input for movement.
        float verticalInput = Input.GetAxis("Vertical"); // Only use Vertical input for forward and backward movement.

        // Calculate the movement vector.
        Vector3 movement = transform.forward * verticalInput * moveSpeed;

        // Apply the movement to the player's Rigidbody.
        rb.velocity = movement;

        // Handle player input for rotation.
        float rotationInput = Input.GetAxis("Rotate");
        float rotationAmount = rotationInput * rotationSpeed * Time.deltaTime;

        // Rotate the player around the Y-axis.
        transform.Rotate(0.0f, rotationAmount, 0.0f);
    }
}
