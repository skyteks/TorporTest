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

        public List<GameObject> objectsToToggle = new List<GameObject>();

        [Space]

        public bool transitionTextColor;

        public Sprite spriteNormal;
        public Sprite spriteSelected;

        public Color colorNormal = new Color(255f / 255f, 255f / 255f, 255f / 255f, 1f);
        public Color colorSelected = new Color(200f / 255f, 200f / 255f, 200f / 255f, 1f);

        [Space]

        private bool selected;

        public UnityEvent onTabSelected;
        public UnityEvent onTabDeselected;

        public void Init()
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
        }

        public void Deselect()
        {
            Toggle(false);
        }

        private void Toggle(bool toggle)
        {
            selected = toggle;
            for (int i = 0; i < objectsToToggle.Count; i++)
            {
                if (objectsToToggle[i] != null)
                {
                    objectsToToggle[i].SetActive(toggle);
                }
            }

            background.sprite = toggle ? spriteSelected : spriteNormal;
            if (transitionTextColor)
            {
                Text text = background.GetComponentInChildren<Text>();
                if (text != null)
                {
                    text.color = toggle ? colorSelected : colorNormal;
                }
            }

            if (toggle)
            {
                onTabSelected?.Invoke();
            }
            else
            {
                onTabDeselected?.Invoke();
            }
        }
    }
}
