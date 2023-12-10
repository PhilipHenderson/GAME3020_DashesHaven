using UnityEngine;

public class BankChestScript : MonoBehaviour
{
    public int storedMoney = 0; // Amount of money stored in the bank
    public GameObject bankPopupWindow;

    private void Start()
    {
        bankPopupWindow = GameObject.FindGameObjectWithTag("BankPopupWindow");
        if (bankPopupWindow != null)
        {
            bankPopupWindow.SetActive(false); // Set bank popup window inactive at the start
        }
    }

    public void DepositMoney(int amount)
    {
        PlayerFishController player = FindObjectOfType<PlayerFishController>();
        if (player != null)
        {
            if (amount > 0 && player.Money >= amount)
            {
                player.Money -= amount; // Subtract the deposited amount from the player's money
                storedMoney += amount; // Add the deposited amount to the bank's stored money
            }
        }
    }

    public void DepositAllMoney()
    {
        PlayerFishController player = FindObjectOfType<PlayerFishController>();
        if (player != null && player.Money > 0)
        {
            storedMoney += player.Money; // Deposit all player's money into the bank
            player.Money = 0; // Set player's money to zero after depositing all
        }
    }

    public void WithdrawMoney(int amount)
    {
        PlayerFishController player = FindObjectOfType<PlayerFishController>();
        if (player != null)
        {
            if (amount > 0 && storedMoney >= amount)
            {
                storedMoney -= amount; // Subtract the withdrawn amount from the bank's stored money
                player.Money += amount; // Add the withdrawn amount to the player's money
            }
        }
    }

    public void WithdrawAllMoney()
    {
        PlayerFishController player = FindObjectOfType<PlayerFishController>();
        if (player != null && storedMoney > 0)
        {
            player.Money += storedMoney; // Withdraw all stored money from the bank to the player
            storedMoney = 0; // Set bank's stored money to zero after withdrawal
        }
    }

    // Other methods and Start function as previously defined...
}
