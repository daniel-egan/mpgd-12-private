//This script was made using this tutorial: https://youtu.be/rJqP5EesxLk?si=CMstgPC0u9ets30Y

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    //values for our camera, X and Y rotations/look on mouse
    public Camera cam;
    private float xRotation = 0f;

    public float xSensitivity = 90f;
    public float ySensitivity = 90f;

    //values for camera tilt when wall running
    public float tiltSpeed = 10f;
    public float currentTilt = 0f;
   
    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        // Calculate camera rotation for looking up and down
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, currentTilt);

        // Combine xRotation for looking up/down with current tilt
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // Rotate player to look left and right
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }
}
