using System.Collections;
using System.Collections.Generic ;
using UnityEngine;
using UnityEngine.UI ;
using TMPro;

public class PlayerCollision : MonoBehaviour
{
    public float pushBackForce = 5f;
    private CharacterController characterController;

    public TextMeshProUGUI hitCount;
    private int count;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        UpdateHitText();

    }

    // Collision detection used https://docs.unity3d.com/ScriptReference/Collider.OnCollisionEnter.html 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Vector created with direction and velocity is calculated to move the player
            // Used https://docs.unity3d.com/ScriptReference/Vector3.html
            // Used https://docs.unity3d.com/ScriptReference/CharacterController.Move.html

            Vector3 direction = (transform.position - collision.transform.position).normalized;
            Vector3 velocity = direction * pushBackForce;
            count++;
            characterController.Move(velocity * (Time.deltaTime*20));
            UpdateHitText();
        }
    }

    private void UpdateHitText()
    {
        hitCount.text = "Hitcount : " + count.ToString();
    }
}

