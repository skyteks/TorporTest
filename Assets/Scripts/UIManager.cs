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
        }
    }

    public bool preSelectFirst;
    public Canvas canvas;

    private TabLevelHolders levelCategoryHolders;

    [Space]
    [Header("Holders")]
    public TabLevelPrefabs levelCategoryPrefabs;
    public TabLevelPrefabs levelTopicPrefabs;
    public TabLevelPrefabs levelEntryPrefabs;

    public void PreviewData(SaveData data)
    {
        SaveData.ITab[] categories = data.codex.GetChildren();
        levelCategoryHolders = GetHolderReferences(SaveData.Codex.Levels.Categories);
        ClearTabLevel(levelCategoryHolders);
        for (int i = 0; i < categories.Length; i++)
        {
            SetupLevel(categories[i], i, levelCategoryPrefabs, levelCategoryHolders);

            SaveData.ITab[] topics = categories[i].GetChildren();
            TabLevelHolders levelTopicHolders = GetHolderReferences(SaveData.Codex.Levels.Topics);
            ClearTabLevel(levelTopicHolders);
            for (int j = 0; j < topics.Length; j++)
            {
                SetupLevel(topics[j], j, levelTopicPrefabs, levelTopicHolders);

                SaveData.ITab[] entries = topics[j].GetChildren();
                TabLevelHolders levelEntryHolders = GetHolderReferences(SaveData.Codex.Levels.Entries);
                ClearTabLevel(levelEntryHolders, !(i == 0 && j == 0));
                for (int k = 0; k < entries.Length; k++)
                {
                    SetupLevel(entries[k], k, levelEntryPrefabs, levelEntryHolders);
                }
            }
        }
    }

    private void SetupLevel(SaveData.ITab tab, int index, TabLevelPrefabs prefabs, TabLevelHolders holders)
    {
        GameObject buttonInstance = Instantiate(prefabs.buttonPrefab, holders.buttonsHolder.transform);
        GameObject panelInstance = Instantiate(prefabs.panelPrefab, holders.panelsHolder.transform);

        buttonInstance.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        panelInstance.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        buttonInstance.name = string.Concat(prefabs.buttonPrefab.name, "  ", tab.GetName());
        panelInstance.name = string.Concat(prefabs.panelPrefab.name, "  ", tab.GetName());

        TabGroup tabGroup = holders.panelsHolder.GetComponent<TabGroup>();
        TabButton tabButton = buttonInstance.GetComponent<TabButton>();
        tabButton.tabGroup = tabGroup;
        tabButton.objectsToToggle.Clear();
        tabButton.objectsToToggle.Add(panelInstance);
        if (preSelectFirst && index == 0)
        {
            tabGroup.selectedTab = tabButton;
        }
        tabButton.Init();

        PanelInfoReference infoReference;
        infoReference = buttonInstance.GetComponent<PanelInfoReference>();
        if (infoReference != null)
        {
            infoReference.nameText.text = tab.GetName();
        }
        infoReference = panelInstance.GetComponent<PanelInfoReference>();
        if (infoReference != null)
        {
            infoReference.nameText.text = tab.GetName();
        }

        if (tab is SaveData.Codex.Category)
        {
            buttonInstance.GetComponentInChildren<Text>().text = (index + 1).ToString();
        }
        if (tab is SaveData.Codex.Category.Topic.Entry)
        {
            SaveData.Codex.Category.Topic.Entry entry = tab as SaveData.Codex.Category.Topic.Entry;
            infoReference.entryImage.sprite = entry.image;
            infoReference.entryText.text = entry.text;
        }
    }

    private TabLevelHolders GetHolderReferences(SaveData.Codex.Levels level)
    {
        ChildHolderReference[] foundReferences = canvas.GetComponentsInChildren<ChildHolderReference>(true);
        ChildHolderReference buttonsReference = null;
        ChildHolderReference panelsReference = null;
        for (int i = 0; i < foundReferences.Length; i++)
        {
            if (foundReferences[i].levelValue == level)
            {
                switch (foundReferences[i].typeValue)
                {
                    case ChildHolderReference.HolderTypes.buttons:
                        buttonsReference = foundReferences[i];
                        break;
                    case ChildHolderReference.HolderTypes.panels:
                        panelsReference = foundReferences[i];
                        break;
                }
            }
        }
        if (buttonsReference == null || panelsReference == null)
        {
            throw new NullReferenceException();
        }
        return new TabLevelHolders(buttonsReference, panelsReference);
    }

    private void ClearTabLevel(TabLevelHolders holders, bool dontClearPanel = false)
    {
        foreach (Transform child in holders.buttonsHolder.transform)
        {
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }
        if (!dontClearPanel)
        {
            foreach (Transform child in holders.panelsHolder.transform)
            {
                child.gameObject.SetActive(false);
                //Destroy(child.gameObject);
                child.transform.SetParent(null, true);
            }
        }
    }
}
