using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    protected CharacterMovement movement;
    protected new CharacterAnimation animation;

    protected virtual void Awake()
    {
        movement = GetComponent<CharacterMovement>();
        animation = GetComponent<CharacterAnimation>();
    }
}
