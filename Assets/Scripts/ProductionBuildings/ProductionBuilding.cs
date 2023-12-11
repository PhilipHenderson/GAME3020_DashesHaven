using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ResourceType
{
    Wood,
    Stone,
    Coral,

}

public class ProductionBuilding : MonoBehaviour
{
    public float resourceGenerationInterval = 10f;
    public int resourcesPerInterval = 2;
    public int currentResources = 0;

    public TMP_Text resourceText;

    private float timer = 0f;
    private bool isBuildingPurchased = false;

    public GameObject woodPopupWindowPrefab;
    public GameObject stonePopupWindowPrefab;
    public GameObject coralPopupWindowPrefab;
    public GameObject foodPopupWindowPrefab;

    private GameObject woodPopupWindow;
    private GameObject stonePopupWindow;
    private GameObject coralPopupWindow;
    private GameObject foodPopupWindow;

    public PlayerFishController playerFishController;

    private void Start()
    {
        playerFishController = FindAnyObjectByType<PlayerFishController>();

        FindExistingPopupWindows();
        InstantiatePopupWindow(woodPopupWindowPrefab, ref woodPopupWindow);
        InstantiatePopupWindow(stonePopupWindowPrefab, ref stonePopupWindow);
        InstantiatePopupWindow(coralPopupWindowPrefab, ref coralPopupWindow);
        InstantiatePopupWindow(foodPopupWindowPrefab, ref foodPopupWindow);

        SetPopupWindowsActive(false);
    }

    private void UpdateUIResources()
    {
        if (resourceText != null)
        {
            resourceText.text = currentResources.ToString();
        }
    }

    public void UpdateResourceDisplay()
    {
        UpdateUIResources();
    }

    private void EnableButtonsAndInfo()
    {
        UpdateResourceDisplay();
    }

    private void FindExistingPopupWindows()
    {
        woodPopupWindow = GameObject.Find("WoodPopupWindow");
        stonePopupWindow = GameObject.Find("StonePopupWindow");
        coralPopupWindow = GameObject.Find("CoralPopupWindow");
        foodPopupWindow = GameObject.Find("FoodPopupWindow");
    }

    private void InstantiatePopupWindow(GameObject prefab, ref GameObject popupWindow)
    {
        if (prefab != null && popupWindow == null)
        {
            popupWindow = Instantiate(prefab, transform.position, Quaternion.identity);
            popupWindow.name = prefab.name;
            popupWindow.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isBuildingPurchased = true;

            if (gameObject.CompareTag("WoodBuilding") && woodPopupWindow != null)
                woodPopupWindow.SetActive(true);
            else if (gameObject.CompareTag("StoneBuilding") && stonePopupWindow != null)
                stonePopupWindow.SetActive(true);
            else if (gameObject.CompareTag("CoralBuilding") && coralPopupWindow != null)
                coralPopupWindow.SetActive(true);
            else if (gameObject.CompareTag("FoodBuilding") && foodPopupWindow != null)
                foodPopupWindow.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetPopupWindowsActive(false);
        }
    }

    private void SetPopupWindowsActive(bool active)
    {
        if (woodPopupWindow != null)
            woodPopupWindow.SetActive(active);

        if (stonePopupWindow != null)
            stonePopupWindow.SetActive(active);

        if (coralPopupWindow != null)
            coralPopupWindow.SetActive(active);

        if (foodPopupWindow != null)
            foodPopupWindow.SetActive(active);
    }

    private void Update()
    {
        if (isBuildingPurchased)
        {
            timer += Time.deltaTime;

            if (timer >= resourceGenerationInterval)
            {
                GenerateResources();
                timer = 0f;
            }
        }
    }

    private void GenerateResources()
    {
        currentResources += resourcesPerInterval;
        UpdateResourceDisplay(); // Make sure this method updates the UI text
        resourceText.text = currentResources.ToString(); // Update the UI text directly here
        Debug.Log("Generated resources: " + resourcesPerInterval);
    }
}
