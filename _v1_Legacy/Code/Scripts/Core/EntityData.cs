using UnityEngine;

[CreateAssetMenu(fileName = "NewEntityData", menuName = "Data/Entity")]
public class EntityData : ScriptableObject
{
    [Header("Base Vitals")]
    public float MaxHealth = 100.0f;
    public float MaxMana = 100.0f;
    public float MaxStamina = 100.0f;

    [Header("Regeneration (Per Second)")]
    public float HealthRegenerationRate = 1.0f;
    public float ManaRecoveryRate = 10.0f;
    public float StaminaRecoveryRate = 5.0f;
    public float StaminaDrainRate = 10.0f;

    [Header("Movement")]
    public float WalkSpeed = 5.0f;
    public float SprintSpeed = 8.0f;
    public float TurnSpeed = 10.0f;
    [HideInInspector] public float MovementThreshold = 0.01f;

    [Header("Perception & AI")]
    [Tooltip("The total distance this entity can 'sense' others")]
    [Range(1.0f, 10.0f)] public float DetectionRange = 15.0f;

    [Tooltip("The width of the vision cone in degrees")]
    [Range(0.0f, 360.0f)] public float ViewAngle = 120.0f;

    [Tooltip("Radius where the entity always detects others, even behind them")]
    public float ProximitySense = 2.5f;

    [Tooltip("Set the default behavior of the entity")]
    public AIBehavior DefaultBehavior;

    [Header("Combat Stats")]
    public float AttackDamage = 10.0f;
    public float AttackRange = 2.0f;
    public float AttackCooldown = 0.5f;

    [Header("Physics Layers")]
    public LayerMask EnemyLayer;
    public LayerMask ObstacleLayer;

    [Header("Equipment")]
    [Tooltip("Leave empty if this entity doesn't use a held weapon.")]
    public WeaponData StartingWeapon;
    [Tooltip("Leave empty if this entity doesn't use a shield.")]
    public ShieldData StartingShield;

    //TODO: In the future add skill selection - each skill will be a SO with type/damage/range/cooldown
}
