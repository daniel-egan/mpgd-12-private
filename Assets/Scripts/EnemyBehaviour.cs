using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Transform targetObj;
    private Rigidbody rb;
    public float pushBackForce = 5f;
    private CharacterController characterController;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {

        // Raycast to check if enemy has hit an object and if so stops
        // Used https://www.youtube.com/watch?v=B34iq4O5ZYI and ChatGPT
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


        // Enemy moves towards a player
        // Used https://docs.unity3d.com/ScriptReference/Vector3.MoveTowards.html
        // and https://docs.unity3d.com/ScriptReference/Transform-position.html
        transform.position = Vector3.MoveTowards(this.transform.position, targetObj.position, distanceToMove);
    }

    // Collision detection used https://docs.unity3d.com/ScriptReference/Collider.OnCollisionEnter.html 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Vector created with direction and velocity is calculated to move the player
            // Used https://docs.unity3d.com/ScriptReference/Vector3.html
            // Used https://docs.unity3d.com/ScriptReference/CharacterController.Move.html

            Vector3 direction = (transform.position - collision.transform.position).normalized;
            Vector3 velocity = direction * pushBackForce;
            characterController.Move(velocity * Time.deltaTime);
        }
    }


}