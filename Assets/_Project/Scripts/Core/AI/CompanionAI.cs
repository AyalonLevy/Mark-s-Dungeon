using UnityEngine;
using UnityEngine.AI;

public enum CompanionOrder { Follow, Stay, Guard }

public class CompanionAI : BaseActorAI
{
    [Header("Follow Settings")]
    [SerializeField] private Transform _leader;
    [SerializeField] private float _followDistance = 3.0f;
    [SerializeField] private float _catchUpDistance = 10.0f;

    [Header("Combat Settings")]
    [SerializeField] private float _supportRange = 5.0f;

    private CompanionOrder _currentOrder = CompanionOrder.Follow;

    private CombatHandler _combatHandler;
    private Transform _combatTarget;


    protected override void Awake()
    {
        base.Awake();
        _combatHandler = GetComponent<CombatHandler>();
    }

    public void GiveOrder(CompanionOrder newOrder)
    {
        _currentOrder = newOrder;
        Debug.Log($"{this.name} received order: {newOrder}");
    }

    protected override void UpdateAI()
    {
        if (_leader == null)
        {
            return;
        }

        if (LookForCombatTargets())
        {
            HandleCombat();
            return;
        }

        switch (_currentOrder)
        {
            case CompanionOrder.Follow:
                HandleFollow();
                break;
            case CompanionOrder.Stay:
                HandleStay();
                break;
        }
    }

    private bool LookForCombatTargets()
    {
        LayerMask mask = _combatHandler != null ? _combatHandler.TargetLayer : (LayerMask)0;

        Collider[] enemies = Physics.OverlapSphere(transform.position, _supportRange, mask);
        
        foreach (var enemy in enemies)
        {
            if (Vision.HasLineOfSight(enemy.transform))
            {
                _combatTarget = enemy.transform;
                return true;
            }
        }
        return false;
    }

    private void HandleFollow()
    {
        float distance = Vector3.Distance(transform.position, _leader.position);

        if (distance > _followDistance)
        {
            Agent.SetDestination(_leader.position);

            Moveable.IsSprinting = distance > _catchUpDistance;

            Moveable.Move(Agent.desiredVelocity.normalized);
        }
        else
        {
            StopMoving();
        }
    }

    private void HandleCombat()
    {
        if (_combatTarget == null)
        {
            return;
        }

        StopMoving();
        
        Vector3 dirToEnemy = (_combatTarget.position - transform.position).normalized;
        Moveable.Rotate(dirToEnemy);

        _combatHandler.RequestAttack();
    }

    private void HandleStay()
    {
        StopMoving();

        // TODO: Check if it is not creepy that the companion is always facing the leader
        Vector3 dirToLeader = (_leader.position - transform.position).normalized;
        Moveable.Rotate(dirToLeader);
    }
}
