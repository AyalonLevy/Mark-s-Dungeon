using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    private float _damage;
    private bool _isActive;
    private LayerMask _targetLayer;

    public void Initialize(float damage, LayerMask targetLayer)
    {
        _damage = damage;
        _targetLayer = targetLayer;
    }

    public void SetActiveState(bool active) => _isActive = active;

    private void OnTriggerEnter(Collider other)
    {
        if (!_isActive)
        {
            return;
        }

        if (((1 << other.gameObject.layer) & _targetLayer) !=0)
        {
            if (other.TryGetComponent<IDamagable>(out IDamagable victim))
            {
                victim.Damage(_damage);
                _isActive = false;  // turn off to prevent hitting the same enemy again (with the same swing)
            }
        }
    }
}
