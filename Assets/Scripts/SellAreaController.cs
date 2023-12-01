using UnityEngine;
using UnityEngine.UI;

public class SellAreaController : MonoBehaviour
{
    public GameObject playerObject; // Reference to the player GameObject.
    public GameObject popUpWindow; // Reference to your UI pop-up window.
    public float interactionRange = 2.0f; // Adjust this range as needed.

    public bool playerInRange; // Define playerInRange here.

    public PlayerFishController playerFishController;
    public GameObject topScreenUIController;
    public int currentWood;
    public int currentStone;
    public int currentCoral;

    private void Awake()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");

    }
    private void Start()
    {
        topScreenUIController = GameObject.FindGameObjectWithTag("TopScreenUI");
        playerFishController = playerObject.GetComponent<PlayerFishController>();
        if (popUpWindow != null)
        {
            popUpWindow.SetActive(false);
        }
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
                if (popUpWindow != null)
                {
                    popUpWindow.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("Pop-up Window reference is null.");
                }
            }
            else
            {
                // Player is out of range
                playerInRange = false;
                if (popUpWindow != null)
                {
                    popUpWindow.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("Pop-up Window reference is null.");
                }
            }
        }
        else
        {
            Debug.LogWarning("Player reference is null.");
        }
    }
    public void FixedUpdate()
    {
        currentWood = playerFishController.Wood;
        currentStone = playerFishController.Stone;
        currentCoral = playerFishController.Coral;
    }

    public bool IsPopUpWindowOpen()
    {
        // Return the state of the pop-up window (active or inactive)
        if (popUpWindow != null)
        {
            return popUpWindow.activeSelf;
        }
        else { return false; }
    }

    public void SellWood()
    {
        Debug.Log("Pressing Sell Wood Button");
        playerFishController.Wood = 0;
        currentWood = playerFishController.Wood;
        playerFishController.topScreenUIController.UpdateWoodUI(currentWood);
    }

    public void BuyWood()
    {
        Debug.Log("Pressing Buy Wood Button");
        playerFishController.Wood += 10;
        currentWood = playerFishController.Wood;
        playerFishController.topScreenUIController.UpdateWoodUI(currentWood);
    }

    public void SellStone()
    {
        Debug.Log("Pressing Sell Stone Button");
        playerFishController.Stone = 0;
        currentStone = playerFishController.Stone;
        playerFishController.topScreenUIController.UpdateStoneUI(currentStone);
    }

    public void BuyStone()
    {
        Debug.Log("Pressing Buy Stone Button");
        playerFishController.Stone += 10;
        currentStone = playerFishController.Stone;
        playerFishController.topScreenUIController.UpdateStoneUI(currentStone);
    }

    public void SellCoral()
    {
        Debug.Log("Pressing Sell Coral Button");
        playerFishController.Coral = 0;
        currentCoral = playerFishController.Coral;
        playerFishController.topScreenUIController.UpdateCoralUI(currentCoral);
    }

    public void BuyCoral()
    {
        Debug.Log("Pressing Buy Coral Button");
        playerFishController.Coral += 10;
        currentCoral = playerFishController.Coral;
        playerFishController.topScreenUIController.UpdateCoralUI(currentCoral);
    }

    // Add any other functionalities or interactions here.
}