using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform cam;

    void LateUpdate()
    {
        Quaternion rotation3D = new Quaternion();
        rotation3D.SetLookRotation(cam.position);
        rotation3D.z = 0f;
        rotation3D.x = 0f;
        rotation3D.Normalize();
        transform.rotation = rotation3D;
    }
}
