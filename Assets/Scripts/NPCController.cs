using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class NPCController : BaseController
{
    public enum NPCStates
    {
        Stopped,
        WalkingArround,
        Talking,
    }

    public NPCStates state;
    public Color stoppedColor = Color.red;
    public Color walkingColor = Color.yellow;
    public Color talkingColor = Color.green;
    private NPCStates oldState;

    [Header("Walking Behavior")]
    public Vector3[] walkingPoints;
    private int walkingIndex;
    private bool arrivalSubscribed;

    [Header("Talking Behavior")]
    public NPCController talkingPartner;
    public bool talkingFirst;
    [TextArea]
    public string[] talkingSpeeches;
    private int talkingIndex;
    private NPCSpeechBubble speechBubble;
    private bool spokenArrived;

#if UNITY_EDITOR
    [Header("Editor")]
    public bool drawWalkingCircle = true;
    public bool drawOtherWalkingPoints = true;
#endif

    protected override void Awake()
    {
        base.Awake();
        speechBubble = GetComponent<NPCSpeechBubble>();
    }

    void Start()
    {
        SwitchState(state);
    }

#if UNITY_EDITOR
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
                        if (drawWalkingCircle)
                        {
                            Vector3 next = walkingPoints[(i + 1) % walkingPoints.Length];
                            Gizmos.DrawLine(next, point);
                        }
                        if (i == walkingIndex)
                        {
                            Gizmos.color = Color.red;
                            Gizmos.DrawLine(transform.position, point);
                        }
                        if (i != walkingIndex ? drawOtherWalkingPoints : true)
                        {
                            Gizmos.DrawSphere(point, 0.2f);
                        }
                    }
                }
                break;
            case NPCStates.Talking:
                break;
            case NPCStates.Stopped:
                break;
        }
    }
#endif

    private void SwitchState(NPCStates newState)
    {
        oldState = state;
        state = newState;
        ArrivalUnsubscribe();
        SpokenUnsubscibe();
        switch (state)
        {
            case NPCStates.WalkingArround:
                ArrivalSubscribe();
                WalkToNextPoint();
                break;
            case NPCStates.Talking:
                SpokenSubscibe();
                if (talkingFirst)
                {
                    TalkAgain();
                }
                break;
            case NPCStates.Stopped:
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
            case NPCStates.Stopped:
                newColor = stoppedColor;
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
            return;
        }
        movement.onArrived.AddListener(OnArrived);
        arrivalSubscribed = true;
    }

    private void ArrivalUnsubscribe()
    {
        if (!arrivalSubscribed)
        {
            return;
        }
        movement.onArrived.RemoveListener(OnArrived);
        arrivalSubscribed = false;
    }

    private void SpokenSubscibe()
    {
        if (spokenArrived)
        {
            return;
        }
        speechBubble.onSpeechOver.AddListener(OnFinishTalking);
        spokenArrived = true;
    }

    private void SpokenUnsubscibe()
    {
        if (!spokenArrived)
        {
            return;
        }
        speechBubble.onSpeechOver.RemoveListener(OnFinishTalking);
        spokenArrived = false;
    }

    public void Interacting(PlayerController player)
    {

        StopTalking();
        talkingPartner?.StopTalking();

        SwitchState(NPCStates.Stopped);

        movement.Stop();
        movement.RotateTowards(player.transform.position);

        ArrivalUnsubscribe();
        SpokenUnsubscibe();

        speechBubble.DoSpeech("Hello?\nWhat can I do for you?");
    }

    public void StopInteracting()
    {
        SwitchState(oldState);
    }

    public void TalkAgain()
    {
        talkingFirst = true;
        RotateTowardsTalkingPartner();
        talkingPartner.RotateTowardsTalkingPartner();
        speechBubble.DoSpeech(talkingSpeeches[talkingIndex]);
    }

    public void OnFinishTalking()
    {
        talkingIndex = (talkingIndex + 1) % talkingSpeeches.Length;
        talkingFirst = false;
        talkingPartner.TalkAgain();
    }

    public void StopTalking()
    {
        speechBubble.StopSpeech();
    }

    public void RotateTowardsTalkingPartner()
    {
        movement.RotateTowards(talkingPartner.transform.position);
    }

    public void OnNearPlayer(bool enter)
    {
        speechBubble.SetBubbleVisibility(enter);
    }
}
