using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public string id;

    // private void UnlockCollectable()
    // {
    //     // PlayerPrefs.SetInt(id, 1); // Means the achievement is now unlocked
    //     // PlayerPrefs.Save();
    //     // gameObject.SetActive(false);
    //     
    // }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name != "Player") return;

        CollectableManager.Instance.UnlockCollectable(id);
        gameObject.SetActive(false);
    }
}