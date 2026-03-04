using UnityEngine;

public abstract class EquipmentData : ScriptableObject
{
    public enum EquipmentTypes
    {
        Weapon,
        Shield
    }

    public string EquipmentName;
    public EquipmentTypes EquipmentType;
    public GameObject ModelPrefab;
    public Sprite Icon;
    public float Durability;
    public float Value;
}
