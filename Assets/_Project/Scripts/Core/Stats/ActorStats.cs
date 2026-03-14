using UnityEngine;

public class ActorStats : MonoBehaviour, IDamagable, ILevelable
{
    [Header("Blueprints")]
    public RaceData Race;
    public ClassData Class;
    public LevelSettings LevelSettings;

    [Header("Current Progress")]
    [SerializeField] private int _level = 1;
    [SerializeField] private float _currentXP;

    public int CurrentLevel => _level;

    [SerializeField] private AttributeSet attributes;
    public AttributeSet Attributes => attributes;

    [Header("Runtime Vitality")]
    public float CurrentHP;
    public float CurrentMP;
    public float CurrentStamina;

    public float CurrentHealth => CurrentHP;
    public float MaxHealth => attributes.MaxHP;

    private void Awake()
    {
        InitializeStats();
    }

    public void InitializeStats()
    {
        if (Race == null || Class == null)
        {
            Debug.LogError($"{gameObject.name} is missing Race or Class Data!");
            return;
        }

        // Race Stats
        attributes.Strength = Race.BaseSTR;
        attributes.Dexterity = Race.BaseDEX;
        attributes.Intelligence = Race.BaseINT;
        attributes.Vitality = Race.BaseVIT;

        // Apply Class Growth
        attributes.Strength += Mathf.RoundToInt(Class.StrGrowth * (_level - 1));
        attributes.Dexterity += Mathf.RoundToInt(Class.DexGrowth * (_level - 1));
        attributes.Intelligence += Mathf.RoundToInt(Class.IntGrowth * (_level - 1));
        attributes.Vitality += Mathf.RoundToInt(Class.VitGrowth * (_level - 1));

        // Calculate the derived max values
        attributes.Recalculate(_level);

        // Update stats
        MaxVitals();
    }

    public void Damage(float amount)
    {
        CurrentHP = Mathf.Max(0.0f, CurrentHP - amount);
        Debug.Log($"{gameObject.name} took {amount} damage. HP: {CurrentHP}");

        if (CurrentHP <= 0.0f)
        {
             Die();
        }
    }

    public void Heal(float amount) => CurrentHP = Mathf.Min(attributes.MaxHP, CurrentHP + amount);

    public bool IsDead() => CurrentHP <= 0.0f;

    public void AddXP(float amount)
    {
        _currentXP += amount;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        float requiredXP = LevelSettings.GetXPRequiredForLevel(_level);

        while (_currentXP >= requiredXP)
        {
            _currentXP -= requiredXP;
            _level++;
            LevelUp();

            requiredXP = LevelSettings.GetXPRequiredForLevel(_level);
        }
    }

    private void LevelUp()
    {
        Debug.Log($"{gameObject.name} reached Level {_level}!");

        InitializeStats();

        // Optional: Restore HP/MP on level up
        MaxVitals();
    }

    private void MaxVitals()
    {
        CurrentHP = Attributes.MaxHP;
        CurrentMP = Attributes.MaxMP;
        CurrentStamina = Attributes.MaxStamina;
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has fallen!");

        // TODO: Add ddeath logic and visuals
    }
}
