using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [System.Serializable]
    public struct TabLevelPrefabs
    {
        public GameObject buttonPrefab;
        public GameObject panelPrefab;
    }
    [System.Serializable]
    public struct TabLevelHolders
    {
        public ChildHolderReference buttonsHolder;
        public ChildHolderReference panelsHolder;

        public TabLevelHolders(ChildHolderReference buttonsReference, ChildHolderReference panelsReference)
        {
            buttonsHolder = buttonsReference;
            panelsHolder = panelsReference;
            SetUsed();
        }

        private void SetUsed()
        {
            if (buttonsHolder != null)
            {
                buttonsHolder.hasBeenUsed = true;
            }
            if (panelsHolder != null)
            {
                panelsHolder.hasBeenUsed = true;
            }
        }
    }

    public Button menuButton;
    public QuestContractUI questContract;
    public InputField noteInputField;

    [Space]
    [Header("Prefabs")]
    public TabLevelPrefabs levelCategoryPrefabs;
    public TabLevelPrefabs levelTopicPrefabs;
    public TabLevelPrefabs levelEntryPrefabs;
    [Space]
    public TabLevelPrefabs levelActPrefabs;
    public TabLevelPrefabs levelNotePrefabs;

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            gameObject.SetActive(false);
            menuButton.gameObject.SetActive(true);
            GameManager.Instance.SetTimeScale(1f);
        }
    }

    public void PreviewData(SaveData data)
    {
        SaveData.ITab[] categories = data.codex.GetChildren();
        TabLevelHolders levelCategoryHolders = GetHolderReferences(SaveData.Levels.Categories);
        for (int i = 0; i < categories.Length; i++)
        {
            SetupLevel(categories[i], i, levelCategoryPrefabs, levelCategoryHolders);

            SaveData.ITab[] topics = categories[i].GetChildren();
            TabLevelHolders levelTopicHolders = GetHolderReferences(SaveData.Levels.Topics);
            for (int j = 0; j < topics.Length; j++)
            {
                SetupLevel(topics[j], j, levelTopicPrefabs, levelTopicHolders);

                SaveData.ITab[] entries = topics[j].GetChildren();
                TabLevelHolders levelEntryHolders = GetHolderReferences(SaveData.Levels.Entries);
                for (int k = 0; k < entries.Length; k++)
                {
                    SetupLevel(entries[k], k, levelEntryPrefabs, levelEntryHolders);
                }
            }
        }

        SaveData.ITab[] acts = data.notes.GetChildren();
        TabLevelHolders levelActHolders = GetHolderReferences(SaveData.Levels.Acts);
        for (int i = 0; i < acts.Length; i++)
        {
            SetupLevel(acts[i], i, levelActPrefabs, levelActHolders);

            SaveData.ITab[] notes = acts[i].GetChildren();
            TabLevelHolders levelNoteHolders = GetHolderReferences(SaveData.Levels.Notes, true);
            for (int j = 0; j < notes.Length; j++)
            {
                SetupLevel(notes[j], j, levelNotePrefabs, levelNoteHolders);
            }
        }

        TabButton[] tabButtons = transform.GetComponentsInChildren<TabButton>(true);
        foreach (var button in tabButtons)
        {
            button.Init();
        }
        TabGroup[] tabGroups = transform.GetComponentsInChildren<TabGroup>(true);
        foreach (var tab in tabGroups)
        {
            tab.ResetTabs(true);
        }
    }

    private void SetupLevel(SaveData.ITab tab, int index, TabLevelPrefabs prefabs, TabLevelHolders holders)
    {
        GameObject buttonInstance = null;
        if (holders.buttonsHolder != null)
        {
            buttonInstance = Instantiate(prefabs.buttonPrefab, holders.buttonsHolder.transform);
            buttonInstance.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            buttonInstance.name = string.Concat(prefabs.buttonPrefab.name, "  ", tab.GetName());
        }
        GameObject panelInstance = Instantiate(prefabs.panelPrefab, holders.panelsHolder.transform);
        panelInstance.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        panelInstance.name = string.Concat(prefabs.panelPrefab.name, "  ", tab.GetName());

        TabGroup tabGroup = holders.panelsHolder.GetComponent<TabGroup>();
        if (buttonInstance != null)
        {
            TabButton tabButton = buttonInstance.GetComponent<TabButton>();
            tabButton.tabGroup = tabGroup;
            tabButton.objectsToToggle.Clear();
            tabButton.objectsToToggle.Add(panelInstance);

            tabGroup.selectedTab = index == 0 ? tabButton : null;
        }

        PanelInfoReference infoReferenceButton;
        PanelInfoReference infoReferencePanel;
        infoReferenceButton = buttonInstance?.GetComponent<PanelInfoReference>();
        if (infoReferenceButton != null)
        {
            infoReferenceButton.nameText.text = tab.GetName();
        }
        infoReferencePanel = panelInstance.GetComponent<PanelInfoReference>();
        if (infoReferencePanel != null)
        {
            infoReferencePanel.nameText.text = tab.GetName();
        }

        if (tab is SaveData.Codex.Category)
        {
            infoReferenceButton.nameText.text = (index + 1).ToString();
        }
        if (tab is SaveData.Codex.Category.Topic.Entry)
        {
            SaveData.Codex.Category.Topic.Entry entry = tab as SaveData.Codex.Category.Topic.Entry;
            infoReferencePanel.entryImage.sprite = entry.image;
            infoReferencePanel.entryText.text = entry.text;
        }
        if (tab is SaveData.Notes.Act)
        {
            string roman = SaveData.Notes.Act.ToRoman(index + 1);
            buttonInstance.name = string.Concat(buttonInstance.name, " ", roman);
            panelInstance.name = string.Concat(panelInstance.name, " ", roman);
            infoReferenceButton.nameText.text = string.Concat(infoReferenceButton.nameText.text, " ", roman);
        }
        if (tab is SaveData.Notes.Act.Note)
        {
            SaveData.Notes.Act.Note note = tab as SaveData.Notes.Act.Note;
            string text = note.text;
            if (text.Length > noteInputField.characterLimit)
            {
                text = text.Remove(noteInputField.characterLimit);
                note.text = text;
            }
            infoReferencePanel.nameText.text = text;
        }
    }

    private TabLevelHolders GetHolderReferences(SaveData.Levels level, bool noButton = false)
    {
        ChildHolderReference[] foundReferences = transform.GetComponentsInChildren<ChildHolderReference>(true);
        ChildHolderReference buttonsReference = null;
        ChildHolderReference panelsReference = null;
        for (int i = 0; i < foundReferences.Length; i++)
        {
            if (foundReferences[i].levelValue == level)
            {
                switch (foundReferences[i].typeValue)
                {
                    case ChildHolderReference.HolderTypes.buttons:
                        if (!foundReferences[i].hasBeenUsed)
                        {
                            buttonsReference = foundReferences[i];
                        }
                        break;
                    case ChildHolderReference.HolderTypes.panels:
                        if (noButton ? !foundReferences[i].hasBeenUsed : true)
                        {
                            panelsReference = foundReferences[i];
                        }
                        break;
                }
                if ((noButton ? true : buttonsReference != null) && panelsReference != null)
                {
                    break;
                }
            }
        }
        if ((!noButton && buttonsReference == null) || panelsReference == null)
        {
            throw new NullReferenceException();
        }
        return new TabLevelHolders(buttonsReference, panelsReference);
    }

    public void ShowQuest(Quest quest, UnityEngine.Events.UnityAction<bool> callback)
    {
        questContract.Show(quest, callback);
    }
}
