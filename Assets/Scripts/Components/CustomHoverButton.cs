using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomHoverButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public Text textNormal;
    public Text textHover;

    [Space]

    public Text noteText;

    [Space]

    public UnityEvent onClick;
    public UnityEvent<bool> onHover;

    void Awake()
    {
        Toggle(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Toggle(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Toggle(false);
    }

    private void Toggle(bool toggle)
    {
        textNormal.enabled = !toggle;
        textHover.enabled = toggle;

        noteText.color = toggle ? textHover.color : textNormal.color;

        onHover?.Invoke(toggle);
    }
}
