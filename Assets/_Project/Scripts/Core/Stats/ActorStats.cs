using UnityEngine;

public class ActorStats : MonoBehaviour, IDamagable, ILevelable
{
    [Header("Data Source")]
    public RaceData Race;
    public ClassData Class;
    public LevelSettings LevelSettings;

    [Header("Attributes")]
    public Stat Strength = new();
    public Stat Dexterity = new();
    public Stat Intelligence = new();
    public Stat Vitality = new();

    [Header("Current Vitals")]
    public float CurrentHP;
    public float CurrentMP;
    public float CurrentStamina;

    [Header("Current Progress")]
    [SerializeField] private int _level = 1;
    [SerializeField] private float _currentXP;
    
    // Public Getters
    public int CurrentLevel => _level;
    public bool IsDead() => CurrentHP <= 0.0f;


    // Derived Vitals
    // TODO: Define formula for Stat -> attributes
    public float MaxHP => Race.BaseHP + (Vitality.Value * 10.0f);
    public float MaxMP => Race.BaseMP + (Intelligence.Value * 5.0f);
    public float MaxStamina => Race.BaseStamina + (Dexterity.Value * 2.0f);


    public float CurrentHealth => CurrentHP;
    public float MaxHealth => MaxHP;

    private void Awake()
    {
        InitializeStats();
    }

    private void Update()
    {
        if (CurrentStamina < MaxStamina)
        {
            // TODO: Define formula
            float regenRate = 5.0f + (Dexterity.Value * 0.5f);
            CurrentStamina = Mathf.Min(MaxStamina, CurrentStamina + regenRate * Time.deltaTime);
        }
    }

    public void InitializeStats()
    {
        if (Race == null || Class == null)
        {
            Debug.LogError($"{gameObject.name} is missing Race or Class Data!");
            return;
        }

        // Formula: RaceBase + (ClassGrowth * LevelGained)
        Strength.BaseValue = Race.BaseSTR + Mathf.RoundToInt(Class.StrGrowth * (_level - 1));
        Dexterity.BaseValue= Race.BaseDEX + Mathf.RoundToInt(Class.DexGrowth * (_level - 1));
        Intelligence.BaseValue = Race.BaseINT + Mathf.RoundToInt(Class.IntGrowth * (_level - 1));
        Vitality.BaseValue = Race.BaseVIT + Mathf.RoundToInt(Class.VitGrowth * (_level - 1));

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

    public void Heal(float amount) => CurrentHP = Mathf.Min(MaxHP, CurrentHP + amount);

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

        // TODO: Optional - Restore HP/MP on level up
        MaxVitals();
    }

    private void MaxVitals()
    {
        CurrentHP = MaxHP;
        CurrentMP = MaxMP;
        CurrentStamina = MaxStamina;
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has fallen!");

        // TODO: Add death logic and visuals
    }
}
