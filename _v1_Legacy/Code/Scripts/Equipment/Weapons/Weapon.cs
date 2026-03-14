using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected float _damage;
    protected bool _isActive;
    protected LayerMask _targetLayer;

    public virtual void Initialize(float damage, LayerMask targetLayer)
    {
        _damage = damage;
        _targetLayer = targetLayer;
    }
    // For Melee
    public abstract void PerformAttack();

    // For Ranged
    public virtual void PerformAttack(Vector3 targetPosition) { }

    public void SetActiveState(bool active) => _isActive = active;

    public void DisableWeapon()
    {
        this.enabled = false;
        if (TryGetComponent<Collider>(out var col))
        {
            col.enabled = false;
        }
        //gameObject.GetComponent<Collider>().enabled = false;
    }

    public void EnableWeapon()
    {
        this.enabled = true;
        if (TryGetComponent<Collider>(out var col))
        {
            col.enabled = true;
        }
        //gameObject.GetComponent<Collider>().enabled = true;
    }

}
