using System.Collections;
using UnityEngine;

public class Gate : MonoBehaviour, IInteractale
{
    public bool isLocked = false;

    [SerializeField] private bool _isOpen = false;
    [SerializeField] private Transform _gate;
    private readonly float _openGate = 0.7f;
    private readonly float _gateOpeningTime = 0.8f;

    public void Interact(Entity user)
    {
        if (_isOpen || isLocked)
        {
            return;
        }

        _isOpen = true;

        //TODO: Add sound effect
        StartCoroutine(OpenGate());
        GetComponentInChildren<InteractableHighlighter>().ToggleHighlight(false);
        GetComponentInChildren<InteractableHighlighter>().DisableHighlight();
    }

    private IEnumerator OpenGate()
    {
        float elapsed = 0.0f;

        float closeGate = _gate.position.y;

        while (elapsed < _gateOpeningTime)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / _gateOpeningTime;

            _gate.position = new(_gate.position.x, Mathf.Lerp(closeGate, _openGate, t), _gate.position.z);

            yield return null;
        }

        _gate.position = new(_gate.position.x, _openGate, _gate.position.z);

        gameObject.GetComponent<Collider>().enabled = false;
    }

}
