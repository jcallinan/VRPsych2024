using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance; // Singleton pattern

    public string selectedScript; // To store the choice

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Makes sure this object persists when loading a new scene
        }
        else
        {
            Destroy(gameObject); // Ensures there's only one instance of this object
        }
    }
}
