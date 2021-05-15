using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestContractUI : MonoBehaviour
{
    public Text titleText;
    public Text descriptionText;
    public Button acceptButton;
    public Button declineButton;

    public bool upperTitle;

    private Quest currentQuest;

    public void Show(uint questID)
    {
        currentQuest = QuestManager.Instance.GetQuestWithID(questID);
        titleText.text = upperTitle ? currentQuest.title.ToUpper() : currentQuest.title;
        descriptionText.text = currentQuest.description;

        gameObject.SetActive(true);
    }

    public void QuestContractReaction(bool accepted)
    {
        Clear();
        GameEvent.QuestAccepted e = new GameEvent.QuestAccepted(currentQuest.ID, accepted);
        EventManager.Instance.Trigger(e);
        currentQuest = null;
    }

    private void Clear()
    {
        titleText.text = "";
        descriptionText.text = "";
        gameObject.SetActive(false);
    }
}
