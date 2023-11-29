using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

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
    public int stone = 0;
    public int Stone { get { return stone; } set { stone = value; } }
    private int wood = 0;
    public int Wood { get { return wood; } set { wood = value; } }
    public float maxPickupRange = 3.0f; // Adjust the range as needed.
    public float sellRange = 2.0f;

    SellAreaController sellArea;
    CameraController cameraController;
    InfoBar infoBar;

    public TopScreenUIController topScreenUIController;

    private static PlayerFishController instance;
    public static PlayerFishController Instance
    {
        get { if (instance == null)
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
        cameraController = FindObjectOfType<CameraController>();
        infoBar = FindAnyObjectByType<InfoBar>();
        DontDestroyOnLoad(gameObject);
        topScreenUIController.UpdateHPUI(100);
        topScreenUIController.UpdateEnergyUI(100);
        topScreenUIController.UpdateFoodUI(100);
        topScreenUIController.UpdateRocksUI(0);
        topScreenUIController.UpdateWoodUI(0);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            wood++;
            topScreenUIController.UpdateWoodUI(wood);
        }

        #region
        bool isPopupWindowOpen = sellArea.IsPopUpWindowOpen();
        if (isPopupWindowOpen)
        {
            isMoving = true;
            // Stop camera movement...
            cameraController.cameraMoveSpeed = 0.0f;
        }
        else
        {
            isMoving = false;
            // Allow camera movement...
            cameraController.cameraMoveSpeed = 10.0f;
        }
        #endregion

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("clicked: " + hit.collider.name);
                switch (hit.collider.tag)
                {
                    case "Tile":
                    case "CityTile":
                    case "Portal":
                        Tile tile = hit.collider.GetComponent<Tile>();
                        if (tile != null)
                        {
                            currentDestination = tile.transform.position;
                            if (!isMoving)
                            {
                                StartCoroutine(MoveToTile(currentDestination));
                            }
                        }
                        break;

                    case "Pickup":
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
                        break;

                    case "SellFishArea":
                        Debug.Log("clicked SellFishArea");
                        MoveToSellArea(hit.collider.transform.position);
                        Debug.Log("Touching: " + hit.collider.name);
                        break;
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("clicked: " + hit.collider.name);
                Vector3 distance = transform.position - hit.collider.transform.position;
                float approximateDistance = distance.magnitude;
                switch (hit.collider.name)
                {
                    case "Wood(Clone)":
                        infoBar.DisplayItemInfo(hit.collider.name, "Sturdy piece of HardWood!", 2, approximateDistance, "");
                        break;
                    case "Stone1(Clone)":
                        infoBar.DisplayItemInfo(hit.collider.name, "Thick Stone, Great For Building!", 2, approximateDistance, "");
                        break;
                    case "Coral1(Clone)":
                        infoBar.DisplayItemInfo(hit.collider.name, "Beautiful Coral, Great For Making Tools!", 2, approximateDistance, "");
                        break;
                    case "Food1(Clone)":
                        infoBar.DisplayItemInfo(hit.collider.name, "Tasty Food, Keeps your energy Levels Up!", 2, approximateDistance, "");
                        break;
                    case "SellFish(Clone)":
                        infoBar.DisplayItemInfo(hit.collider.name, "Sell Goods Here!", 1000000, approximateDistance, "");
                        break;
                    case "Cylinder":
                        infoBar.DisplayItemInfo("Civizens Hut", "A Fishy Residence", 200, approximateDistance, "");
                        break;
                }
            }
        }

        void MoveToSellArea(Vector3 sellPosition)
        {
            float distanceToSellArea = Vector3.Distance(transform.position, sellPosition);

            if (distanceToSellArea >= sellRange)
            {
                currentDestination = sellPosition;
                if (!isMoving)
                {
                    StartCoroutine(MoveToTile(currentDestination));
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

        void CollectPickup(Pickup pickup)
        {
            if (pickup.pickupType == PickupType.Food)
            {
                food++;
                Destroy(pickup.gameObject);
            }
            else if (pickup.pickupType == PickupType.Rock)
            {
                stone++;
                Destroy(pickup.gameObject);
            }
            else if (pickup.pickupType == PickupType.Wood)
            {
                wood++;
                Destroy(pickup.gameObject);
            }

            // Update the UI based on the collected pickup type and amount
            topScreenUIController.UpdateFoodUI(food);
            topScreenUIController.UpdateRocksUI(stone);
            topScreenUIController.UpdateWoodUI(wood);
        }


    }
}
