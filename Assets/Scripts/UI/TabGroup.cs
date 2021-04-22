using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class TabGroup : MonoBehaviour
    {
        private List<TabButton> tabButtons;

        public TabButton selectedTab;

        private void Start()
        {
            if (selectedTab != null)
            {
                OnTabSelected(selectedTab);
            }
        }

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
        }

        public void OnTabExit(TabButton button)
        {
            ResetTabs();
        }

        public void OnTabSelected(TabButton button)
        {
            selectedTab = button;
            selectedTab.Select();
            ResetTabs();
        }

        public void ResetTabs()
        {
            foreach (var button in tabButtons)
            {
                if (button != null && button == selectedTab)
                {
                    continue;
                }
                button.Deselect();
            }
        }
    }
}
