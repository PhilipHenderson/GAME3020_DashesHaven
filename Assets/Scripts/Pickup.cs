using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType
{
    Food,
    Rock,
    Coral,
    Wood
};

public class Pickup : MonoBehaviour
{
    public bool isCollidingWithPlayer;
    public Collider playerCollider;

    public PickupType pickupType;

    private Tile tile;

    private void Start()
    {
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();

        tile = GetComponentInParent<Tile>();
    }

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
