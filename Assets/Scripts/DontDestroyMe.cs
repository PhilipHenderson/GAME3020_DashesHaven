using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyMe : MonoBehaviour
{
    public string whatAmI;

    void Start()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(whatAmI); // Replace "YourTag" with the appropriate tag

        // Check if an object with this tag already exists in the scene
        if (objects.Length > 1)
        {
            // If an object already exists, destroy this instance
            Destroy(gameObject);
        }
        else
        {
            // If no other object exists, make this object persistent through scene changes
            DontDestroyOnLoad(gameObject);
        }
    }
}
