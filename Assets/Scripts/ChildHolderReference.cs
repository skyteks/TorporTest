using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildHolderReference : MonoBehaviour
{
    public enum HolderTypes
    {
        buttons,
        panels,
    }

    [SerializeField]
    private SaveData.Levels level = SaveData.Levels.Categories;
    [SerializeField]
    private HolderTypes holderType = HolderTypes.buttons;

    public SaveData.Levels levelValue { get { return level; } }
    public HolderTypes typeValue { get { return holderType; } }
}
