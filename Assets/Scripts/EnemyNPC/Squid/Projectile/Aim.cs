using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAiming : MonoBehaviour
{
    public Transform player;
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float fireRate = 1f;
    private float nextFireTime;

    // Only shoot if the player is within attack range
    public float attackRange = 10f;

    void Update()
    {
        if (player != null)
        {
            // Calculate the direction to the player
            Vector2 direction = player.position - firePoint.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(0, 0, angle);

            // Only shoot if the player is within attack range
            if (Vector3.Distance(player.position, firePoint.position) <= attackRange)
            {
                if (Time.time > nextFireTime)
                {
                    Shoot();
                    nextFireTime = Time.time + fireRate;
                }
            }
            
        }
    }

    public void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        ProjectileBehavior projectileBehavior = projectile.GetComponent<ProjectileBehavior>();

        if (projectileBehavior != null && player != null)
        {
            projectileBehavior.Initialize(player);
        }
        Destroy(projectile, 2f);
    }
}

