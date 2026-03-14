using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform _target;

    [Header("offset Settings")]
    [SerializeField] private Vector3 _offset = new(0.0f, 15.0f, -10.0f);
    [SerializeField] private float _smoothTime = 0.2f;

    private Vector3 _currentVelocity = Vector3.zero;

    private void LateUpdate()
    {
        if (_target == null)
        {
            return;
        }

        Vector3 targetPosition = _target.position + _offset;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, _smoothTime);

        transform.LookAt(_target.position + Vector3.up);
    }
}
