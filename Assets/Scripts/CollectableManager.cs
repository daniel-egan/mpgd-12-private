using UnityEngine;


public class CollectableManager : MonoBehaviour
{
    public static CollectableManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}