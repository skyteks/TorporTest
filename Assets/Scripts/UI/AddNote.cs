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
        CreateNote(inputField.text);

        inputField.text = "";
    }

    public void CreateNote(string text)
    {
        GameObject holder = tabGroup.selectedTab.objectsToToggle[0];
        GameObject noteInstance = Instantiate(notePanelPrefab, holder.transform);

        Text noteText = noteInstance.transform.Find("Text Note")?.GetComponent<Text>();

        noteText.text = inputField.text;

        SaveNote(text);
    }

    private void SaveNote(string text)
    {
        int index = tabGroup.selectedTab.transform.GetSiblingIndex();
        GameManager.Instance.AddNote(text, index);
    }
}
