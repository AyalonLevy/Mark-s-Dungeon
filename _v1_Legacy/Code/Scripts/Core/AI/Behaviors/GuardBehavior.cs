using UnityEngine;

[CreateAssetMenu(fileName = "NewGuardBehavior", menuName = "AI/Behavior/Guard")]
public class GuardBehavior : AIBehavior
{
    public float GuardRadius = 5.0f;
    public float GuardStopDistance = 0.5f;

    public override float GetStopDistance() => GuardStopDistance;

    public override void Execute(Entity entity)
    {
        if (entity is Enemy enemy)
        {
            // Walk randomaly in the allowed area (leashed to the guarding object
            enemy.UpdateGuard(this);
        }
    }
}
