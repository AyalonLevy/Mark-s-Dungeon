using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Data/Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Health Settings")]
    public float MaxHealth = 100.0f;
    [Tooltip("Amount of HP regenerated per second when not in combat")]
    public float HealthRegenerationRate = 1.0f;

    [Header("Movement Settings")]
    public float WalkSpeed = 5.0f;
    public float SprintSpeed = 8.0f;

    [Header("Combat Settings")]
    public float AttackDamage = 10.0f;
    public float AttackCooldown = 0.5f;

    [Header("Skill Settings")]
    public float MaxMana = 100.0f;
    //TODO: In the future add skill selection - each skill will be a SO with type/damage/range/cooldown
}
