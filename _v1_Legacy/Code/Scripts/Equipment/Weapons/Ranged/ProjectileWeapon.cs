using UnityEngine;

public class ProjectileWeapon : RangedWeapon
{
    private GameObject _ammoPrefab;

    public void SetAmmo(GameObject ammo) => _ammoPrefab = ammo;

    public override void PerformAttack(Vector3 targetPosition)
    {
        if (_ammoPrefab == null)
        {
            return;
        }

        foreach (var muzzle in _muzzlePoints)
        {
            GameObject bold = Instantiate(_ammoPrefab, muzzle.position, muzzle.rotation);

            if (bold.TryGetComponent(out Projectile p))
            {
                p.Launch(targetPosition, _damage, _targetLayer);
            }
        }
    }
}
