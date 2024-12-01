using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CollectableItem
{
    public string id;
    public string name;
    public string description;
    public bool isUnlocked;
}

[Serializable]
public class CollectableItemList
{
    public List<CollectableItem> collectables;
}

public class CollectableManager : MonoBehaviour
{
    public static CollectableManager Instance { get; private set; }
    public List<CollectableItem> CollectableItems { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        CollectableItems = GetCollectableItems();
        print(CollectableItems.Count);
    }

    private List<CollectableItem> GetCollectableItems()
    {
        string savedPlayerPrefs = PlayerPrefs.GetString("collectables", string.Empty);

        if (string.IsNullOrEmpty(savedPlayerPrefs))
        {
            print("LOADING FROM JSON");
            List<CollectableItem> jsonStored = LoadJsonFile();
            return jsonStored;
        }

        else
        {
            print("LOADING FROM PLAYERPREFS");
            List<CollectableItem> jsonStored = JsonUtility.FromJson<List<CollectableItem>>(savedPlayerPrefs);
            return jsonStored;
        }

    }

    private List<CollectableItem> LoadJsonFile()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("achievements");

        try
        {
            CollectableItemList jsonStored = JsonUtility.FromJson<CollectableItemList>(jsonFile.text);
            return jsonStored.collectables;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error parsing achievements.json: {e.Message}");
            return new List<CollectableItem>();
        }

    }

    private CollectableItem findItemById(string id)
    {
        CollectableItem element = CollectableItems.Find(element => element.id == id);
        return element;
    }

    public void UnlockCollectable(string id)
    {
        var collectable = findItemById(id);
        collectable.isUnlocked = true;
        SavePlayerPrefs();
    }

    private void SavePlayerPrefs()
    {
        var json = JsonUtility.ToJson(CollectableItems, true);
        PlayerPrefs.SetString("collectables", json);
        PlayerPrefs.Save();
    }
}