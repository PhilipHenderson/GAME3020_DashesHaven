using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoBar : MonoBehaviour
{
    public TMP_Text itemInfoText;

    public void DisplayItemInfo(string itemName, string itemDescription, int itemCost, float distanceFromPlayer, string extraInfo = "")
    {
        string formattedInfo = "Name: " + itemName +
                               "\nDescription: " + itemDescription +
                               "\nCost: " + itemCost.ToString() +
                               "\nDistance from Player: " + distanceFromPlayer.ToString("F2") + " units";

        // If there's additional info, append it
        if (!string.IsNullOrEmpty(extraInfo))
        {
            formattedInfo += "\n" + extraInfo;
        }

        itemInfoText.text = formattedInfo;
    }

    public void ClearItemInfo()
    {
        itemInfoText.text = "";
    }
}
