using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest : ScriptableObject
{
    public string title;
    [TextArea]
    public string description;

    [Header("Quest Giver")]
    [TextArea]
    public string priorContractReaction;
    public bool giveQuestDirectly { get { return priorContractReaction == null || priorContractReaction.Length == 0; } }
    [TextArea]
    public string acceptedReaction;
    [TextArea]
    public string declinedReaction;

    [Header("Quest Replier")]
    public uint questAnsweringID;
}
