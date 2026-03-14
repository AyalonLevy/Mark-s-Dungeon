using UnityEngine;

public abstract class RangedWeapon : Weapon
{
    [Header("Weapon Settings")]
    [SerializeField] protected Transform[] _muzzlePoints;
    protected float _range;

    public void SetRange(float range) => _range = range;

    public abstract override void PerformAttack(Vector3 targetPosition);

    public override void PerformAttack()
    {
        Debug.LogWarning("Ranged weapon requires a target position! Call PerformAttack(Vector3) instead.");
    }
}
