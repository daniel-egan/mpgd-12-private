using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up; // Axis of rotation (default: Y-axis)
    public float rotationSpeed = 50f; // Rotation speed in degrees per second

    void Update()
    {
        // Apply rotation around the specified axis
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.Self);
    }
}
