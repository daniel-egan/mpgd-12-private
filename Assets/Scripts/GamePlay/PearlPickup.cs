using UnityEngine;
using UnityEngine.UI;
public class PearlPickup : MonoBehaviour
{
    public Image pearl;
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pearl.gameObject.SetActive(true);
        }
    }
}
