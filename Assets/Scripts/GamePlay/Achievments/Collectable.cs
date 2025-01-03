using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public string id;

    private void OnCollisionEnter(Collision other)
    {
        // Makes sure that the player is the only thing that unlocks the achievement
        // For example an NPC fish bumping into the gameObject will not trigger it
        if (other.gameObject.name != "Player") return;

        // Will call into the CollectableManager singleton and unlock the collectable
        CollectableManager.Instance.UnlockCollectable(id);
        gameObject.SetActive(false);
    }
}