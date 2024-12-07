using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // For accessing UI components

public class ProjectileBehavior : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 direction;
    public Canvas splashScreen;

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
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(WaitAndDeactivateSplashScreen(2f));
        }
    }
    private IEnumerator WaitAndDeactivateSplashScreen(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        splashScreen.gameObject.SetActive(false);
    }

}



