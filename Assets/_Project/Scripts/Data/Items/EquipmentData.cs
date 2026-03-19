using UnityEngine;
using System.Collections.Generic;
using System;

public enum EquipmentSlot { Head, Torso, Legs, Weapon, Accessory }

[CreateAssetMenu(fileName = "NewEquipment", menuName = "Mark's Dungeon/Items/Equipment")]
public class EquipmentData : ItemData
{
    public EquipmentSlot Slot;

    [Header("Stat Bonus")]
    public List<StatModifierData> Modifiers;

    public GameObject VisualPrefab;
}

[Serializable]
public struct StatModifierData
{
    public AffectedStat StatType;
    public float Value;
}
