using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestlogEntryUI : MonoBehaviour
{
    public uint ID { private set; get; }
    public Text title;
    public Image checkmark;

    public void Show(uint questID, bool completed = false)
    {
        ID = questID;
        Quest quest = QuestManager.Instance.GetQuestWithID(questID);
        title.text = quest.title;
        checkmark.gameObject.SetActive(completed);
    }

    public void EnableCheckmark()
    {
        checkmark.gameObject.SetActive(true);
    }
}
