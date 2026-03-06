using UnityEngine;
using UnityEngine.Rendering;

public class Minimap : MonoBehaviour
{
    [SerializeField] private Transform player;

    private void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }

    private void OnEnable() => RenderPipelineManager.beginCameraRendering += OnBeginCamera;
    private void OnDisable() => RenderPipelineManager.beginCameraRendering -= OnBeginCamera;

    private void OnBeginCamera(ScriptableRenderContext context, Camera camera)
    {
        if (camera == GetComponent<Camera>())
        {
            RenderSettings.fog = false;
        }
        else
        {
            RenderSettings.fog = true;
        }
    }
}
