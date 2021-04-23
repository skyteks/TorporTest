using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveNote : MonoBehaviour
{
    public void DeleteNote(PanelInfoReference panel)
    {
        int index = panel.transform.GetSiblingIndex();
        int actIndex = panel.transform.parent.GetSiblingIndex();
        GameManager.Instance.DeleteNote(index, actIndex);
    }

    public void DestroyObj(Object obj)
    {
        Destroy(obj);
    }
}
