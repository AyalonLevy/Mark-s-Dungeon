using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Data/Enemy Data")]
public class EnemyData : EntityDataSO
{
    [Header("Health Settings")]
    [Tooltip("Amount of HP regenerated per second when not in combat")]
    public float HealthRegenerationRate = 1.0f;

    [Header("Movement Settings")]
    public float WalkSpeed = 5.0f;
    public float SprintSpeed = 8.0f;

    [Header("Combat Settings")]
    public float AttackDamage = 10.0f;
    public float AttackCooldown = 0.5f;
}
