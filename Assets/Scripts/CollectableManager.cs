using UnityEngine;


public class CollectableManager : MonoBehaviour
    {
        public static CollectableManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
