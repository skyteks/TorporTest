using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;

    public float followSpeed;
    private Vector3 cameraFollowVelocity = Vector3.zero;

    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref cameraFollowVelocity, Time.deltaTime / followSpeed);
    }
}
