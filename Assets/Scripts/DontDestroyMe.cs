using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyMe : MonoBehaviour
{
    public string whatAmI;

    void Start()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(whatAmI); 

        if (objects.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
