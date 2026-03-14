using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody), typeof(ActorStats))]
public class EntityMovement : MonoBehaviour, IMoveable
{
    [Header("Settings Reference")]
    [SerializeField] private MovementSettings _settings;

    private Rigidbody _rb;
    private NavMeshAgent _agent;
    private ActorStats _stats;

    public bool IsSprinting { get; set; }

    public float CurrentMoveSpeed
    {
        get
        {
            // TODO: Define formula for speed
            float speed = 2.0f + (_stats.Attributes.Dexterity * 0.1f);

            // Add encumbrance by load of gear and inventory
            // speed *= _stats.GetEncumbranceMultiplier();  // TODO: Create this later

            if (IsSprinting && _stats.CurrentStamina > 0.0f)
            {
                return speed * _settings.SprintMultiplier;
            }

            return speed;
        }
    }


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
        _stats = GetComponent<ActorStats>();

        // To prevent the agent from moving the actors
        if (_agent != null)
        {
            _agent.updatePosition = false;
            _agent.updateRotation = false;
        }

        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void FixedUpdate()
    {
        // Handle Stamina depletion
        if (IsSprinting && _rb.linearDamping > (_settings.VelocityThreshold * _settings.VelocityThreshold))
        {
            _stats.CurrentStamina -= _settings.StaminaCostPerSecond * Time.fixedDeltaTime;

            if (_stats.CurrentStamina <= 0.0f)
            {
                IsSprinting = false;
            }
        }
    }

    public void Move(Vector3 direction)
    {
        Vector3 targetVelocity = direction * CurrentMoveSpeed;
        targetVelocity.y = _rb.linearVelocity.y;
        _rb.linearVelocity = targetVelocity;

        // Look where we are going
        if (direction.sqrMagnitude > (_settings.MovementThreshold * _settings.MovementThreshold))
        {
            Rotate(direction);
        }

        // Sync NavMeshAgent position
        if (_agent != null && _agent.isOnNavMesh)
        {
            _agent.nextPosition = transform.position;
        }
    }

    public void Rotate(Vector3 lookDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _settings.RotationSpeed * Time.deltaTime);
    }
}
