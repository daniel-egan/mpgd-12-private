// Code used from CHATGPT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAiming : MonoBehaviour
{
    public Transform player;
    public Transform firePoint;
    public GameObject projectilePrefab;
    public Canvas splashScreen;

    public float attackRange = 10f;

    // When the player is within a certain distance from them enemy the shoot function is called
    void Update()
    {
        if (player != null)
        {
            Vector2 direction = player.position - firePoint.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(0, 0, angle);
            if (Vector3.Distance(player.position, firePoint.position) <= attackRange)
            {
                Shoot();
            }
        }

        if (GameObject.Find("Splash") != null)
        {
            StartCoroutine(DisableSplashScreenAfterDelay(2f));
        }
    }
    // The shoot function initializes and calls the Projectile prehab
    // This prehab is shot toawards the player like 'ink'
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

    // Timer to remove the splash screen off the screen
    private IEnumerator DisableSplashScreenAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        splashScreen.gameObject.SetActive(false);
    }
}
