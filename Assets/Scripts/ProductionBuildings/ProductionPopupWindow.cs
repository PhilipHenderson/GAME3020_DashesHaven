using UnityEngine;
using UnityEngine.UI;

public enum ProductionBuildingType
{
    Wood,
    Stone,
    Coral,
    Food
}

public class ProductionPopupWindow : MonoBehaviour
{
    public ProductionBuildingType buildingType;

    public Button purchaseButton;
    public Button collectButton;
    public Button upgradeButton;

    public GameObject infoPanel1;
    public GameObject infoPanel2;

    private CityMapGenerator cityMapGenerator;
    public ProductionBuilding productionBuilding;
    public ProductionBuilding[] productionBuildingList;

    private void Start()
    {
        productionBuildingList = GameObject.FindObjectsOfType<ProductionBuilding>();

        purchaseButton.onClick.AddListener(Purchase);
        collectButton.onClick.AddListener(Collect);

        // Find the CityMapGenerator script in the scene
        cityMapGenerator = FindObjectOfType<CityMapGenerator>();

        // Check the bool in CityMapGenerator and set buttons and panels active accordingly
        if (cityMapGenerator != null)
        {
            bool buildingPurchased = false;

            // Check the specific bool based on the building type
            switch (buildingType)
            {
                case ProductionBuildingType.Wood:
                    buildingPurchased = cityMapGenerator.woodProductionPurchased;
                    break;
                case ProductionBuildingType.Stone:
                    buildingPurchased = cityMapGenerator.stoneProductionPurchased;
                    break;
                case ProductionBuildingType.Coral:
                    buildingPurchased = cityMapGenerator.coralProductionPurchased;
                    break;
                case ProductionBuildingType.Food:
                    buildingPurchased = cityMapGenerator.foodProductionPurchased;
                    break;
            }

            if (buildingPurchased)
            {
                EnableButtonsAndInfo();
            }
            else
            {
                DisableButtonsAndInfo();
            }
        }
    }

    private void Purchase()
    {
        // Logic for purchasing the production building

        if (cityMapGenerator != null)
        {
            switch (buildingType)
            {
                case ProductionBuildingType.Wood:
                    cityMapGenerator.woodProductionPurchased = true;
                    break;
                case ProductionBuildingType.Stone:
                    cityMapGenerator.stoneProductionPurchased = true;
                    break;
                case ProductionBuildingType.Coral:
                    cityMapGenerator.coralProductionPurchased = true;
                    break;
                case ProductionBuildingType.Food:
                    cityMapGenerator.foodProductionPurchased = true;
                    break;
            }
            EnableButtonsAndInfo();
        }
    }

    public void Collect()
    {
        foreach (ProductionBuilding building in productionBuildingList)
        {
            if (building != null)
            {
                if (building.gameObject.CompareTag("WoodBuilding"))
                {
                    PlayerFishController.Instance.Wood += building.currentResources;
                    Debug.Log("Collected resources: " + building.currentResources);
                    building.currentResources = 0;
                    building.UpdateResourceDisplay(); // Assuming this method updates the UI text
                }
                else if (building.gameObject.CompareTag("StoneBuilding"))
                {
                    PlayerFishController.Instance.Stone += building.currentResources;
                    Debug.Log("Collected resources: " + building.currentResources);
                    building.currentResources = 0;
                    building.UpdateResourceDisplay(); // Assuming this method updates the UI text
                }
                else if (building.gameObject.CompareTag("CoralBuilding"))
                {
                    PlayerFishController.Instance.Coral += building.currentResources;
                    Debug.Log("Collected resources: " + building.currentResources);
                    building.currentResources = 0;
                    building.UpdateResourceDisplay(); // Assuming this method updates the UI text
                }
                else if (building.gameObject.CompareTag("FoodBuilding"))
                {
                    PlayerFishController.Instance.Food += building.currentResources;
                    Debug.Log("Collected resources: " + building.currentResources);
                    building.currentResources = 0;
                    building.UpdateResourceDisplay(); // Assuming this method updates the UI text
                }
            }
        }
    }


    private void EnableButtonsAndInfo()
    {
        // Enable buttons and info panels
        collectButton.interactable = true;
        upgradeButton.interactable = true;
        infoPanel1.SetActive(true);
        infoPanel2.SetActive(true);
    }

    private void DisableButtonsAndInfo()
    {
        // Disable buttons and info panels
        collectButton.interactable = false;
        upgradeButton.interactable = false;
        infoPanel1.SetActive(false);
        infoPanel2.SetActive(false);
    }
}
