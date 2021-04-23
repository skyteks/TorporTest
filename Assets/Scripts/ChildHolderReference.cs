using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildHolderReference : MonoBehaviour
{
    public enum HolderTypes
    {
        buttons,
        tabs,
    }

    [SerializeField]
    private SaveData.Codex.Levels level = SaveData.Codex.Levels.Categories;
    [SerializeField]
    private HolderTypes holderType = HolderTypes.buttons;

    public SaveData.Codex.Levels levelValue { get { return level; } }
    public HolderTypes typeValue { get { return holderType; } }
}
