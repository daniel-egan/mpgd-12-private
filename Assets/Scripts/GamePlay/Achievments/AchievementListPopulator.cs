using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class AchievementListPopulator : MonoBehaviour
{
    public GameObject contentParent; // The Content GameObject of the Scroll View
    public GameObject achievementPrefab; // The prefab for a single achievement

    void Start()
    {
        PopulateAchievements();
    }

    private void PopulateAchievements()
    {
        // Retrieve the list of achievements from CollectableManager
        List<CollectableItem> achievements = CollectableManager.Instance.CollectableItems;

        // Clear existing children under contentParent (if any)
        foreach (Transform child in contentParent.transform)
        {
            Destroy(child.gameObject);
        }

        // Iterate through each achievement and create a corresponding UI element
        foreach (var achievement in achievements)
        {
            // Instantiate a new achievement panel
            GameObject newAchievement = Instantiate(achievementPrefab, contentParent.transform);

            // Assign text components explicitly
            TextMeshProUGUI[] texts = newAchievement.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var text in texts)
            {
                if (text.gameObject.name == "Name")
                {
                    text.text = achievement.name; // Set the name of the achievement

                    if (achievement.isUnlocked)
                    {
                        Color textcolor = Color.black;
                        textcolor.a = 1f;
                        text.color = textcolor;
                    }
                }
                else if (text.gameObject.name == "Description")
                {
                    text.text = achievement.description; // Set the description

                    if (achievement.isUnlocked)
                    {
                        Color textcolor = Color.black;
                        textcolor.a = 1f;
                        text.color = textcolor;
                    }
                }

            }

            if (achievement.isUnlocked)
            {
                Image bg = newAchievement.GetComponent<Image>();
                bg.color = Color.white; // Change background color
            }

        }

    }
}