using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new InteractQuest", menuName = "ScriptableObject/InteractQuest")]
public class InteractQuest : Quest
{
    [TextArea]
    public string rewardingReaction;
}
