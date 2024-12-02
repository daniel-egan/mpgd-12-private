using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    public float speed = 2f;           // How fast the platform moves
    public float height = 3f;         // Total range of vertical motion
    private Vector3 startPosition;    // Starting position of the platform

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Move the platform up and down using Mathf.PingPong
        float newY = startPosition.y + Mathf.PingPong(Time.time * speed, height);
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}