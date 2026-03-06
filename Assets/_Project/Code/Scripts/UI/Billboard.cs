using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform _cam;

    void Start() => _cam = Camera.main.transform;
    private void LateUpdate() => transform.LookAt(transform.position + _cam.forward);
}
