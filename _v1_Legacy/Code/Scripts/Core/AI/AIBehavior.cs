using UnityEngine;

public abstract class AIBehavior : ScriptableObject
{
    public abstract float GetStopDistance();
    public abstract void Execute(Entity entity);
}
