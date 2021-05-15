using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class TabGroup : MonoBehaviour
    {
        private List<TabButton> buttons = new List<TabButton>();

        public TabButton selectedTab;

        public void Subscribe(TabButton button)
        {
            buttons.Add(button);
        }

        public void OnTabEnter(TabButton button)
        {
        }

        public void OnTabExit(TabButton button)
        {
        }

        public void OnTabSelected(TabButton button)
        {
            selectedTab = button;
            selectedTab.Select();
            ResetTabs();
        }

        public void ResetTabs(bool hard = false)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i] == null)
                {
                    buttons.RemoveAt(i);
                    i--;
                    continue;
                }
                if (buttons[i] == selectedTab && !hard)
                {
                    continue;
                }
                buttons[i].Deselect();
            }
        }

        public TabButton GetButton(int index)
        {
            return buttons[index];
        }
    }
}
