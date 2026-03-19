using UnityEngine;

[CreateAssetMenu(fileName = "NewConsumable", menuName = "Mark's Dungeon/Items/Consumable")]
public class ConsumableData : ItemData
{
    [Header("Usage Effect")]
    public StatusEffectData EffectToApply;

    public void Use(GameObject user)
    {
        if (user.TryGetComponent(out StatusEffectManager manager))
        {
            manager.ApplyStatus(EffectToApply);
            Debug.Log($"Used {ItemName} on {user.name}");
        }
    }
}
