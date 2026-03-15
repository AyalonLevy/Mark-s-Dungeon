using UnityEngine;
using UnityEngine.AI;


public enum EnemyOrder { Patrol, Guard }
public enum EnemyState { Idle, Patrol, Chase, Attack , ReturningToPost }

public class SimpleEnemyAI : BaseActorAI
{
    [Header("Strategic Assignment")]
    [SerializeField] private EnemyOrder _currentOrder = EnemyOrder.Patrol;

    [SerializeField] private Transform _guardPost;

    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private float _stopDistance = 1.5f;

    private int _currentPointIndex = 0;
    private Transform _currentTarget;
    private EnemyState _currentState = EnemyState.Patrol;

    private CombatHandler _combatHandler;
    private WeaponData _equippedWeapon;

    private float DetectionRange => (Stats != null && Stats.Race != null) ? Stats.Race.DetectionRange : 5.0f;

    protected override void Awake()
    {
        base.Awake();

        _combatHandler = GetComponent<CombatHandler>();

        if (_combatHandler != null)
        {
            _equippedWeapon = _combatHandler.GetWeaponData();
        }

        if (_currentOrder == EnemyOrder.Guard && _guardPost == null)
        {
            Debug.LogWarning($"{gameObject.name} is set to Guard but has no Guard Post assigned!");
        }
    }

    protected override void UpdateAI()
    {
        _currentTarget = FindBestTarget();

        if (_currentTarget != null)
        {
            float distance = Vector3.Distance(transform.position, _currentTarget.position);

            if (distance < _equippedWeapon.AttackRange)
            {
                _currentState = EnemyState.Attack;
            }
            else
            {
                _currentState = EnemyState.Chase;
            }
        }
        else
        {
            _currentState = (_currentOrder == EnemyOrder.Guard) ? EnemyState.ReturningToPost : EnemyState.Patrol;
        }

        ExecuteState();
    }

    private void ExecuteState()
    {
        switch (_currentState)
        {
            case EnemyState.Patrol:
                HandlePatrol();
                break;
            case EnemyState.Chase:
                HandleChase();
                break;
            case EnemyState.Attack:
                HandleAttack();
                break;
            case EnemyState.ReturningToPost:
                HandleGuardPost();
                break;
            case EnemyState.Idle:
                StopMoving();
                break;
        }
    }

    private Transform FindBestTarget()
    {
        LayerMask mask = _combatHandler.TargetLayer;
        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, DetectionRange, mask);

        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (var target in potentialTargets)
        {
            if (Vision.HasLineOfSight(target.transform))
            {
                float dist = Vector3.Distance(transform.position, target.transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closest = target.transform;
                }
            }
        }
        return closest;
    }

    private void HandlePatrol()
    {
        if (_patrolPoints.Length == 0)
        {
            return;
        }

        Transform targetPoint = _patrolPoints[_currentPointIndex];
        Agent.SetDestination(targetPoint.position);

        if (Vector3.Distance(transform.position, targetPoint.position) < _stopDistance)
        {
            _currentPointIndex = (_currentPointIndex + 1) % _patrolPoints.Length;
        }

        Moveable.Move(Agent.desiredVelocity.normalized);
    }

    private void HandleChase()
    {
        if (_currentTarget == null)
        {
            return;
        }

        Agent.SetDestination(_currentTarget.position);
        Moveable.Move(Agent.desiredVelocity.normalized);
    }

    private void HandleAttack()
    {
        if (_currentTarget == null)
        {
            return;
        }

        Moveable.Move(Vector3.zero);

        Vector3 dirToPlayer = (_currentTarget.transform.position - transform.position).normalized;
        Moveable.Rotate(dirToPlayer);

        _combatHandler.RequestAttack();
    }

    private void HandleGuardPost()
    {
        if (_guardPost == null)
        {
            StopMoving();
            return;
        }

        float distToPost = Vector3.Distance(transform.position, _guardPost.position);

        if (distToPost > 0.5f)
        {
            Agent.SetDestination(_guardPost.position);
            Moveable.Move(Agent.desiredVelocity.normalized);
        }
        else
        {
            StopMoving();
            Moveable.Rotate(_guardPost.forward);
        }
    }

    private void OnDrawGizmos()
    {
        // Detection Range (Red)
        Gizmos.color = new Color(1, 0, 0, 0.2f); // Transparent Red
        Gizmos.DrawWireSphere(transform.position, DetectionRange);

        // Current Path (Blue line to target)
        if (Agent != null && Agent.hasPath)
        {
            Gizmos.color = Color.cyan;
            var path = Agent.path;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Gizmos.DrawLine(path.corners[i], path.corners[i + 1]);
            }
        }

        if (_currentOrder == EnemyOrder.Guard && _guardPost != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(_guardPost.position, Vector3.one * 0.5f);

            Gizmos.DrawLine(transform.position, _guardPost.position);
        }
    }
}
