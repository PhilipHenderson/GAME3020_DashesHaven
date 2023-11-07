using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFishController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float aboveTileHeight = 0.8f;

    private bool isMoving = false;

    private Pickup targetPickup;
    public List<Pickup> pickups;

    [Header("Tile Movement Settings")]
    private Coroutine currentMoveCoroutine; // Store the current movement coroutine.
    private Vector3 currentDestination;
    private Tile targetTile;

    [Header("Spawn Position Settings")]
    public Vector3 citySpawnPosition; // Public variable for city spawn position.
    public Vector3 seabedSpawnPosition;

    private static PlayerFishController instance;

    public static PlayerFishController Instance
    {
        get {if (instance == null)
            {
                // Try to find an existing instance in the scene
                instance = FindObjectOfType<PlayerFishController>();

                // If no instance was found, create a new one
                if (instance == null)
                {
                    GameObject playerFishControllerObject = new GameObject("PlayerFishController");
                    instance = playerFishControllerObject.AddComponent<PlayerFishController>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("PlayerFishController instance created and marked as DontDestroyOnLoad.");
        }
        else
        {
            Debug.Log("Destroying duplicate PlayerFishController instance.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Find all Pickup components in the scene and put them in the list
        pickups = new List<Pickup>(FindObjectsOfType<Pickup>());
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Tile") || hit.collider.CompareTag("CityTile") || hit.collider.CompareTag("Portal"))
                {
                    Tile tile = hit.collider.GetComponent<Tile>();
                    if (tile != null)
                    {
                        // Update the destination immediately
                        currentDestination = tile.transform.position;

                        // If the fish is not moving, start moving to the new destination
                        if (!isMoving)
                        {
                            StartCoroutine(MoveToTile(currentDestination));
                        }
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

                        if (!isMoving)
                        {
                            StartCoroutine(MoveToTileAndDestroyPickup(currentDestination));
                        }
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
        currentMoveCoroutine = null;
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
        currentMoveCoroutine = null;

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
        }
        isMoving = false;
        currentMoveCoroutine = null;
    }


    public void StopMovementOnPortalCollision()
    {
        // Stop the fish's movement and reset its state.
        StopCurrentMovement();
    }
}
