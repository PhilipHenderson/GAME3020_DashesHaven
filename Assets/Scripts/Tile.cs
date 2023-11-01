using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public void DestroyPickups()
    {
        Pickup[] pickups = GetComponentsInChildren<Pickup>();
        foreach (Pickup pickup in pickups)
        {
            if(pickup.isCollidingWithPlayer == true)
                Destroy(pickup.gameObject);
        }
    }
}
