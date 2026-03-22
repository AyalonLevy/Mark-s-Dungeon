using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LootEntry
{
    public ItemData Item;
    [Range(1, 100)] public int Weight;  // higher weight = more common
}

[CreateAssetMenu(fileName = "NewLootTable", menuName = "Mark's Dungeon/Items/LootTable")]
public class LootTable : ScriptableObject
{
    public List<LootEntry> PossibleDrops;
    public int MinItems = 1;
    public int MaxItems = 3;

    public List<ItemData> GetLoot()
    {
        List<ItemData> droppedItems = new();
        int count = Random.Range(MinItems, MaxItems + 1);

        for (int i = 0; i < count; i++)
        {
            ItemData picked = PickRandomItem();
            if (picked != null)
            {
                droppedItems.Add(picked);
            }
        }

        return droppedItems;
    }

    private ItemData PickRandomItem()
    {
        int totalWeight = 0;
        foreach (var entry in PossibleDrops)
        {
            totalWeight += entry.Weight;
        }

        int roll = Random.Range(0, totalWeight);
        int cursor = 0;

        foreach (var entry in PossibleDrops)
        {
            cursor += entry.Weight;
            if (roll < cursor)
            {
                return entry.Item;
            }
        }

        return null;
    }
}
