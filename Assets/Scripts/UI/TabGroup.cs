using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class TabGroup : MonoBehaviour
    {
        public List<TabButton> tabButtons;

        public Color tabIdle = new Color(255f, 255f, 255f);
        public Color tabHover = new Color(245f, 245f, 245f);
        public Color tabActive = new Color(200f, 200f, 200f);

        public TabButton selectedTab;

        public List<GameObject> objectsToSwap;

        public void Subscribe(TabButton button)
        {
            if (tabButtons == null)
            {
                tabButtons = new List<TabButton>();
            }

            tabButtons.Add(button);
        }

        public void OnTabEnter(TabButton button)
        {
            ResetTabs();
            if (selectedTab == null || button != selectedTab)
            {
                button.background.color = tabHover;
            }
        }

        public void OnTabExit(TabButton button)
        {
            ResetTabs();

        }

        public void OnTabSelected(TabButton button)
        {
            selectedTab?.Deselect();

            selectedTab = button;
            selectedTab.Select();
            ResetTabs();
            button.background.color = tabActive;

            int index = button.transform.GetSiblingIndex();
            for (int i = 0; i < objectsToSwap.Count; i++)
            {
                if (i == index)
                {
                    objectsToSwap[i].SetActive(true);
                }
                else
                {
                    objectsToSwap[i].SetActive(false);
                }
            }
        }

        public void ResetTabs()
        {
            foreach (var button in tabButtons)
            {
                if (button != null && button == selectedTab)
                {
                    continue;
                }
                button.background.color = tabIdle;
            }
        }
    }
}
