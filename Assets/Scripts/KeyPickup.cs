using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))  // if the player collides with the object
        {
            collision.gameObject.GetComponent<Player>().hasKey = true; // makes the bool hasKey true so the door knows the player has the key
            Transform l0ck = transform.Find("Lock");  //lock -> l0ck [lock is a keyword in C#]
            if (l0ck != null)
            {
                Destroy(l0ck.gameObject); // destroys lock as visual indication of obtaining key
            }
        }
    }
}
