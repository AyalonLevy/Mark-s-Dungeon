using UnityEngine;
using UnityEngine.AI;

public abstract class BaseActorAI : MonoBehaviour
{
    protected IMoveable Moveable;
    protected NavMeshAgent Agent;
    protected ActorStats Stats;
    protected VisionProvider Vision;

    protected virtual void Awake()
    {
        Moveable = GetComponent<IMoveable>();
        Agent = GetComponent<NavMeshAgent>();
        Stats = GetComponent<ActorStats>();
        Vision = GetComponent<VisionProvider>();

        if (Agent != null)
        {
            Agent.updatePosition = false;
            Agent.updateRotation = false;
        }
    }

    protected abstract void UpdateAI();

    private void FixedUpdate()
    {
        UpdateAI();
    }
}
