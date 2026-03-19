using UnityEngine;

public enum ItemType { Equipment, Consumable, Quest, Trash }

public abstract class ItemData : ScriptableObject
{
    [Header("Basic Info")]
    public string ItemName;
    [TextArea] public string ItemDescription;
    public Sprite Icon;
    public ItemType Type;
    public int MaxStackSize = 1;
    public int Value = 10;  // Gold values
    public float weight = 1.0f;
}
