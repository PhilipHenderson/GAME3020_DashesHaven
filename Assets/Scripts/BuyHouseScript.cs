using UnityEngine;
using UnityEngine.UI;

public class BuyHouseScript : MonoBehaviour
{
    public GameObject housePopupWindow1;
    public GameObject housePopupWindow2;
    public GameObject housePopupWindow3;

    CityMapGenerator mapGenerator;

    private void Awake()
    {
        if (housePopupWindow1 == null || housePopupWindow2 == null || housePopupWindow3 == null)
        {
            Debug.LogError("One or more popup windows not found!");
        }

        mapGenerator = FindAnyObjectByType<CityMapGenerator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("BuyPad1"))
            {
                OpenBuyHousePopup(mapGenerator.popup1);
            }
            else if (gameObject.CompareTag("BuyPad2"))
            {
                OpenBuyHousePopup(mapGenerator.popup2);
            }
            else if (gameObject.CompareTag("BuyPad3"))
            {
                OpenBuyHousePopup(mapGenerator.popup3);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CloseAllBuyHousePopups();
        }
    }

    void OpenBuyHousePopup(GameObject housePopupWindow)
    {
        housePopupWindow.SetActive(true);
        // Add any other logic when opening the popup window
    }

    void CloseAllBuyHousePopups()
    {
        mapGenerator.popup1.SetActive(false);
        mapGenerator.popup2.SetActive(false);
        mapGenerator.popup3.SetActive(false);
        // Add any other logic when closing the popup window
    }

    public void BuyHouse1()
    {
        mapGenerator.house1Purchased = true;
        mapGenerator.spawnPlotsAndHouses();
        Debug.Log("BuyHouse1 Pressed");
    }

    public void BuyHouse2()
    {
        mapGenerator.house2Purchased = true;
        mapGenerator.spawnPlotsAndHouses();
        Debug.Log("BuyHouse2 Pressed");
    }

    public void BuyHouse3()
    {
        mapGenerator.house3Purchased = true;
        mapGenerator.spawnPlotsAndHouses();
        Debug.Log("BuyHouse3 Pressed");
    }
}
