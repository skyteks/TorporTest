using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Rotator : MonoBehaviour
{
    public float speed = 1f;
    public Vector3 axis = Vector3.up;

    void Update()
    {
        transform.Rotate(axis, speed * Time.deltaTime);
    }
}
