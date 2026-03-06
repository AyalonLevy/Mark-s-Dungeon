using UnityEngine;

public class InteractableHighlighter : MonoBehaviour
{
    public LayerMask layer;
    public Outline outline;

    private bool _interacted = false;

    private void Awake()
    {
        outline.enabled = false;
    }

    public void DisableHighlight() => _interacted = true;

    public void ToggleHighlight(bool show)
    {
       if (outline == null || _interacted)
        {
            return;
        }

       outline.enabled = show;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) &  layer.value) > 0)
        {
            ToggleHighlight(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & layer.value) > 0)
        {
            ToggleHighlight(false);
        }
    }
}
