using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Setting Reference")]
    [SerializeField] private MovementSettings _settings;

    private IMoveable _moveable;
    private PlayerInteractionHandler _interactionHandler;
    private PlayerControls _controls;
    private Vector2 _rawInput;

    private void Awake()
    {
        _moveable = GetComponent<IMoveable>();
        _interactionHandler = GetComponent<PlayerInteractionHandler>();
        _controls = new PlayerControls();

        // Bind Move action
        _controls.Player.Move.performed += ctx => _rawInput = ctx.ReadValue<Vector2>();
        _controls.Player.Move.canceled += ctx => _rawInput = Vector2.zero;

        // Bind Sprint action
        _controls.Player.Sprint.performed += _ => _moveable.IsSprinting = true;
        _controls.Player.Sprint.canceled += _ => _moveable.IsSprinting = false;

        // Bind Attack action
        _controls.Player.Attack.performed += _ => OnAttackPressed();

        // Bind Interact action
        _controls.Player.Interact.performed += _ =>
        {
            Debug.Log("E Key Pressed!");
            _interactionHandler.RequestInteraction();
        };
    }

    private void OnEnable() => _controls.Enable();
    private void OnDisable() => _controls.Disable();

    private void FixedUpdate()
    {
        if (_moveable == null)
        {
            return;
        }

        Vector3 moveDir = new(_rawInput.x, 0.0f, _rawInput.y);

        if (moveDir.sqrMagnitude > (_settings.MovementThreshold * _settings.MovementThreshold))
        {
            _moveable.Move(moveDir);
        }
        else
        {
            _moveable.Move(Vector3.zero);
        }
    }

    private void OnAttackPressed()
    {
        Debug.Log("Mark attacks!");
    }
}
