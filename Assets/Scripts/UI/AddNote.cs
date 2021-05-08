using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddNote : MonoBehaviour
{
    public GameObject notePanelPrefab;

    public InputField inputField;
    public TabGroup tabGroup;

    public void AddNewNote()
    {
        GameObject holder = tabGroup.selectedTab.objectsToToggle[0];

        GameObject noteInstance = Instantiate(notePanelPrefab, holder.transform);

        Text noteText = noteInstance.transform.Find("Text Note")?.GetComponent<Text>();
        noteText.text = inputField.text;

        inputField.text = "";

        SaveNewNote(noteText.text);
    }

    private void SaveNewNote(string text)
    {
        int index = tabGroup.selectedTab.transform.GetSiblingIndex();
        GameManager.Instance.AddNote(text, index);
    }
}
