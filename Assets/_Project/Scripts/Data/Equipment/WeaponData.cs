using UnityEngine;


public enum AttackShape { Circle, Cone, Line }

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Mark's Dungeon/Data/Weapon")]
public class WeaponData : ScriptableObject
{
    [Header("Stats")]
    public float Damage = 10.0f;
    public float AttackRange = 2.0f;
    public float StaminaCost = 15.0f;

    [Header("Timing")]
    public float WindUpTime = 0.2f;     // Delay before the hit
    public float ActiveTime = 0.1f;     // How long the "hitbox" exists
    public float RecoveryTime = 0.3f;   // Delay before next attack

    [Header("Physics")]
    public AttackShape Shape = AttackShape.Circle;
    public float KnockbackForce = 5.0f;
}
