using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Player Reference")]
    [SerializeField] private Entity _player;

    [Header("Status Sliders")]
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private Slider _mpSlider;
    [SerializeField] private Slider _staminaSlider;
    [SerializeField] private Slider _xpSlider;

    [Header("Screens")]
    [SerializeField] private GameObject _hudCanvas;
    [SerializeField] private GameObject _gameOverScreen;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
                }
        else 
        {
            Destroy(gameObject);
        }

        _gameOverScreen.SetActive(false);
        _hudCanvas.SetActive(true);
    }

    public void ShowGameOver()
    {
        _hudCanvas.SetActive(false);
        _gameOverScreen.SetActive(true);
    }

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
