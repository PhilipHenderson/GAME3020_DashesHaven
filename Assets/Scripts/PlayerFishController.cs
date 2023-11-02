using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFishController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float aboveTileHeight = 0.8f;

    private bool isMoving = false;
    private Tile targetTile;
    private Pickup targetPickup;

    public List<Pickup> pickups;

    private Coroutine currentMoveCoroutine; // Store the current movement coroutine.

    private Vector3 currentDestination;

    private void Start()
    {
        // Find all Pickup components in the scene and put them in the list
        pickups = new List<Pickup>(FindObjectsOfType<Pickup>());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isMoving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Tile"))
                {
                    Tile tile = hit.collider.GetComponent<Tile>();
                    if (tile != null)
                    {
                        targetTile = tile;
                        currentDestination = targetTile.transform.position;
                        StartCoroutine(MoveToTile(currentDestination));
                    }
                }
                else if (hit.collider.CompareTag("Pickup"))
                {
                    Pickup pickup = hit.collider.GetComponent<Pickup>();
                    if (pickup != null)
                    {
                        targetPickup = pickup;
                        targetTile = pickup.GetTile();
                        currentDestination = targetTile.transform.position;
                        StartCoroutine(MoveToTileAndDestroyPickup(currentDestination));
                    }
                }
            }
        }
    }

    IEnumerator MoveToTile(Vector3 targetPosition)
    {
        isMoving = true;

        Vector3 destination = new Vector3(targetPosition.x, aboveTileHeight, targetPosition.z);

        while (Vector3.Distance(transform.position, destination) > 1.0f)
        {
            // Update the current destination
            destination = new Vector3(currentDestination.x, aboveTileHeight, currentDestination.z);

            // Move towards the destination
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

            // Make the fish look at the destination
            transform.LookAt(destination);

            yield return null;
        }

        isMoving = false;
    }

    IEnumerator MoveToTileAndDestroyPickup(Vector3 targetPosition)
    {
        isMoving = true;

        Vector3 destination = new Vector3(targetPosition.x, aboveTileHeight, targetPosition.z);

        while (Vector3.Distance(transform.position, destination) > 1.0f)
        {
            // Move towards the destination
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

            // Make the fish look at the destination
            transform.LookAt(destination);

            yield return null;
        }

        isMoving = false;

        // Check for any pickups on the target tile and destroy them
        if (targetTile != null)
        {
            targetTile.DestroyPickups();
        }
    }

    // Function to stop the current movement coroutine.
    void StopCurrentMovement()
    {
        if (currentMoveCoroutine != null)
        {
            StopCoroutine(currentMoveCoroutine);
            isMoving = false;
        }
    }
}
