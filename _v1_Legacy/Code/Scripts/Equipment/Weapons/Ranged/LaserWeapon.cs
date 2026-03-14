using System.Collections;
using UnityEngine;

public class LaserWeapon : RangedWeapon
{
    [SerializeField] private LineRenderer[] _lasers;
    [SerializeField] private float _beamVisibleDuration = 0.15f;
    public override void PerformAttack(Vector3 targetPosition)
    {
        _isActive = true;

        for (int i = 0; i < _muzzlePoints.Length; i++)
        {
            Transform muzzle = _muzzlePoints[i];
            Vector3 direction = (muzzle.position - targetPosition).normalized;
            Vector3 endPoint = muzzle.position + (direction * _range);

            if (Physics.Raycast(muzzle.position, direction, out RaycastHit hit, _range, _targetLayer))
            {
                endPoint = hit.point;

                if (hit.collider.TryGetComponent(out IDamagable victim))
                {
                    victim.Damage(_damage);
                }
            }

            if (i < _lasers.Length)
            {
                StartCoroutine(ShowBeam(_lasers[i], muzzle.position, endPoint));
            }
        }
    }

    private IEnumerator ShowBeam(LineRenderer line, Vector3 start, Vector3 end)
    {
        line.enabled = true;
        line.SetPosition(0, start);
        line.SetPosition(1, end);

        yield return new WaitForSeconds(_beamVisibleDuration);

        line.enabled = false;
    }
}
