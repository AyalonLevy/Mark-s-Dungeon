using UnityEngine;

public class Chest : MonoBehaviour, IDamagable, IInteractable
{
    [Header("Settings")]
    public int goldAmount;
    public float health = 5.0f;
    [Range(0.0f, 1.0f)]public float breakPenalty = 0.2f;
    
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

    private void Loot()
    {
        if (_isOpened)
        {
            return;
        }

        _isOpened = true;
        
        if (IsDead())
        {
            float totalLoot = Mathf.FloorToInt(goldAmount * (1 - breakPenalty));
            Debug.Log($"You've destroyed {breakPenalty * 100}% of the loot! You manage to salvege only {totalLoot} gold");
        }
        else
        {
            Debug.Log($"You've collected {goldAmount} gold");
        }
    }

    private void Die()
    {
        Loot();
        // TODO: visual destroied state
    }

    public void Interact()
    {
        Debug.Log($"<color=green>SUCCESS: You oppened the checst, you recieve {goldAmount} gold!</color>");

        Loot();

        // TODO: visual used state
    }
}
