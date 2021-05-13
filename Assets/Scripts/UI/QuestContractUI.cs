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
    UnityEngine.Events.UnityAction<bool> contractReactionCallback;

    public void Show(Quest quest, UnityEngine.Events.UnityAction<bool> callback)
    {
        currentQuest = quest;
        contractReactionCallback = callback;
        titleText.text = upperTitle ? quest.title.ToUpper() : quest.title;
        descriptionText.text = quest.description;

        gameObject.SetActive(true);
    }

    public void QuestContractReaction(bool accepted)
    {
        Clear();
        contractReactionCallback.Invoke(accepted);
        currentQuest = null;
        contractReactionCallback = null;
    }

    private void Clear()
    {
        titleText.text = "";
        descriptionText.text = "";
        gameObject.SetActive(false);
    }
}
