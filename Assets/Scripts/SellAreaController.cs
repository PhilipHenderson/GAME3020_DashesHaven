using UnityEngine;
using UnityEngine.UI;

public class SellAreaController : MonoBehaviour
{
    public GameObject playerObject; // Reference to the player GameObject.
    public GameObject popUpWindow; // Reference to your UI pop-up window.
    public float interactionRange = 2.0f; // Adjust this range as needed.

    public bool playerInRange; // Define playerInRange here.

    public PlayerFishController playerFishController;
    public int currentWood;
    public int currentStone;

    public int DisplayWood { set { currentWood = playerFishController.Wood; } }
    public int DisplayStone;


    private void Start()
    {
        playerFishController = FindAnyObjectByType<PlayerFishController>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
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
    }

    public void SellStone()
    {
        playerFishController.Stone = 0;
    }

    public void SellCoral()
    {

    }

    // Add any other functionalities or interactions here.
}
