using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class NPCController : BaseController
{
    public enum NPCStates
    {
        None,
        WalkingArround,
        Talking,
        GivingQuests,
    }

    public NPCStates state;
    public Color walkingColor = Color.cyan;
    public Color talkingColor = Color.green;
    public Color questGivingColor = Color.Lerp(Color.yellow, Color.red, 0.5f);

    [Header("Interacting with Player")]
    public bool canBeInteractedWith;
    private bool isInteracting;
    [TextArea]
    public string interactionText;

    [Header("Walking Behavior")]
    public Vector3[] walkingPoints;
    private int walkingIndex;

    [Header("Talking Behavior")]
    public NPCController talkingPartner;
    public bool talkingFirst;
    [TextArea]
    public string[] talkingSpeeches;
    private int talkingIndex;
    private NPCSpeechBubble speechBubble;

    [Header("Quest Giving Behavior")]
    public GameObject questionmarkSymbol;
    public bool giveQuestDirectly;

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
            case NPCStates.None:
                break;
            case NPCStates.GivingQuests:
                break;
        }
    }
#endif

    private void SwitchInteracting(bool interactingWithPlayer)
    {
        isInteracting = interactingWithPlayer;
        ResetStateFunctionality();
        SetNewStateFunctionality();
    }

    private void SwitchState(NPCStates newState)
    {
        state = newState;
        ResetStateFunctionality();
        SetNewStateFunctionality();
    }

    private void ResetStateFunctionality()
    {
        ArrivalSubscribeToggle(false);
        SpokenSubscibeToggle(false);

        if (state != NPCStates.GivingQuests)
        {
            ToggleQuestionmarkSymbol(false);
        }
    }

    private void SetNewStateFunctionality()
    {
        SetStateColor();
        if (isInteracting)
        {
            return;
        }
        switch (state)
        {
            case NPCStates.WalkingArround:
                ArrivalSubscribeToggle(true);
                WalkToNextPoint();
                break;
            case NPCStates.Talking:
                SpokenSubscibeToggle(true);
                if (talkingFirst)
                {
                    TalkAgainToPartner();
                }
                break;
            case NPCStates.None:
                break;
            case NPCStates.GivingQuests:
                ToggleQuestionmarkSymbol(true);
                break;
        }
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
            case NPCStates.None:
                newColor = Color.gray;
                break;
            case NPCStates.GivingQuests:
                newColor = questGivingColor;
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

    private void ArrivalSubscribeToggle(bool toggle)
    {
        if (toggle)
        {
            movement.onArrived.AddListener(OnArrived);
        }
        else
        {
            movement.onArrived.RemoveListener(OnArrived);
        }
    }

    private void SpokenSubscibeToggle(bool toggle)
    {
        if (toggle)
        {
            speechBubble.onSpeechOver.AddListener(OnFinishTalkingToPartner);
        }
        else
        {
            speechBubble.onSpeechOver.RemoveListener(OnFinishTalkingToPartner);
        }
    }

    public void Interacting(PlayerController player)
    {
        if (!canBeInteractedWith)
        {
            return;
        }

        StopTalking();
        talkingPartner?.StopTalking();

        SwitchInteracting(true);

        movement.Stop();
        movement.RotateTowards(player.transform.position);

        ArrivalSubscribeToggle(false);
        SpokenSubscibeToggle(false);

        if (state == NPCStates.GivingQuests)
        {
            if (giveQuestDirectly)
            {
                TryGivingQuestToPlayer();
                return;
            }
            else
            {
                speechBubble.onSpeechOver.AddListener(TryGivingQuestToPlayer);
            }
        }

        speechBubble.DoSpeech(interactionText);
    }

    public void StopInteracting()
    {
        SwitchInteracting(false);
    }

    public void TalkAgainToPartner()
    {
        talkingFirst = true;
        RotateTowardsTalkingPartner();
        talkingPartner.RotateTowardsTalkingPartner();
        speechBubble.DoSpeech(talkingSpeeches[talkingIndex]);
    }

    public void OnFinishTalkingToPartner()
    {
        talkingIndex = (talkingIndex + 1) % talkingSpeeches.Length;
        talkingFirst = false;
        talkingPartner.TalkAgainToPartner();
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

    private void ToggleQuestionmarkSymbol(bool toggle)
    {
        questionmarkSymbol?.SetActive(toggle);
    }

    private void TryGivingQuestToPlayer()
    {
        speechBubble.onSpeechOver.RemoveListener(TryGivingQuestToPlayer);
        print("HERE IS A QUEST");
    }
}
