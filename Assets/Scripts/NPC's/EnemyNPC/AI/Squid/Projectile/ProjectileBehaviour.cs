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

    public void Initialize(Transform target)
    {
        direction = (target.position - transform.position).normalized;
    }

    void Start()
    {
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

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



