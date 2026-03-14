using UnityEngine;
using UnityEngine.AI;


public enum EnemyState { Idle, Patrol, Chase }

public class SimpleEnemyAI : BaseActorAI
{
    [Header("Detection")]
    [SerializeField] private Transform _player;
    [SerializeField] private float _detectionRange = 8.0f;

    [Header("Patrol")]
    [SerializeField] private Transform[] _partolPoints;
    private int _currentPointIndex = 0;
    private float _stopDistance = 1.5f;

    private EnemyState _currentState = EnemyState.Patrol;

    protected override void UpdateAI()
    {
        if (Vision != null && Vision.CanSee(_player, _detectionRange))
        {
            _currentState = EnemyState.Chase;
        }
        else
        {
            _currentState = EnemyState.Patrol;
        }

        switch (_currentState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Patrol:
                HandlePatrol();
                break;
            case EnemyState.Chase:
                HandleChase();
                break;
        }
    }

    private void HandlePatrol()
    {
        if (_partolPoints.Length == 0)
        {
            return;
        }

        Transform targetPoint = _partolPoints[_currentPointIndex];
        Agent.SetDestination(targetPoint.position);

        if (Vector3.Distance(transform.position, targetPoint.position) < _stopDistance)
        {
            _currentPointIndex = (_currentPointIndex + 1) % _partolPoints.Length;
        }

        Moveable.Move(Agent.desiredVelocity.normalized);
    }

    private void HandleChase()
    {
        Agent.SetDestination(_player.position);
        Moveable.Move(Agent.desiredVelocity.normalized);
    }

    private void OnDrawGizmos()
    {
        // Detection Range (Red)
        Gizmos.color = new Color(1, 0, 0, 0.3f); // Transparent Red
        Gizmos.DrawWireSphere(transform.position, _detectionRange);

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
    }
}
