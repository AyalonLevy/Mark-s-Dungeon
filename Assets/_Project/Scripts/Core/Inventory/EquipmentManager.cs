using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    private ActorStats _stats;
    private InventoryManager _inventory;
    private CombatHandler _combatHandler;

    private Dictionary<EquipmentSlot, EquipmentData> _currentEquipment = new();

    private void Awake()
    {
        _stats = GetComponent<ActorStats>();
        _inventory = GetComponent<InventoryManager>();
        _combatHandler = GetComponent<CombatHandler>();

        // Initialize empty slots
        foreach (EquipmentSlot slot in System.Enum.GetValues(typeof(EquipmentSlot)))
        {
            _currentEquipment[slot] = null;
        }
    }

    public void Equip(EquipmentData newItem)
    {
        _stats.UpdateMaxVitalsProportionally(() =>
        {
            if (_currentEquipment[newItem.Slot] != null)
            {
                Unequip(newItem.Slot);
            }

            _currentEquipment[newItem.Slot] = newItem;

            ApplyEquipmentEffects(newItem, true);
        });

        Debug.Log($"Equipped {newItem.ItemName} to {newItem.Slot}");
    }

    public void Unequip(EquipmentSlot slot)
    {
        if (_currentEquipment[slot] == null)
        {
            return;
        }

        _stats.UpdateMaxVitalsProportionally(() =>
        {
            EquipmentData oldItem = _currentEquipment[slot];

            ApplyEquipmentEffects(oldItem, false);

            _currentEquipment[slot] = null;
            
            Debug.Log($"Unequipped {oldItem.ItemName} from {slot}");
        });
    }

    private void ApplyEquipmentEffects(EquipmentData item, bool isEquipping)
    {
        foreach (var modData in item.Modifiers)
        {
            Stat targetStat = GetStatByType(modData.StatType);

            if (targetStat == null)
            {
                continue;
            }

            if (isEquipping)
            {
                var mod = new StatModifier
                {
                    Value = modData.Value,
                    Source = ModifierSource.Equipment, Connection = item
                };
                targetStat.AddModifier(mod);
            }
            else
            {
                targetStat.RemoveModifier(item);
            }
        }

        if (item is BackpackData backpack)
        {
            _inventory.AdditionalSlots = isEquipping ? backpack.SlotBonus : 0;
            _inventory.AdditionalWeightCapacity = isEquipping ? backpack.WeightCapacityBonus : 0;
        }

        if (item.Slot == EquipmentSlot.Weapon)
        {
            WeaponData weapon = item as WeaponData;
            _combatHandler.UpdateEquippedWeapon(isEquipping ? weapon : null);
        }
    }

    private Stat GetStatByType(AffectedStat type)
    {
        return type switch
        {
            AffectedStat.Strength => _stats.Strength,
            AffectedStat.Dexterity => _stats.Dexterity,
            AffectedStat.Intelligence => _stats.Intelligence,
            AffectedStat.Vitality => _stats.Vitality,
            _ => null
        };
    }
}
