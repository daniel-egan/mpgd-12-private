using System;
using System.Collections.Generic;
using UnityEngine;

// Represents each individual collectable that can be unlocked
[Serializable]
public class CollectableItem
{
    public string id;
    public string name;
    public string description;
    public bool isUnlocked;
}

// Represents the list which is how the JSON must be represented
// Creating a list of each of the different collectables (CollectableItem)
[Serializable]
public class CollectableItemList
{
    public List<CollectableItem> collectables;
}

public class CollectableManager : MonoBehaviour
{
    // Creates the singleton instance property
    public static CollectableManager Instance { get; private set; }
    // Creates the list property which will hold all the collectables
    public List<CollectableItem> CollectableItems { get; private set; }

    private void Awake()
    {
        // Ensures the singleton remains, and there only is one instance within a game session
        if (Instance != null && Instance != this)
        {
            // Destroy the object because we don't want more than one instance
            Destroy(gameObject);
            return;
        }

        // Set the current instance as the singleton instance
        Instance = this;
        // Prevent the destruction of the gameObject across scenes
        DontDestroyOnLoad(gameObject);

        CollectableItems = GetCollectableItems();
    }

    private List<CollectableItem> GetCollectableItems()
    {
        // Get the PlayerPrefs, if it exists then use it, otherwise default to an empty string
        string savedPlayerPrefs = PlayerPrefs.GetString("collectables", string.Empty);

        // Check if the string is empty, meaning the PlayerPrefs are not saved
        if (string.IsNullOrEmpty(savedPlayerPrefs))
        {
            print("LOADING FROM JSON");
            List<CollectableItem> jsonStored = LoadJsonFile();
            return jsonStored;
        }

        else
        {
            print("LOADING FROM PLAYERPREFS");
            // Create an object from the JSON stored within the PlayerPrefs
            CollectableItemList jsonStored = JsonUtility.FromJson<CollectableItemList>(savedPlayerPrefs);
            // Return the object within as we only care about the List of CollectableItem, not the wrapper class around it
            return jsonStored.collectables;
        }

    }

    private List<CollectableItem> LoadJsonFile()
    {
        // Look in the Resources folder and find the achievements.json file, loading it
        TextAsset jsonFile = Resources.Load<TextAsset>("achievements");

        try
        {
            // Using the string of the text from the JSON file, place that into the wrapper class
            CollectableItemList jsonStored = JsonUtility.FromJson<CollectableItemList>(jsonFile.text);
            // Return the List within the wrapper class
            return jsonStored.collectables;
        }
        // If something goes wrong with the loading of the JSON, then print out the error message
        catch (Exception e)
        {
            Debug.LogError($"Error parsing achievements.json: {e.Message}");
            return new List<CollectableItem>();
        }

    }

    private CollectableItem FindItemById(string id)
    {
        // Go through the list of collectables and find the element which has the matching id
        CollectableItem element = CollectableItems.Find(element => element.id == id);
        return element;
    }

    public void UnlockCollectable(string id)
    {
        var collectable = FindItemById(id);

        if (collectable == null)
        {
            print($"NO COLLECTABLE WITH ID: {id}");
        }
        else
        {
            if (collectable.isUnlocked)
            {
                print("ALREADY UNLOCKED");
                return;
            }

            // Change the field of the collectable to be unlocked being true
            collectable.isUnlocked = true;
            SavePlayerPrefs();

        }


    }

    private void SavePlayerPrefs()
    {
        // Create a new wrapper object which has all the collectables as a field
        CollectableItemList itemList = new CollectableItemList { collectables = CollectableItems };
        // Convert the wrapper into a JSON string
        var json = JsonUtility.ToJson(itemList, true);
        // Save to the PlayerPrefs
        PlayerPrefs.SetString("collectables", json);
        PlayerPrefs.Save();
    }
}