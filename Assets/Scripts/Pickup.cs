using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType
{
    Food,
    Rock,
    Coral,
    ScrapWood
};

public class Pickup : MonoBehaviour
{
    public bool isCollidingWithPlayer;
    public Collider playerCollider;

    // Reference to the tile this pickup is on
    private Tile tile;

    private void Start()
    {
        // Initialize the playerCollider reference.
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();

        // Find and store the tile this pickup is on
        tile = GetComponentInParent<Tile>();
    }

    // Get the associated tile
    public Tile GetTile()
    {
        return tile;
    }

    private void OnTriggerEnter(Collider rangeCollider)
    {
        if (rangeCollider == playerCollider)
        {
            isCollidingWithPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == playerCollider)
        {
            isCollidingWithPlayer = false;
        }
    }
}
