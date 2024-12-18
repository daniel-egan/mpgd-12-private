using UnityEngine;

public class SchoolManager : MonoBehaviour
{
    public GameObject fishPrefab;
    public int fishCount = 100;
    public Vector3 spawnBounds = new Vector3(50, 50, 50);

    void Start()
    {
        for (int i = 0; i < fishCount; i++)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(-spawnBounds.x, spawnBounds.x),
                Random.Range(-spawnBounds.y, spawnBounds.y),
                Random.Range(-spawnBounds.z, spawnBounds.z)
            );

            Instantiate(fishPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
