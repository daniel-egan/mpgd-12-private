using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosivePufferFish : MonoBehaviour
{
    public float triggerRadius = 5f; // Detection range
    public float explosionForce = 500f;
    public float explosionRadius = 10f;
    public float damage = 50f;
    public GameObject explosionEffect;

    private bool isTriggered = false;

    void Start()
    {
        // Optional: Add a detection trigger
        SphereCollider trigger = gameObject.AddComponent<SphereCollider>();
        trigger.radius = triggerRadius;
        trigger.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isTriggered && other.CompareTag("Player"))
        {
            isTriggered = true;
            StartCoroutine(InflateAndExplode());
        }
    }

    private IEnumerator InflateAndExplode()
    {
        // Inflate Animation
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Inflate");
        }

        // Wait for the animation (adjust time as per animation length)
        yield return new WaitForSeconds(2f);

        // Explosion Logic
        Explode();
    }

    private void Explode()
    {
        // Spawn explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Apply force to nearby objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            // Optional: Deal damage
            if (hit.CompareTag("Player"))
            {
                // Add damage logic here
                Debug.Log("Player takes " + damage + " damage!");
            }
        }

        // Destroy the puffer fish
        Destroy(gameObject);
    }
}
