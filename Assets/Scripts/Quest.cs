using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest : ScriptableObject
{
    public enum Progress
    {
        Hidden,
        Active,
        Completed,
    }

    public uint ID;
    public Progress progress;

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
}
