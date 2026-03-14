using System.Collections;
using UnityEngine;

public class ForceFieldController : MonoBehaviour
{
    [Header("settings")]
    [SerializeField] private float _fadeDuration = 1.5f;
    [SerializeField] private MeshRenderer _portal;

    private Collider _collider;
    private MeshRenderer _renderer;
    private Material _material;
    private Material _portalMaterial;


    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _renderer = GetComponent<MeshRenderer>();
        _material = _renderer.material;
        _portalMaterial = _portal.material;
    }

    public void DeactivateForceField()
    {
        _collider.enabled = false;

        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {
        float elapsed = 0.0f;

        float startAlpha = _material.GetFloat("_AlphaScale");
        Color startEmission = _material.GetColor("_Emission");
        float startPortal = _portalMaterial.GetFloat("_Fade");

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _fadeDuration;

            float newAlpha = Mathf.Lerp(startAlpha, 0.0f, t);
            _renderer.material.SetFloat("_AlphaScale", newAlpha);

            Color newEmission = Color.Lerp(startEmission, Color.black, t);
            _renderer.material.SetColor("_Emission", newEmission);

            // Make the portal visible
            float newFade = Mathf.Lerp(startPortal, 1.0f, t);
            _portal.material.SetFloat("_Fade", newFade);

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
