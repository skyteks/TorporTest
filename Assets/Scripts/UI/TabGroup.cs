using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class TabGroup : MonoBehaviour
    {
        private List<TabButton> buttons = new List<TabButton>();
        private List<TabGroup> childTabs = new List<TabGroup>();

        public TabButton selectedTab;

        public void Subscribe(TabButton button)
        {
            buttons.Add(button);

            List<TabGroup> children = new List<TabGroup>(button.objectsToToggle[0].transform.parent?.GetComponentsInChildren<TabGroup>());
            if (children != null && children.Count > 0)
            {
                children.Remove(this);
                childTabs.AddRange(children);
            }
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
            for (int i = 0; i < childTabs.Count; i++)
            {
                if(childTabs[i] == null)
                {
                    childTabs.RemoveAt(i);
                    i--;
                    continue;
                }
                childTabs[i].ResetTabs(true);
            }
        }
    }
}
