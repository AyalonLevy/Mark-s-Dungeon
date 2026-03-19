using UnityEngine;

[CreateAssetMenu(fileName = "NewBackpack", menuName = "Mark's Dungeon/Items/Backpack")]
public class BackpackData : EquipmentData
{
    [Header("Backpack Stats")]
    public int SlotBonus = 12;
    public float WeightCapacityBonus = 30.0f;

    private void OnEnable()
    {
        Slot = EquipmentSlot.Accessory;
    }
}
