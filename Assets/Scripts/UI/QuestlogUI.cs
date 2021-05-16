using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestlogUI : MonoBehaviour
{
    public GameObject questlogEntryPrefab;
    private List<QuestlogEntryUI> entries = new List<QuestlogEntryUI>();

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
        if (!e.accepted)
        {
            return;
        }
        QuestlogEntryUI logEntry = null;
        foreach (QuestlogEntryUI entry in entries)
        {
            if (entry.ID == e.questID)
            {
                logEntry = entry;
                break;
            }
        }
        if (logEntry == null)
        {
            GameObject entryInstance = Instantiate(questlogEntryPrefab, transform);
            logEntry = entryInstance.GetComponent<QuestlogEntryUI>();
            entries.Add(logEntry);
        }
        logEntry.Show(e.questID);
    }

    private void OnQuestCompleted(GameEvent.QuestCompleted e)
    {
        foreach (QuestlogEntryUI entry in entries)
        {
            if (entry.ID == e.questID)
            {
                entry.EnableCheckmark();
                return;
            }
        }
    }
}
