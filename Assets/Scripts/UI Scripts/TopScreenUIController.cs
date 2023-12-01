using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TopScreenUIController : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI rocksText;
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI coralText;

    public void UpdateHPUI(int hp)
    {
        hpText.text = "HP: " + hp;
    }

    public void UpdateEnergyUI(int energy)
    {
        energyText.text = "Energy: " + energy;
    }

    public void UpdateFoodUI(int food)
    {
        foodText.text = "Food: " + food;
    }

    public void UpdateStoneUI(int stone)
    {
        rocksText.text = "Stone: " + stone;
    }

    public void UpdateWoodUI(int wood)
    {
        woodText.text = "Wood: " + wood;
    }

    public void UpdateCoralUI(int coral)
    {
        coralText.text = "Coral: " + coral;
    }
}
