using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileBehavior : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 direction;
    public Canvas splashScreen;
    public GameObject DestroyableWall;


    // Projectiles initial direction is towards the player
    public void Initialize(Transform target)
    {
        direction = (target.position - transform.position).normalized;
    }

    void Start()
    {
    }
    // Projectiles direction is towards the player
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    // If the projectile hits the player the splash scree will be activated
    // If the projectile hits the wall it destroys the wall
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            splashScreen.gameObject.SetActive(true);
        }
        if (other.CompareTag("DestroyWall"))
        {
            Debug.Log("Hit wall");
            Destroy(DestroyableWall);
        }
    }
}



