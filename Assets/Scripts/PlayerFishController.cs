using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("Player Settings")]
    public int hp = 100;
    public int energy = 100;
    public int food = 0;
    public int rocks = 0;
    public int wood = 0;
    public float maxPickupRange = 3.0f; // Adjust the range as needed.
    public float sellRange = 2.0f;

    SellAreaController sellArea;

    public TopScreenUIController topScreenUIController;

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
        sellArea = FindAnyObjectByType<SellAreaController>();
        DontDestroyOnLoad(gameObject);
        topScreenUIController.UpdateHPUI(100);
        topScreenUIController.UpdateEnergyUI(100);
        topScreenUIController.UpdateFoodUI(100);
        topScreenUIController.UpdateRocksUI(0);
        topScreenUIController.UpdateWoodUI(0);
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
                        currentDestination = tile.transform.position;
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
                        float distanceToPickup = Vector3.Distance(transform.position, pickup.transform.position);
                        if (distanceToPickup <= maxPickupRange)
                        {
                            // Call the CollectPickup method to handle pickup collection
                            CollectPickup(pickup);
                        }
                    }
                }
                else if (hit.collider.CompareTag("SellFish"))
                {
                    float distanceToSellArea = Vector3.Distance(transform.position, hit.collider.transform.position);

                    OpenSellWindow();
                }
            }
        }
    }

    void OpenSellWindow()
    {
        if (sellArea.playerInRange == true)
        {
            sellArea.popUpWindow.SetActive(true);
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

    void CollectPickup(Pickup pickup)
    {
        if (pickup.pickupType == PickupType.Food)
        {
            food++;
            Destroy(pickup.gameObject);
        }
        else if (pickup.pickupType == PickupType.Rock)
        {
            rocks++;
            Destroy(pickup.gameObject);
        }
        else if (pickup.pickupType == PickupType.Wood)
        {
            wood++;
            Destroy(pickup.gameObject);
        }

        // Update the UI based on the collected pickup type and amount
        topScreenUIController.UpdateFoodUI(food);
        topScreenUIController.UpdateRocksUI(rocks);
        topScreenUIController.UpdateWoodUI(wood);
    }
}
