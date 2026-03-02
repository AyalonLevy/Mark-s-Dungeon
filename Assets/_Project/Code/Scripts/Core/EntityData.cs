using UnityEngine;

[CreateAssetMenu(fileName = "NewEntityData", menuName = "Data/Entity")]
public class EntityData : ScriptableObject
{
    [Header("Health Settings")]
    [Tooltip("Amount of HP regenerated per second when not in combat")]
    public float MaxHealth = 100.0f;
    public float HealthRegenerationRate = 1.0f;

    [Header("Movement Settings")]
    [Tooltip("The threshold for movement input (squared)")]
    [HideInInspector] public float MovementThreshold = 0.01f;
    public float WalkSpeed = 5.0f;
    public float SprintSpeed = 8.0f;
    public float MaxStamina = 100.0f;
    [Tooltip("Amount of Stamina used per second when sprinting")]
    public float StaminaDrainRate = 10.0f;
    [Tooltip("Amount of Stamina regenerated per second when not sprinting or in combat")]
    public float StaminaRecoveryRate = 5.0f;

    [Header("AI Settings")]
    public float AggroRange = 10.0f;
    public float AttackRange = 2.0f;

    [Header("Combat Settings")]
    public float AttackDamage = 10.0f;
    public float AttackCooldown = 0.5f;

    [Header("Skill Settings")]
    public float MaxMana = 100.0f;
    [Tooltip("Amount of MP regenerated per second when not in combat")]
    public float ManaRecoveryRate = 10.0f;

    //TODO: In the future add skill selection - each skill will be a SO with type/damage/range/cooldown
}
