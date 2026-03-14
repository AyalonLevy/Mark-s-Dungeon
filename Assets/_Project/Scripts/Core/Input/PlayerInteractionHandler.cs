using UnityEngine;

public class PlayerInteractionHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _interactionRadius = 2.0f;
    [SerializeField] private LayerMask _inteactableLayer;

    public void RequestInteraction()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, _interactionRadius, _inteactableLayer);

        foreach (var target in targets)
        {
            if (target.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact();
                break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _interactionRadius);
    }
}
