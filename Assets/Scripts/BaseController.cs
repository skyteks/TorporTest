using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    public CharacterMovement movement { get; private set; }

    protected virtual void Awake()
    {
        movement = GetComponent<CharacterMovement>();
    }
}
