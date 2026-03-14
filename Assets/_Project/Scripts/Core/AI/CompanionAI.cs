using UnityEngine;
using UnityEngine.AI;

public class CompanionAI : BaseActorAI
{
    [Header("Follow Settings")]
    [SerializeField] private Transform _target;
    [SerializeField] private float _stopDistance = 3.0f;
    [SerializeField] private float _catchUpDistance = 10.0f;


    protected override void UpdateAI()
    {
        if (_target == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, _target.position);

        if (distance > _stopDistance)
        {
            Agent.SetDestination(_target.position);

            Vector3 direction = Agent.desiredVelocity.normalized;

            Moveable.IsSprinting = distance > _catchUpDistance;

            Moveable.Move(direction);
        }
        else
        {
            Moveable.Move(Vector3.zero);
            Moveable.IsSprinting = false;
        }
    }
}
