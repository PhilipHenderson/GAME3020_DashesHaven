using UnityEngine;
using UnityEngine.UI;

public class SellAreaController : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject popupWindow; 
    public GameObject topScreenUIController;

    public bool playerInRange;
    public float interactionRange = 4.0f;

    public PlayerFishController playerFishController;

    public int currentWood;
    public int currentStone;
    public int currentCoral;
    public int currentMoney;

    public int woodCost;
    public int stoneCost;
    public int coralCost;
    public int foodCost;

    private void Awake()
    {
        playerFishController = FindObjectOfType<PlayerFishController>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
        popupWindow = GameObject.FindGameObjectWithTag("PopupWindow");
    }

    private void Start()
    {
        topScreenUIController = GameObject.FindGameObjectWithTag("TopScreenUI");
        popupWindow.SetActive(false);
    }

    private void Update()
    {
        if (playerObject != null)
        {
            float distance = Vector3.Distance(playerObject.transform.position, transform.position);
            //Debug.Log("Distance: " + distance);
            if (distance <= interactionRange)
            {
                // Player is in range
                playerInRange = true;
                //Debug.Log("Player In Range");
                if (popupWindow != null)
                {
                    popupWindow.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("Pop-up Window reference is null1.");
                }
            }
            else
            {
                // Player is out of range
                playerInRange = false;
                if (popupWindow != null)
                {
                    popupWindow.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("Pop-up Window reference is null2.");
                }
            }
        }
        else
        {
            Debug.LogWarning("Player reference is null3.");
        }
    }
    public void FixedUpdate()
    {
        currentWood = playerFishController.Wood;
        currentStone = playerFishController.Stone;
        currentCoral = playerFishController.Coral;
        currentMoney = playerFishController.Money;
    }

    public bool IsPopUpWindowOpen()
    {
        // Return the state of the pop-up window (active or inactive)
        if (popupWindow != null)
        {
            return popupWindow.activeSelf;
        }
        else { return false; }
    }

    public void SellWood()
    {
        if (playerFishController.Wood >= 2)
        {
            Debug.Log("Pressing Sell Wood Button");
            playerFishController.Money += currentWood * 2;
            playerFishController.Wood = 0;
        }
        else
        {
            Debug.Log("Not Enough Wood");
        }

        playerFishController.topScreenUIController.UpdateWoodUI(currentWood);
        playerFishController.topScreenUIController.UpdateMoneyUI(currentMoney);
    }

    public void BuyWood()
    {
        if (playerFishController.Money >= 10)
        {
            Debug.Log("Pressing Buy Wood Button");
            playerFishController.Wood += 2;
            playerFishController.Money -= woodCost;
            currentWood = playerFishController.Wood;
        }
        else
        {
            Debug.Log("Not Enough Money");
        }
        playerFishController.topScreenUIController.UpdateWoodUI(currentWood);
        playerFishController.topScreenUIController.UpdateMoneyUI(currentMoney);
    }

    public void SellStone()
    {
        if (playerFishController.Stone >= stoneCost)
        {
            Debug.Log("Pressing Sell Stone Button");
            playerFishController.Money += currentStone * 2;
            playerFishController.Stone = 0;
        }
        else
        {
            Debug.Log("Not Enough Stone");
        }

        playerFishController.topScreenUIController.UpdateStoneUI(currentStone);
        playerFishController.topScreenUIController.UpdateMoneyUI(currentMoney);
    }

    public void BuyStone()
    {
        if (playerFishController.Money >= stoneCost)
        {
            Debug.Log("Pressing Buy Stone Button");
            playerFishController.Stone += 10;
            playerFishController.Money -= stoneCost;
            currentStone = playerFishController.Stone;
        }
        else
        {
            Debug.Log("Not Enough Money");
        }
        playerFishController.topScreenUIController.UpdateStoneUI(currentStone);
        playerFishController.topScreenUIController.UpdateMoneyUI(currentMoney);
    }

    public void SellCoral()
    {
        if (playerFishController.Coral >= 2)
        {
            Debug.Log("Pressing Sell Coral Button");
            playerFishController.Money += currentCoral * 2;
            playerFishController.Coral = 0;
        }
        else
        {
            Debug.Log("Not Enough Coral");
        }

        playerFishController.topScreenUIController.UpdateCoralUI(currentCoral);
        playerFishController.topScreenUIController.UpdateMoneyUI(currentMoney);
    }

    public void BuyCoral()
    {
        if (playerFishController.Money >= coralCost)
        {
            Debug.Log("Pressing Buy Coral Button");
            playerFishController.Coral += 10;
            playerFishController.Money -= coralCost;
            currentCoral = playerFishController.Coral;
        }
        else
        {
            Debug.Log("Not Enough Money");
        }
        playerFishController.topScreenUIController.UpdateCoralUI(currentCoral);
        playerFishController.topScreenUIController.UpdateMoneyUI(currentMoney);
    }

    // Add any other functionalities or interactions here.
}