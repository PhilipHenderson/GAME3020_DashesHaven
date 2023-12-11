using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Button button3;

    BuyHouseScript buyHouseScript;
    PlayerFishController playerFishController;

    private void Start()
    {
        playerFishController = FindObjectOfType<PlayerFishController>();

        button1.onClick.AddListener(BuyHouse1);
        button2.onClick.AddListener(BuyHouse2);
        button3.onClick.AddListener(BuyHouse3);
    }

    private void Update()
    {
        buyHouseScript = FindObjectOfType<BuyHouseScript>();
    }

    public void BuyHouse1()
    {
        if (playerFishController.Money >= 1000)
        {
            buyHouseScript.BuyHouse1();
            playerFishController.Money -= 1000;
            Debug.Log("BuyHouse1 Pressed");
        }
        else
            Debug.Log("Not Enough Money");
    }

        public void BuyHouse2()
    {
        if (playerFishController.Money >= 5000)
        {
            buyHouseScript.BuyHouse2();
            playerFishController.Money -= 5000;
            Debug.Log("BuyHouse2 Pressed");
        }
        else
            Debug.Log("Not Enough Money");
    }

    public void BuyHouse3()
    {
        if (playerFishController.Money >= 15000)
        {
            buyHouseScript.BuyHouse3();
            playerFishController.Money -= 15000;
            Debug.Log("BuyHouse3 Pressed");
        }
        else
            Debug.Log("Not Enough Money");
    }
}
