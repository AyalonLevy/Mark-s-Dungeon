using UnityEngine;

[CreateAssetMenu(fileName = "NewFollowBehavior", menuName = "AI/Behavior/Follow")]
public class FollowBehavior : AIBehavior
{
    [SerializeField] private float followDistance = 1.0f;
    [SerializeField] private float StopDistance = 0.5f;

    public override void Execute(Entity entity)
    {
        if (entity is Companion companion && companion.Leader != null)
        {
            float distance = Vector3.Distance(entity.transform.position, companion.Leader.position);

            if (distance > followDistance)
            {
                companion.SetDestination(companion.Leader.position);

                if (companion.StateMachine.CurrentState == companion.IdleState)
                {
                    companion.StateMachine.ChangeState(companion.MoveState);
                }
            }
            else if (distance <= StopDistance)
            {
                if (companion.StateMachine.CurrentState == companion.MoveState)
                {
                    companion.StateMachine.ChangeState(companion.IdleState);
                }
            }
        }
    }

    public override float GetStopDistance() => StopDistance;
}
