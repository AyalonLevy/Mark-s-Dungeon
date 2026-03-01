using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    public InputActionAsset InputAction;

    private InputAction m_moveAction;
    private InputAction m_attackjAction;
    private InputAction m_interactAction;
    private InputAction m_sprintAction;

    public Vector2 movement { get; private set; }
    public bool IsAttacking { get; private set; }
    public bool IsInteracting { get; private set; }
    public bool IsSprinting { get; private set; }

    private void OnEnable() => InputAction.FindActionMap("Player").Enable();
    private void OnDisable() => InputAction.FindActionMap("Player").Disable();

    private void Awake()
    {
        m_moveAction = InputAction.FindAction("Move");
        m_attackjAction = InputAction.FindAction("Attack");
        m_interactAction = InputAction.FindAction("Interact");
        m_sprintAction = InputAction.FindAction("Sprint");
    }


    void Update()
    {
        movement = m_moveAction.ReadValue<Vector2>();
        IsSprinting = m_sprintAction.IsPressed();
        IsAttacking = m_attackjAction.WasPressedThisFrame();
        IsInteracting = m_interactAction.WasPressedThisFrame();
    }
}
