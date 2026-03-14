using System.Collections;
using UnityEngine;

public class Companion : Enemy
{
    [Header("Companion Settings")]
    [SerializeField] private Transform _headBlock;
    public Transform Leader;

    private readonly float _desiredVelocityThreshold = 0.1f;

    protected override void Update()
    {
        base.Update();

        AimHead();
    }

    public override void ExecuteBehavior()
    {
        if (_inCombat)
        {
            return;
        }

        base.ExecuteBehavior();
    }

    private void AimHead()
    {
        if (CurrentTarget != null && _inCombat)
        {
            Vector3 targetDir = (CurrentTarget.position - _headBlock.position).normalized;

            _headBlock.rotation = Quaternion.Slerp(_headBlock.rotation, Quaternion.LookRotation(targetDir), Time.deltaTime * Data.TurnSpeed);
        }
        else
        {
            _headBlock.localRotation = Quaternion.Slerp(_headBlock.localRotation, Quaternion.identity, Time.deltaTime * Data.TurnSpeed);
        }
    }

    public override Vector2 GetMoveInput()
    {
        if (_isDead || agent == null || !agent.isOnNavMesh)
        {
            return Vector2.zero;
        }

        Transform target = _inCombat ? CurrentTarget : Leader;
        
        if (target == null)
        {
            return Vector2.zero;
        }

        float dist = Vector3.Distance(transform.position, target.position);
        float stopDist = _inCombat ? Data.AttackRange : GetActiveBehavior().GetStopDistance();
        Debug.Log($"{this.name}: distance to target: {dist}");
        if (dist <= stopDist)
        {
            return Vector2.zero;
        }

        Debug.Log($"{this.name}: I target {target.name}");
        Debug.Log($"{this.name}: pathPending: {agent.pathPending}");
        Debug.Log($"{this.name}: agent.desiredVelocity.sqrMagnitude: {agent.desiredVelocity.sqrMagnitude} > _desiredVelocityThreshold {_desiredVelocityThreshold}: {agent.desiredVelocity.sqrMagnitude > _desiredVelocityThreshold}");
        if (agent.pathPending || agent.desiredVelocity.sqrMagnitude > _desiredVelocityThreshold)
        {
            Vector3 dir = agent.desiredVelocity.normalized;
            Debug.Log($"{this.name}: Moving to target, dir: {dir}");
            return new(dir.x, dir.z);
        }

        return Vector2.zero;
    }

    private AIBehavior GetActiveBehavior() => LocalBehaviorOverride != null ? LocalBehaviorOverride : Data.DefaultBehavior;
}
