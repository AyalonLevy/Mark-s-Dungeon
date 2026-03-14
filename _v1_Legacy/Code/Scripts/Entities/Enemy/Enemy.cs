using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    [Header("Behavior Overrides")]
    [Tooltip("If set, this overrides the default behavior in the EntityData SO")]
    public AIBehavior LocalBehaviorOverride;

    [Header("Partol Settings")]
    public List<Transform> Waypoints;
    [SerializeField] private float _waitAtWaypoint = 2.0f;
    private int _currentWaypointIndex = 0;
    private float _waypointTimer;
    private bool _isWaiting;

    [Header("Guard Settings")]
    [Tooltip("The object to guard. If null, guards its spawn pposition")]
    public Transform GuardObject;
    private Vector3 _guardAnchor;

    private readonly float _targetPositionThreshold = 0.5f;
    private readonly float _attackRangeMultiplier = 1.3f;


    protected override void Start()
    {
        base.Start();

        if (GuardObject != null)
        {
            _guardAnchor = GuardObject.position;
        }

        if (!_inCombat && Waypoints != null && Waypoints.Count > 0)
        {
            _currentWaypointIndex = 0;
            SetTarget(Waypoints[_currentWaypointIndex]);
        }
    }

    protected override void HandleBehavior()
    {
        if (_inCombat && CurrentTarget != null)
        {
            if (!agent.hasPath || Vector3.Distance(agent.destination, CurrentTarget.position) > _targetPositionThreshold)
            { 
                SetDestination(CurrentTarget.position);
            }

            return;
        }

        AIBehavior active = LocalBehaviorOverride != null ? LocalBehaviorOverride : Data.DefaultBehavior;
        if (active != null)
        {
            active.Execute(this);
        }
    }

    #region Specific Behavior Logic

    public void UpdatePatrol()
    {
        if (Waypoints == null || Waypoints.Count == 0)
        {
            return;
        }

        if (_isWaiting)
        {
            _waypointTimer -= Time.deltaTime;
            if (_waypointTimer <= 0)
            {
                _isWaiting = false;
            }
            return;
        }

        CurrentTarget = Waypoints[_currentWaypointIndex];

        if (!agent.pathPending && agent.remainingDistance < GetActiveBehavior().GetStopDistance())
        {
            _isWaiting = true;
            _waypointTimer = _waitAtWaypoint;

            _currentWaypointIndex = (_currentWaypointIndex + 1) % Waypoints.Count;
            SetDestination(Waypoints[_currentWaypointIndex].position);
        }
    }

    public void UpdateGuard(GuardBehavior settings)
    {
        if (_isWaiting)
        {
            _waypointTimer -= Time.deltaTime;
            if (_waypointTimer < 0)
            {
                _isWaiting = false;
            }
            return;
        }

        if (agent.remainingDistance < settings.GetStopDistance() && !agent.pathPending)
        {
            _isWaiting = true;
            _waypointTimer = _waitAtWaypoint;

            Vector3 anchor = GuardObject != null ? GuardObject.position : _guardAnchor;
            Vector2 randomPoint = Random.insideUnitCircle * settings.GuardRadius;
            Vector3 targetPos = anchor + new Vector3(randomPoint.x, 0.0f, randomPoint.y);

            if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, settings.GuardRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
    }

    #endregion

    #region Physical Implementation

    public override Vector2 GetMoveInput()
    {
        if (_isDead || !agent.isOnNavMesh)
        {
            return Vector2.zero;
        }

        float distToTarget = CurrentTarget != null ? Vector3.Distance(transform.position, CurrentTarget.position) : Mathf.Infinity;

        // --- COMBAT OVERRIDE ---
        // The enemy will target the player directly, ignoring NavMesh
        if (_inCombat && CurrentTarget != null)
        {
            bool hasDirectLine = !Physics.Raycast(transform.position + Vector3.up, (CurrentTarget.position - transform.position), distToTarget, Data.ObstacleLayer);

            if (hasDirectLine && distToTarget < Data.AttackRange * _attackRangeMultiplier)
            {
                if (distToTarget <= Data.AttackRange)
                {
                    return Vector2.zero;
                }

                Vector3 directDir = (CurrentTarget.position - transform.position).normalized;
                return new(directDir.x, directDir.z);
            }
        }
        
        if (!agent.hasPath || agent.pathPending || !agent.isOnNavMesh)
        {
            return Vector2.zero;
        }

        float stopRange = _inCombat ? Data.AttackRange : GetActiveBehavior().GetStopDistance();

        if (agent.remainingDistance <= stopRange)
        {
            return Vector2.zero;
        }

        Vector3 nextDir = agent.desiredVelocity.normalized;
        return new(nextDir.x, nextDir.z);
    }

    public override bool IsAttacking()
    {
        if (CurrentTarget == null || !_inCombat)
        {
            return false;
        }

        float distSqr = (CurrentTarget.position - transform.position).sqrMagnitude;
        float attackRangeSqr = Data.AttackRange * Data.AttackRange;

        bool shouldAttack = distSqr <= attackRangeSqr && CanAttack();

        return shouldAttack;
    }

    public override bool IsSprinting()
    {
        //TODO for now, we'll see if enemies will have sprint speed while in aggro mode or something
        return _inCombat;
    }

    #endregion

    #region Health / Die Functions

    protected override void OnDeath()
    {
        Debug.Log("Alas I died! (enmy/companion in case you forgot....)");
    }

    #endregion

    #region Behavior Functions

    private AIBehavior GetActiveBehavior() => LocalBehaviorOverride != null ? LocalBehaviorOverride : Data.DefaultBehavior;

    public override void ExecuteBehavior()
    {
        if (_inCombat)
        {
            return;
        }

        AIBehavior active = GetActiveBehavior();
        if (active != null)
        {
            active.Execute(this);
        }
    }

    #endregion

    #region Helper Functions

    private void OnDrawGizmosSelected()
    {
        if (Data == null) return;

        // Visualizing the search "Pulse" range
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, Data.DetectionRange);

        // Visualize FOV
        Vector3 leftRayDirection = Quaternion.AngleAxis(-Data.ViewAngle / 2.0f, Vector3.up) * transform.forward;
        Vector3 rightRayDirection = Quaternion.AngleAxis(Data.ViewAngle / 2.0f, Vector3.up) * transform.forward;
        Gizmos.DrawRay(transform.position, leftRayDirection * Data.DetectionRange);
        Gizmos.DrawRay(transform.position, rightRayDirection * Data.DetectionRange);

        // Visualize attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Data.AttackRange);

        // Visualize proximity range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Data.ProximitySense);

        // Visualize waypoints
        DrawWaypoints();
    }

    private void DrawWaypoints()
    {
        if (Waypoints != null && Waypoints.Count > 1)
        {
            Gizmos.color = Color.aliceBlue;
            for (int i = 0; i < Waypoints.Count; i++)
            {
                if (Waypoints[i] == null)
                {
                    continue;
                }

                Gizmos.DrawSphere(Waypoints[i].position, 0.2f);

                int nextWaypoint = (i + 1) % Waypoints.Count;
                if (Waypoints[nextWaypoint] != null)
                {
                    Gizmos.DrawLine(Waypoints[i].position, Waypoints[nextWaypoint].position);
                }
            }
        }
    }

    #endregion
}
