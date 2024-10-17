using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public float pushBackForce = 1000f;
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector3 direction = (transform.position - collision.transform.position).normalized;
            Vector3 velocity = direction * pushBackForce;
            characterController.Move(velocity * Time.deltaTime);
        }
    }
}


