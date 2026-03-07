using UnityEngine;

public class Player : Entity
{
    [Header("Dependencies")]
    [SerializeField] private InputReader inputReader;

    [Header("Interaction Settings")]
    [SerializeField] private LayerMask interactableLayer;
    private static Collider[] _interactBuffer = new Collider[8];


    private readonly float _interactRadius = 1.5f;

    protected override void Awake()
    {
        base.Awake();

        IdleState = new IdleState(this, StateMachine);
        MoveState = new MoveState(this, StateMachine);
        AttackState = new AttackState(this, StateMachine);
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();

        StateMachine.CurrentState.FrameUpdate();

        if (inputReader.IsInteracting &&!_inCombat)
        {
            TryInteract();
        }
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    #region Health / Die Functions

    protected override void OnDeath()
    {
        //TODO: Trigger gameover
        GameManager.Instance.OnPlayerDeath();

        Debug.Log("I died! (player in case you forgot....)");
    }

    #endregion

    public void TryInteract()
    {
        Vector3 searchPoint = transform.position + (Vector3.up * 0.5f) + (transform.forward * 0.5f);

        int numFound = Physics.OverlapSphereNonAlloc(searchPoint, _interactRadius, _interactBuffer, interactableLayer);

        for (int i = 0; i < numFound; i++)
        {
            if (_interactBuffer[i].TryGetComponent<IInteractale>(out var interactable))
            {
                interactable.Interact(this);
                break;
            }
        }
    }

    #region Input Implementation

    public override Vector2 GetMoveInput() => inputReader.Movement;

    public override bool IsAttacking() => inputReader.IsAttacking && CanAttack();

    public override bool IsSprinting() => inputReader.IsSprinting;

    #endregion
}
