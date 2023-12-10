using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;
using Microsoft.Win32.SafeHandles;
using UnityEditor;

public class PlayerFishController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float aboveTileHeight = 0.8f;

    public bool isMoving = false;

    private Pickup targetPickup;
    public List<Pickup> pickups;

    [Header("Tile Movement Settings")]
    public Coroutine currentMoveCoroutine; // Store the current movement coroutine.
    private Vector3 currentDestination;
    private Tile targetTile;

    [Header("Spawn Position Settings")]
    public Vector3 citySpawnPosition; // Public variable for city spawn position.
    public Vector3 seabedSpawnPosition;

    [Header("Player Settings")]
    public int hp = 100;
    public int Health { get { return hp; } set { hp = value; } }

    public int energy = 100;
    public int Energy { get { return energy; } set { energy = value; } }

    public int food = 0;
    public int Food { get { return food; } set { food = value; } }

    public int money = 0;
    public int Money { get { return money; } set { money = value; } }

    public int stone = 0;
    public int Stone { get { return stone; } set { stone = value; } }

    private int wood = 0;
    public int Wood { get { return wood; } set { wood = value; } }

    private int coral = 0;
    public int Coral { get { return coral; } set { coral = value; } }

    public float maxPickupRange = 3.0f; // Adjust the range as needed.
    public float sellRange = 2.0f;

    SellAreaController sellArea;
    CameraController cameraController;
    InfoBar infoBar;
    public GameObject popupWindow;

    public TopScreenUIController topScreenUIController;

    private static PlayerFishController instance;
    public static PlayerFishController Instance
    {
        get
        {
            if (instance == null)
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
    SellAreaController controller;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        controller = FindObjectOfType<SellAreaController>();
    }

    private void Start()
    {
        // Find all Pickup components in the scene and put them in the list
        pickups = new List<Pickup>(FindObjectsOfType<Pickup>());
        sellArea = FindAnyObjectByType<SellAreaController>();
        cameraController = FindObjectOfType<CameraController>();
        topScreenUIController = FindAnyObjectByType<TopScreenUIController>();
        infoBar = FindAnyObjectByType<InfoBar>();
        popupWindow = GameObject.FindGameObjectWithTag("PopupWindow");

        DontDestroyOnLoad(gameObject);
        topScreenUIController.UpdateHPUI(100);
        topScreenUIController.UpdateEnergyUI(100);
        topScreenUIController.UpdateFoodUI(100);
        topScreenUIController.UpdateWoodUI(0);
        topScreenUIController.UpdateStoneUI(0);
        topScreenUIController.UpdateCoralUI(0);
        topScreenUIController.UpdateMoneyUI(0);
        StartCoroutine(EnergyControl());
    }

    void Update()
    {

        

        if (topScreenUIController == null)
        {
            topScreenUIController = FindAnyObjectByType<TopScreenUIController>();
        }

        if (Input.GetKey(KeyCode.C))
        {
            money += 100;
        }
        if (Input.GetKey(KeyCode.T))
        {
            wood++;
        }
        if (Input.GetKey(KeyCode.Y))
        { 
            stone++;
        }
        if (Input.GetKey(KeyCode.U))
        { 
            coral++;
        }
        if (Input.GetKey(KeyCode.G))
        { 
            controller.SellWood();
        }
        if (Input.GetKey(KeyCode.H))
        { 
            controller.SellStone();
        }
        if (Input.GetKey(KeyCode.J))
        {
            controller.SellCoral();
        }

        #region
        if (sellArea != null)
        {
            bool isPopupWindowOpen = sellArea.IsPopUpWindowOpen();
            if (isPopupWindowOpen)
            {
                cameraController.cameraMoveSpeed = 0.0f;
            }
            else
            {
                cameraController.cameraMoveSpeed = 10.0f;
            }
        }
        #endregion

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                switch (hit.collider.tag)
                {
                    case "Tile":
                    case "CityTile":
                    case "Portal":
                        Debug.Log("Clicked on Tile or CityTile or Portal");
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
                        Debug.Log("Clicked on Pickup");
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
                        Debug.Log("Clicked on SellFishArea");
                        MoveToSellArea(hit.collider.transform.position);
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
                Vector3 distance = transform.position - hit.collider.transform.position;
                float approximateDistance = distance.magnitude;
                switch (hit.collider.name)
                {
                    case "Wood(Clone)":
                        Debug.Log("Clicked on Wood(Clone)");
                        infoBar.DisplayItemInfo(hit.collider.name, "Sturdy piece of HardWood!", 2, approximateDistance, "");
                        break;

                    case "Stone1(Clone)":
                        Debug.Log("Clicked on Stone1(Clone)");
                        infoBar.DisplayItemInfo(hit.collider.name, "Thick Stone, Great For Building!", 2, approximateDistance, "");
                        break;

                    case "Coral1(Clone)":
                        Debug.Log("Clicked on Coral1(Clone)");
                        infoBar.DisplayItemInfo(hit.collider.name, "Beautiful Coral, Great For Making Tools!", 2, approximateDistance, "");
                        break;

                    case "Food1(Clone)":
                        Debug.Log("Clicked on Food1(Clone)");
                        infoBar.DisplayItemInfo(hit.collider.name, "Tasty Food, Keeps your energy Levels Up!", 2, approximateDistance, "");
                        break;

                    case "SellFish(Clone)":
                        Debug.Log("Clicked on SellFish(Clone)");
                        infoBar.DisplayItemInfo(hit.collider.name, "Sell Goods Here!", 1000000, approximateDistance, "");
                        break;

                    case "Cylinder":
                        Debug.Log("Clicked on Cylinder");
                        infoBar.DisplayItemInfo("Civizens Hut", "A Fishy Residence", 200, approximateDistance, "");
                        break;

                    case "CityTile(Clone)":
                        Debug.Log("Clicked on CityTile(Clone)");
                        infoBar.DisplayBlank();
                        break;

                    case "BaseTile(Clone)":
                        Debug.Log("Clicked on BaseTile(Clone)");
                        infoBar.DisplayBlank();
                        break;
                }
            }
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        topScreenUIController.UpdateHPUI(hp);
        topScreenUIController.UpdateFoodUI(food);
        topScreenUIController.UpdateWoodUI(wood);
        topScreenUIController.UpdateStoneUI(stone);
        topScreenUIController.UpdateCoralUI(coral);
        topScreenUIController.UpdateMoneyUI(money);
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

    public IEnumerator MoveToTile(Vector3 targetPosition)
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
        else if (pickup.pickupType == PickupType.Coral)
        {
            coral++;
            Destroy(pickup.gameObject);
        }

        // Update the UI based on the collected pickup type and amount
        topScreenUIController.UpdateFoodUI(food);
        topScreenUIController.UpdateStoneUI(stone);
        topScreenUIController.UpdateWoodUI(wood);
        topScreenUIController.UpdateCoralUI(coral);
    }

    IEnumerator EnergyControl()
    {
        for (int i = 0; Energy >= i; Energy--)
        {
            topScreenUIController.UpdateEnergyUI(energy);
            if (energy == 0)
            {
                for (int j = 1; Health >= j; Health -= 5)
                {
                    topScreenUIController.UpdateHPUI(hp);
                    yield return new WaitForSeconds(1.0f);
                }
            }
            else
            {
                if (food != 0)
                {
                    Energy = 100;
                    if (food != 0)
                    {


                        for (int j = 1; Food >= j; Food -= 1)
                        {
                            topScreenUIController.UpdateFoodUI(food);
                            yield return new WaitForSeconds(5.0f);
                        }
                    }
                    else
                        break;
                }
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    public void FreezePlayer()
    {
        isMoving = false;
        StopAllCoroutines();
    }

    public void UnfreezePlayer()
    {
        StartCoroutine(MoveToTile(currentDestination));
        isMoving = true;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            // Player is dead, handle death logic here
            // Display game over screen, reset player position, etc.
            Debug.Log("GAME OVER, PLAYER DIED");

            Destroy(gameObject);
        }
        else
        {
            // Player is still alive, update UI or perform other actions
            topScreenUIController.UpdateHPUI(hp); // Update the health UI
        }
    }



}

