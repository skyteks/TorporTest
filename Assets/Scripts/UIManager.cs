using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [System.Serializable]
    public struct TabLevel
    {
        public GameObject buttonPrefab;
        public GameObject panelPrefab;
        [Space]
        public ChildHolderReference buttonsHolder;
        public ChildHolderReference panelsHolder;
    }

    public bool preSelectFirst;

    [Header("Category")]
    public TabLevel levelCategory;

    public void PreviewData(SaveData data)
    {
        SaveData.Codex.Category[] categories = data.codex.categories;
        ClearTabLevel(levelCategory);
        for (int i = 0; i < categories.Length; i++)
        {
            GameObject buttonInstance = Instantiate(levelCategory.buttonPrefab, Vector3.zero, Quaternion.identity, levelCategory.buttonsHolder.transform);
            GameObject panelInstance = Instantiate(levelCategory.panelPrefab, Vector3.zero, Quaternion.identity, levelCategory.panelsHolder.transform);

            TabGroup tabGroup = levelCategory.panelsHolder.GetComponent<TabGroup>();
            TabButton tabButton = buttonInstance.GetComponent<TabButton>();
            tabButton.tabGroup = tabGroup;
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
        }
    }

    private void ClearTabLevel(TabLevel level)
    {
        foreach (Transform child in level.buttonsHolder.transform)
        {
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }
        foreach (Transform child in level.panelsHolder.transform)
        {
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }
    }
}
