// Code used from CHATGPT
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab; 
    public Vector2 spawnAreaSize = new Vector2(10, 10); 
    public int numberOfNPCs = 5; 
    public Transform spawnAreaCenter;

    void Start()
    {
        SpawnNPCs();
    }


    // Spawns the npc prehabs at the start of the level
    // valid spawn positions are first retrieved to ensure no npc is colliding with anything
    void SpawnNPCs()
    {
        for (int i = 0; i < numberOfNPCs; i++)
        {
            Vector3 spawnPosition = GetValidSpawnPosition();
            Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
        }
    }

    // Function to return valid vector spawn position
    Vector3 GetValidSpawnPosition()
    {
        Vector3 randomPosition;
        randomPosition = GetRandomPosition();
        return randomPosition;
    }


    // Function to randomly generate coordinates from bounds for a random position for the prehab
    Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float randomZ = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);
        return new Vector3(randomX, 0, randomZ) + spawnAreaCenter.position;
    }
}
