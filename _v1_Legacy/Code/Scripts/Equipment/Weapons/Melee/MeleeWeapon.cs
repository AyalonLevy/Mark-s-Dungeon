using UnityEngine;

public class MeleeWeapon : Weapon
{
    private Vector3 _collisionBounds;

    public override void Initialize(float damage, LayerMask targetLayer)
    {
        base.Initialize(damage, targetLayer);

        _collisionBounds = GetComponent<Collider>().bounds.size;
    }

    public override void PerformAttack()
    {
        CollisionDetection();
    }

    public void CollisionDetection()
    {
        Collider[] potentialVictims = Physics.OverlapBox(gameObject.transform.position, _collisionBounds / 2.0f, gameObject.transform.rotation, _targetLayer);

        int i = 0;

        while (i < potentialVictims.Length)
        {
            if (!_isActive)
            {
                return;
            }

            if (((1 << potentialVictims[i].gameObject.layer) & _targetLayer.value) != 0)
            {
                if (potentialVictims[i].TryGetComponent<IDamagable>(out IDamagable victim))
                {
                    victim.Damage(_damage);
                    _isActive = false;
                }
            }

            i++;
        }
    }

    //#region Helper Functions
    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    // Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
    //    if (Application.isPlaying)
    //        // Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
    //        Gizmos.DrawWireCube(transform.position, _collisionBounds);
    //}
    //#endregion
}
