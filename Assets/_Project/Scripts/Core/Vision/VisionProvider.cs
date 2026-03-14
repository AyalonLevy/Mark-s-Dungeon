using UnityEngine;

public class VisionProvider : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private float _eyeHeight = 1.5f;

    public Vector3 EyePosition => transform.position + Vector3.up * _eyeHeight;

    public bool HasLineOfSight(Transform target)
    {
        if (target == null)
        {
            return false;
        }

        Vector2 targetPos = target.position + Vector3.up * _eyeHeight;

        bool isBlocked = Physics.Linecast(EyePosition, targetPos, _obstacleMask);

        return !isBlocked;
    }

    public bool CanSee(Transform target, float range)
    {
        if (target == null)
        {
            return false; 
        }

        float distanceSqr = (target.position - transform.position).sqrMagnitude;
        if (distanceSqr > (range * range))
        {
            return false;
        }

        return HasLineOfSight(target);
    }
}
