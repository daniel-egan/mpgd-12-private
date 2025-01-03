using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            // Change the field of the collectable to be unlocked being true
            GameObject levelCanvas = GameObject.Find("Canvas");

            // Create a panel for the background
            GameObject panelGameObject = new GameObject("AchievementPanel");
            panelGameObject.transform.SetParent(levelCanvas.transform);

            RectTransform panelRectTransform = panelGameObject.AddComponent<RectTransform>();
            panelRectTransform.anchorMin = new Vector2(0.5f, 1f); // Top center
            panelRectTransform.anchorMax = new Vector2(0.5f, 1f); // Top center
            panelRectTransform.pivot = new Vector2(0.5f, 1f);
            panelRectTransform.anchoredPosition = new Vector2(0, 100); // Start off-screen
            panelRectTransform.sizeDelta = new Vector2(500, 120); // Panel size

            // Add an Image component to the panel for a background
            Image panelImage = panelGameObject.AddComponent<Image>();
            panelImage.color = new Color(204f / 255f, 213f / 255f, 227f / 255f, 1f);

            // Create the text as a child of the panel
            GameObject textGameObject = new GameObject("AchievementText");
            textGameObject.transform.SetParent(panelGameObject.transform);

            TextMeshProUGUI achievementTextMeshPro = textGameObject.AddComponent<TextMeshProUGUI>();

            if (collectable.isUnlocked)
            {
                achievementTextMeshPro.text = $"Achievement already unlocked!";
            }
            else
            {
                achievementTextMeshPro.text = $"You unlocked achievement: {collectable.name}";
                collectable.isUnlocked = true;
                SavePlayerPrefs();
            }

            achievementTextMeshPro.alignment = TextAlignmentOptions.Center;
            achievementTextMeshPro.fontSize = 36;
            achievementTextMeshPro.color = Color.white;

            // Configure text RectTransform
            RectTransform textRectTransform = textGameObject.GetComponent<RectTransform>();
            textRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            textRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            textRectTransform.pivot = new Vector2(0.5f, 0.5f);
            textRectTransform.anchoredPosition = Vector2.zero; // Centered in the panel
            textRectTransform.sizeDelta = new Vector2(480, 100); // Size within the panel

            // Start the animation coroutine
            StartCoroutine(SwipePanelCoroutine(panelRectTransform, panelGameObject));
        }
    }

    IEnumerator SwipePanelCoroutine(RectTransform panelRectTransform, GameObject panelGameObject)
    {
        float duration = 0.5f; // Swipe animation duration
        float waitTime = 2f;  // Time the text stays visible
        Vector2 startPos = panelRectTransform.anchoredPosition;        // Start off-screen
        Vector2 midPos = new Vector2(0, -50);                          // Visible position
        Vector2 endPos = startPos;                                     // Swipe back up

        // Animate swipe down
        yield return StartCoroutine(AnimatePosition(panelRectTransform, startPos, midPos, duration));

        // Wait while visible
        yield return new WaitForSeconds(waitTime);

        // Animate swipe back up
        yield return StartCoroutine(AnimatePosition(panelRectTransform, midPos, endPos, duration));

        // Destroy the panel and text
        Destroy(panelGameObject);
    }

    IEnumerator AnimatePosition(RectTransform rectTransform, Vector2 from, Vector2 to, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(from, to, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = to;
    }

    public bool AreAllTanksCollected()
    {
        var oxygenPickup1 = FindItemById("oxygen_pickup1");
        var oxygenPickup2 = FindItemById("oxygen_pickup2");

        // Ensure both pickups are unlocked
        return oxygenPickup1?.isUnlocked == true && oxygenPickup2?.isUnlocked == true;
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