using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class InventoryManager : MonoBehaviour
{
    private ActorStats _stats;

    [Header("Inventory Settings")]
    public int BaseSlot = 20;
    public float WeightLimitMultiplier = 5.0f;  // TODO: Define properly -> 1 STR = 5kg

    private List<ItemData> _items = new();
    private float _minimumSpeedMultiplier = 0.2f;

    // "Magic Backpack"
    public int AdditionalSlots { get; set; }
    public float AdditionalWeightCapacity { get; set; }

    public int MaxSlot => BaseSlot + AdditionalSlots;
    public float MaxWeight => (_stats.Strength.Value * WeightLimitMultiplier) + AdditionalWeightCapacity;

    private void Awake() => _stats = GetComponent<ActorStats>();

    public bool AddItem(ItemData item)
    {
        if (_items.Count >= MaxSlot)
        {
            return false;
        }

        _items.Add(item);
        return true;
    }

    public float GetTotalWeight()
    {
        float total = 0;
        foreach (var item in _items)
        {
            total += item.weight;
        }

        return total;
    }

    public float GetEncumbranceMultiplier()
    {
        float total = GetTotalWeight();
        if (total <= MaxWeight)
        {
            return 1.0f;
        }

        // If over weight, slow down by the percentage over limit
        // TODO: Refine formula: if 20% over, speed is 0.8x
        float overage = (total - MaxWeight) / MaxWeight;
        return Mathf.Clamp(1.0f - overage, _minimumSpeedMultiplier, 1.0f);
    }
}
