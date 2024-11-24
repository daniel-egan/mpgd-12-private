using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab; // NPC prefab to spawn
    public Vector2 spawnAreaSize = new Vector2(10, 10); // Size of the spawn area
    public int numberOfNPCs = 5; // Number of NPCs to spawn
    public Transform spawnAreaCenter; // Center of the spawn area
    public float spawnCheckRadius = 0.5f; // Radius to check for collisions

    void Start()
    {
        SpawnNPCs();
    }

    void SpawnNPCs()
    {
        for (int i = 0; i < numberOfNPCs; i++)
        {
            Vector3 spawnPosition = GetValidSpawnPosition();
            Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
        }
    }

    Vector3 GetValidSpawnPosition()
    {
        Vector3 randomPosition;

        // Retry until a valid position is found
        do
        {
            randomPosition = GetRandomPosition();
        }
        while (IsPositionOccupied(randomPosition));

        return randomPosition;
    }

    Vector3 GetRandomPosition()
    {
        // Generate random X and Z within the spawn area bounds
        float randomX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float randomZ = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);

        // Return the position relative to the spawn area center
        return new Vector3(randomX, 0, randomZ) + spawnAreaCenter.position;
    }

    bool IsPositionOccupied(Vector3 position)
    {
        // Check for overlapping objects within the specified radius
        Collider[] colliders = Physics.OverlapSphere(position, spawnCheckRadius);
        return colliders.Length > 0;
    }
}
