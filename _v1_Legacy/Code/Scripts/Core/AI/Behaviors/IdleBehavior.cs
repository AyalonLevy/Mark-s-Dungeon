using UnityEngine;

[CreateAssetMenu(fileName = "NewIdleBehavior", menuName = "AI/Behavior/Idle")]
public class IdleBehavior : AIBehavior
{
    public override void Execute(Entity entity)
    {
        // Will do nothing - a place to add hidden idle animations (that will play after a long time of idling
    }

    public override float GetStopDistance() => 0.1f;
}
