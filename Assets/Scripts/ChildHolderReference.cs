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
    private int level = 0;
    [SerializeField]
    private HolderTypes holderType = HolderTypes.buttons;

    public int levelValue { get { return level; } }
    public HolderTypes typeValue { get { return holderType; } }
}
