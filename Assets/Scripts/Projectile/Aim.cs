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

    void Update()
    {
        if (player != null)
        {
            Vector2 direction = player.position - firePoint.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(0, 0, angle);

            if (Time.time > nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        ProjectileBehavior projectileBehavior = projectile.GetComponent<ProjectileBehavior>();

        if (projectileBehavior != null && player != null)
        {
            projectileBehavior.Initialize(player); // Pass the player's Transform as the target
        }
    }

}

