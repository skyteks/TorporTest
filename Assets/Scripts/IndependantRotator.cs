using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class IndependantRotator : MonoBehaviour
{
    public float speed = 1f;
    public Vector3 axis = Vector3.up;
    private float currentAngle;

    void LateUpdate()
    {
        currentAngle = (currentAngle + speed * Time.deltaTime) % 360f;
        float parentAngle = transform.parent.rotation.y;
        transform.rotation = Quaternion.Euler(Vector3.up * (parentAngle - currentAngle));
    }
}
