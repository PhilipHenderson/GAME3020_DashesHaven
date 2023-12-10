using UnityEngine;

public class BankInteraction : MonoBehaviour
{
    public BankChestScript bankChestScript;

    private void Start()
    {
        bankChestScript = FindObjectOfType<BankChestScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bankChestScript.bankPopupWindow.SetActive(true); // Open the bank's popup window
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bankChestScript.bankPopupWindow.SetActive(false); // Close the bank's popup window
        }
    }
}
