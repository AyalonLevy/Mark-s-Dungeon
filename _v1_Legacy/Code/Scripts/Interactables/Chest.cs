using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractale
{
    public int minCoins = 7;
    public int maxCoins = 16;
    private int _coins;
    
    [SerializeField] private bool _isOpen = false;
    [SerializeField] private Transform _lid;
    private readonly float _openAngle = -60.0f;
    private readonly float _lidOpeningTime = 0.5f;


    private void Awake()
    {
        _coins = Random.Range(minCoins, maxCoins);
    }

    public void Interact(Entity user)
    {
        if (_isOpen)
        {
            return;
        }

        _isOpen = true;
        Debug.Log($"Chest Opened! you got {_coins} gold coins! Don't spend it all in one place!");


        //TODO: Add sound effect
        StartCoroutine(OpenLid());
        GetComponentInChildren<InteractableHighlighter>().ToggleHighlight(false);
        GetComponentInChildren<InteractableHighlighter>().DisableHighlight();
    }

    private IEnumerator OpenLid()
    {
        float elapsed = 0.0f;

        Quaternion closedLid = _lid.localRotation;
        Quaternion openLid = Quaternion.Euler(_openAngle, 0.0f, 0.0f);

        while (elapsed < _lidOpeningTime)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / _lidOpeningTime;

            _lid.localRotation = Quaternion.Slerp(closedLid, openLid, t);

            yield return null;
        }

        _lid.localRotation = openLid;
    }
}
