using UnityEngine;

[CreateAssetMenu(fileName = "NewPatrolBehavior", menuName = "AI/Behavior/Patrol")]
public class PatrolBehavior : AIBehavior
{
    public float StopDistance = 0.2f;

    public override void Execute(Entity entity)
    {
        if (entity is Enemy enemy)
        {
            if (enemy.Waypoints == null || enemy.Waypoints.Count == 0)
            {
                Debug.LogWarning($"{enemy.gameObject.name} is on patrol duty but has no waypoints!");
                return;
            }

            enemy.UpdatePatrol();
        }
    }

    public override float GetStopDistance() => StopDistance;
}
