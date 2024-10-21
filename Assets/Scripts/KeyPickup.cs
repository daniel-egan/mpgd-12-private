using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().hasKey = true;
            Transform l0ck = transform.Find("Lock");  //lock -> l0ck [lock is a keyword in C#]
            if (l0ck != null)
            {
                Destroy(l0ck.gameObject);
            }
        }
    }
}
