using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEffect : MonoBehaviour
{
    public Text text;

    [Space]

    public FontStyle selectFontStyle = FontStyle.Bold;

    [Space]

    public FontStyle deselectFontStyle = FontStyle.Normal;

    public void Select()
    {
        text.fontStyle = selectFontStyle;
    }

    public void Deselect()
    {
        text.fontStyle = deselectFontStyle;
    }
}
