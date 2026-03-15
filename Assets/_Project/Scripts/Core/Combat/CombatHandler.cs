using System.Collections;
using UnityEngine;

public class CombatHandler : MonoBehaviour
{
    [SerializeField] private WeaponData _equippedWeapon;
    [SerializeField] private LayerMask _targetLayer;

    private ActorStats _stats;
    private bool _isAttacking;

    public LayerMask TargetLayer => _targetLayer;

    private void Awake()
    {
        _stats = GetComponent<ActorStats>();
    }

    public void RequestAttack()
    {
        if (_isAttacking || _equippedWeapon == null)
        {
            return;
        }

        if (_stats.CurrentStamina < _equippedWeapon.StaminaCost)
        {
            return;
        }

        StartCoroutine(ExecuteAttackRooutine());
    }

    private IEnumerator ExecuteAttackRooutine()
    {
        _isAttacking = true;
        _stats.CurrentStamina -= _equippedWeapon.StaminaCost;

        // Wind-up
        yield return new WaitForSeconds(_equippedWeapon.WindUpTime);

        // Hit (if there is a target)
        PerformDetection();

        yield return new WaitForSeconds(_equippedWeapon.ActiveTime);

        // Rcovery
        yield return new WaitForSeconds(_equippedWeapon.RecoveryTime);
        _isAttacking = false;
    }

    private void PerformDetection()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _equippedWeapon.AttackRange, _targetLayer);

        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject)
            {
                continue;
            }

            if (hit.TryGetComponent(out IDamagable target))
            {
                // TODO: Define formula
                float finalDamage = _equippedWeapon.Damage + (_stats.Strength.Value * 0.5f);
                target.Damage(finalDamage);
                Debug.Log($"{gameObject.name} hit {hit.name} for {_equippedWeapon.Damage} damage");
            }
        }
    }

    public WeaponData GetWeaponData()
    {
        return _equippedWeapon;
    }

    private void OnDrawGizmosSelected()
    {
        if (_equippedWeapon == null)
        {
            return;
        }

        Gizmos.color = _isAttacking ? Color.red : Color.white;
        Gizmos.DrawWireSphere(transform.position, _equippedWeapon.AttackRange);
    }
}
