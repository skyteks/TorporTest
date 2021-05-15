using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    [SerializeField]
    private List<Quest> allQuests = new List<Quest>();
    private List<Quest> activeQuests = new List<Quest>();

    void OnEnable()
    {
        EventManager.Instance.AddListener<GameEvent.QuestAccepted>(OnQuestAccepted);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<GameEvent.QuestAccepted>(OnQuestAccepted);
    }

    private void OnQuestAccepted(GameEvent.QuestAccepted questAcceptedEvent)
    {
        Quest quest = GetQuestWithID(questAcceptedEvent.questID);
        quest.progress = Quest.Progress.Active;
        activeQuests.Add(quest);
    }

    private void OnQuestCompleted(GameEvent.QuestCompleted questCompletedEvent)
    {
        Quest quest = GetActiveQuestWithID(questCompletedEvent.questID);
        quest.progress = Quest.Progress.Completed;
        activeQuests.Remove(quest);
    }

    public Quest GetQuestWithID(uint questID)
    {
        foreach (Quest quest in allQuests)
        {
            if (quest.ID == questID)
            {
                return quest;
            }
        }
        return null;
    }

 





    public bool HasActiveQuestWithID(IList<uint> questIDs)
    {
        for (int i = 0; i < questIDs.Count; i++)
        {
            foreach (Quest quest in activeQuests)
            {
                if (quest.ID == questIDs[i])
                {
                    return true;
                }
            }
        }
        return false;
    }

    public Quest GetActiveQuestWithID(uint questID)
    {
        foreach (Quest quest in activeQuests)
        {
            if (quest.ID == questID)
            {
                return quest;
            }
        }
        return null;
    }

    public Quest GetFirstActiveQuestWithID(IList<uint> questIDs)
    {
        for (int i = 0; i < questIDs.Count; i++)
        {
            foreach (Quest quest in activeQuests)
            {
                if (quest.ID == questIDs[i])
                {
                    return quest;
                }
            }
        }
        return null;
    }

    public void TryGivingQuest(uint questID)
    {
        UIManager.Instance.ShowQuest(questID);
    }
}
