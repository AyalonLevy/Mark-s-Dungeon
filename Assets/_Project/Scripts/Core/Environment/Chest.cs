using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IDamagable, IInteractable
{
    [Header("Loot Settings")]
    public LootTable lootTable;
    public int goldAmount;
    [Range(0.0f, 1.0f)]public float breakPenalty = 0.2f;
    
    [Header("Stats")]
    public float health = 5.0f;
    private float _currentHealth;
    private bool _isOpened = false;
    

    public float CurrentHealth => _currentHealth;

    public float MaxHealth => health;

    public string InteractionPrompt => "Press E to Open Chest";

    private void Awake()
    {
        _currentHealth = health;
    }

    public void Damage(float amount)
    {
        _currentHealth -= amount;
        if (_currentHealth < 0.0f )
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        _currentHealth += amount;
    }

    public bool IsDead()
    {
        return _currentHealth < 0.0f;
    }

    private void Loot(GameObject looter)
    {
        if (_isOpened)
        {
            return;
        }

        _isOpened = true;
        
        InventoryManager inventory = looter.GetComponent<InventoryManager>();

        float finalGold = IsDead() ? Mathf.FloorToInt(goldAmount * (1 - breakPenalty)) : goldAmount;
        // TODO: Add gold system -> add gold to "wallet"
        Debug.Log($"Collected {finalGold} gold from chest");

        if (lootTable != null)
        {
            List<ItemData> items = lootTable.GetLoot();
            foreach (var item in items)
            {
                // If the chest was destroyed, each item has a 'breakPenalty' chance to be lost
                if (IsDead() && Random.value < breakPenalty)
                {
                    Debug.Log($"<color=red>The {item.ItemName} was destroyed in the blast!</colo>");
                    continue;
                }

                if (inventory != null && inventory.AddItem(item))
                {
                    Debug.Log($"Found: {item.ItemName}");
                }
                else
                {
                    Debug.Log($"{item.ItemName} dropped on the floor (Inventory Full!)");
                    // TODO: Spawn physical item on the floor
                }
            }
        }
    }

    private void Die()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Loot(player);
        // TODO: visual destroied state
    }

    public void Interact(GameObject user)
    {
        Debug.Log($"<color=green>SUCCESS: You oppened the checst, you recieve {goldAmount} gold!</color>");

        Loot(user);

        // TODO: visual used state
    }
}
