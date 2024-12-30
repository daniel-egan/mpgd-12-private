using UnityEngine;
using UnityEngine.UI;

public class KeyPickup : MonoBehaviour
{
    public Text Objectives;
    public Image KeyIcon;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))  // if the player collides with the object
        {
            Debug.Log("Coliiiiiiiiiiiidedd");

            collision.gameObject.GetComponent<Player>().hasKey = true; // makes the bool hasKey true so the door knows the player has the key
            GameObject l0ck = GameObject.Find("rust_key");  //lock -> l0ck [lock is a keyword in C#]
            if (l0ck != null)
            {
                Objectives.text = "Use the key to escape!";
                KeyIcon.gameObject.SetActive(true);
                Destroy(l0ck.gameObject); // destroys lock as visual indication of obtaining key
            }
        }
    }
}
