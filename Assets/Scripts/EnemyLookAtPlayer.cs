using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLookAtPlayer : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // LookAt the player with a +90 degrees offset
        Vector3 directionToPlayer = player.transform.position - transform.position;

        // Create the LookAt rotation
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

        // Apply the offset of +90 degrees around the desired axis (e.g., Y-axis)
        Quaternion offsetRotation = Quaternion.Euler(0, 90, 0);

        // Combine the rotations
        transform.rotation = lookRotation * offsetRotation;


    }
}
