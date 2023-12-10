using UnityEngine;
using UnityEngine.UI;

public class ButtonScript2 : MonoBehaviour
{
    public InputField amountInputField; // Reference to the input field for amount

    public Button withAll;
    public Button DepoAll;
    public Button With;
    public Button Depo;

    private BankChestScript bankChestScript;
    private PlayerFishController playerFishController;
    private SellAreaController sellAreaController;

    private void Start()
    {
        // Find the necessary controllers or scripts
        playerFishController = FindObjectOfType<PlayerFishController>();
        bankChestScript = FindObjectOfType<BankChestScript>();

        // Add listeners to buttons
        withAll.onClick.AddListener(WithdrawAll);
        DepoAll.onClick.AddListener(DepositAll);
        With.onClick.AddListener(Withdraw);
        Depo.onClick.AddListener(Deposit);
    }

    private void Update()
    {
        bankChestScript = FindObjectOfType<BankChestScript>();
    }

    public void WithdrawAll()
    {
        bankChestScript.WithdrawAllMoney();
    }

    public void DepositAll()
    {
        bankChestScript.DepositAllMoney();
    }

    public void Withdraw()
    {
        if (amountInputField != null)
        {
            int amount = int.Parse(amountInputField.text);
            bankChestScript.WithdrawMoney(amount);
        }
        else
        {
            Debug.LogError("Amount Input Field is not assigned!");
        }
    }

    public void Deposit()
    {
        if (amountInputField != null)
        {
            int amount = int.Parse(amountInputField.text);
            bankChestScript.DepositMoney(amount);
        }
        else
        {
            Debug.LogError("Amount Input Field is not assigned!");
        }
    }
}
