public interface IDamagable
{
    float CurrentHealth { get; }
    float MaxHealth { get; }

    void Damage(float amount);
    void Heal(float amount);
    bool IsDead();
}
