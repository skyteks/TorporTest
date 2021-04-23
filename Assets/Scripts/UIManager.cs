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
    public TabLevelPrefabs levelEntryPrefabs;
    public TabLevelPrefabs levelContentPrefabs;

    public void PreviewData(SaveData data)
    {
        SaveData.Codex.Category[] categories = data.codex.categories;
        levelCategoryHolders = GetHolderReferences(SaveData.Codex.Levels.Categories);
        ClearTabLevel(levelCategoryHolders);
        for (int i = 0; i < categories.Length; i++)
        {
            GameObject buttonInstance = Instantiate(levelCategoryPrefabs.buttonPrefab, Vector3.zero, Quaternion.identity, levelCategoryHolders.buttonsHolder.transform);
            GameObject panelInstance = Instantiate(levelCategoryPrefabs.panelPrefab, Vector3.zero, Quaternion.identity, levelCategoryHolders.panelsHolder.transform);

            TabGroup tabGroup = levelCategoryHolders.panelsHolder.GetComponent<TabGroup>();
            TabButton tabButton = buttonInstance.GetComponent<TabButton>();
            tabButton.tabGroup = tabGroup;
            tabButton.objectsToToggle.Clear();
            tabButton.objectsToToggle.Add(panelInstance);
            if (preSelectFirst && i == 0)
            {
                tabGroup.selectedTab = tabButton;
            }
            tabButton.Init();

            buttonInstance.GetComponentInChildren<Text>().text = (i + 1).ToString();

            PanelInfoReference infoReference;
            infoReference = buttonInstance.GetComponent<PanelInfoReference>();
            if (infoReference == null)
            {
                infoReference = panelInstance.GetComponent<PanelInfoReference>();
            }
            infoReference.nameText.text = categories[i].name;

            TabLevelHolders levelEntryHolders = GetHolderReferences(SaveData.Codex.Levels.Entries);
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
                    case ChildHolderReference.HolderTypes.tabs:
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

    private void ClearTabLevel(TabLevelHolders holders)
    {
        foreach (Transform child in holders.buttonsHolder.transform)
        {
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }
        foreach (Transform child in holders.panelsHolder.transform)
        {
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }
    }
}
