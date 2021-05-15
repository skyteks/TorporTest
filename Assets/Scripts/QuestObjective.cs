using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjective : MonoBehaviour
{
    public uint questID;

    private NPCController npc;
    public bool isActiveObstacle { get; private set; }

    private void Awake()
    {
        npc = GetComponent<NPCController>();
    }

    void OnEnable()
    {
        EventManager.Instance.AddListener<GameEvent.QuestAccepted>(OnQuestAccepted);
        EventManager.Instance.AddListener<GameEvent.QuestCompleted>(OnQuestCompleted);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<GameEvent.QuestAccepted>(OnQuestAccepted);
        EventManager.Instance.RemoveListener<GameEvent.QuestCompleted>(OnQuestCompleted);
    }

    private void OnQuestAccepted(GameEvent.QuestAccepted e)
    {
        if (e.questID != questID || !e.accepted)
        {
            return;
        }

        isActiveObstacle = true;

        if (npc != null)
        {
            npc.SetStateColor();
        }
    }

    private void OnQuestCompleted(GameEvent.QuestCompleted e)
    {
        if (e.questID != questID)
        {
            return;
        }

        isActiveObstacle = false;

        if (npc != null)
        {
            npc.SetStateColor();
        }
    }
}
