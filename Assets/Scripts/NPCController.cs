using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : BaseController
{
    public enum NPCStates
    {
        WalkingArround,
        Talking,
    }

    public NPCStates state;
    public Color walkingColor = Color.yellow;
    public Color talkingColor = Color.green;

    public Vector3[] walkingPoints;
    private int walkingIndex;
    private bool arrivalSubscribed;

    void Start()
    {
        SwitchState(state);
    }

    void OnDrawGizmosSelected()
    {
        switch (state)
        {
            case NPCStates.WalkingArround:
                if (walkingPoints != null)
                {
                    for (int i = 0; i < walkingPoints.Length; i++)
                    {
                        Vector3 point = walkingPoints[i];

                        Gizmos.color = Color.yellow;
                        Vector3 next = walkingPoints[(i + 1) % walkingPoints.Length];
                        Gizmos.DrawLine(next, point);
                        if (i == walkingIndex)
                        {
                            Gizmos.color = Color.red;
                            Gizmos.DrawLine(transform.position, point);
                        }
                        Gizmos.DrawSphere(point, 0.2f);
                    }
                }
                break;
            case NPCStates.Talking:
                break;
        }
    }

    public void SwitchState(NPCStates newState)
    {
        state = newState;
        switch (state)
        {
            case NPCStates.WalkingArround:
                WalkFromPointToPoint();
                break;
            case NPCStates.Talking:
                arrivalSubscribed = false;
                break;
        }
        SetStateColor();
    }

    private void SetStateColor()
    {
        Color newColor;
        switch (state)
        {
            case NPCStates.WalkingArround:
                newColor = walkingColor;
                break;
            case NPCStates.Talking:
                newColor = talkingColor;
                break;
            default:
                throw new ArgumentException();
        }

        Renderer render = GetComponentInChildren<Renderer>();
        if (render != null)
        {
            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            render.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor("_Color", newColor);
            render.SetPropertyBlock(propertyBlock);
        }
    }

    private void WalkFromPointToPoint()
    {
        ArrivalSubscribe();

        WalkToNextPoint();
    }

    private void OnArrived()
    {
        walkingIndex = (walkingIndex + 1) % walkingPoints.Length;
        WalkToNextPoint();
    }

    private void WalkToNextPoint()
    {
        Vector3 point = walkingPoints[walkingIndex];
        movement.GoTo(point);
    }

    private void ArrivalSubscribe()
    {
        if (arrivalSubscribed)
        {
            Debug.LogError("Already subscribed to Arrival Event");
            return;
        }
        movement.onArrived.AddListener(OnArrived);
        arrivalSubscribed = true;
    }

    private void ArrivalUnsubscribe()
    {
        if (!arrivalSubscribed)
        {
            Debug.LogError("Already unsubscribed to Arrival Event");
            return;
        }
        movement.onArrived.RemoveListener(OnArrived);
        arrivalSubscribed = false;
    }
}
