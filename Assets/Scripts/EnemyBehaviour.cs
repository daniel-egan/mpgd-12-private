using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Transform targetObj;
    private Rigidbody rb;
    public float pushBackForce = 1000f;
    private CharacterController characterController;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 direction = (targetObj.position - transform.position).normalized;

        float distanceToMove = 3 * Time.deltaTime;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, distanceToMove))
        {
            if (hit.collider.CompareTag("Platform"))
            {
                return;
            }

        }
        transform.position = Vector3.MoveTowards(this.transform.position, targetObj.position, distanceToMove);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 direction = (transform.position - collision.transform.position).normalized;
            Vector3 velocity = direction * pushBackForce;
            characterController.Move(velocity * Time.deltaTime);
        }
    }


}