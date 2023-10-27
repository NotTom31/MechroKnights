using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float rotationSpeed = 0.5f;

    private void Update()
    {
        // Rotate the camera around its up axis (Y-axis)
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
