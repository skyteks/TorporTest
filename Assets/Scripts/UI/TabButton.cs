using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Image))]
    public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        public TabGroup tabGroup;

        public Image background;

        public GameObject[] objectsToToggle;

        [Space]

        public UnityEvent onTabSelected;
        public UnityEvent onTabDeselected;

        void Awake()
        {
            background = GetComponent<Image>();
            tabGroup.Subscribe(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            tabGroup.OnTabSelected(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tabGroup.OnTabEnter(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tabGroup.OnTabExit(this);
        }

        public void Select()
        {
            Toggle(true);
            onTabSelected?.Invoke();
        }

        public void Deselect()
        {
            Toggle(false);
            onTabDeselected?.Invoke();
        }

        private void Toggle(bool toggle)
        {
            for (int i = 0; i < objectsToToggle.Length; i++)
            {
                if (objectsToToggle[i] != null)
                {
                    objectsToToggle[i].SetActive(toggle);
                }
            }
        }
    }
}
