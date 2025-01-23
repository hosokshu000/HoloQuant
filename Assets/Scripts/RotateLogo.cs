using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLogo : MonoBehaviour
{
    public float rotationSpeed = 50f; // Speed of rotation in degrees per second

    void Update()
    {
        // Calculate the rotation amount
        float rotationAmount = rotationSpeed * Time.deltaTime;

        // Apply rotation to the object around the Z-axis
        transform.Rotate(Vector3.forward, rotationAmount);
    }
}

