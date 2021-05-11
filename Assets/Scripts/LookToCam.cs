using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookToCam : MonoBehaviour
{
    public Camera cam;

    void Awake()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    void LateUpdate()
    {
        if (Camera.main.orthographic)
        {
            transform.forward = cam.transform.forward;
        }
        else
        {
            Quaternion oldRot = transform.rotation;
            transform.LookAt(cam.transform, Vector3.up);
            transform.rotation = Quaternion.Euler(new Vector3(oldRot.eulerAngles.x, transform.rotation.eulerAngles.y, oldRot.eulerAngles.z));
        }
    }
}
