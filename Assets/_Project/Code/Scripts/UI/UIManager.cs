using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Player Reference")]
    [SerializeField] private Entity _player;

    [Header("Status Sliders")]
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private Slider _mpSlider;
    [SerializeField] private Slider _staminaSlider;
    [SerializeField] private Slider _xpSlider;

    private void Start()
    {
        if (_player != null)
        {
            UpdateAllUI();
        }
    }

    private void Update()
    {
        UpdateAllUI();
    }

    private void UpdateAllUI()
    {
        if (_player == null)
        {
            return;
        }

        _hpSlider.value = _player.CurrentHealth / _player.MaxHealth;
        _mpSlider.value = _player.CurrentMana / _player.Data.MaxMana;
        _staminaSlider.value = _player.CurrentStamina / _player.Data.MaxStamina;

        //TODO: handle XP
    }
}
