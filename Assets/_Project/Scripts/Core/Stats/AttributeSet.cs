using System;

[Serializable]
public class AttributeSet
{
    // The "Big Four"
    public int Strength;
    public int Dexterity;
    public int Intelligence;
    public int Vitality;

    // Derived Stats
    public float MaxHP;
    public float MaxMP;
    public float MaxStamina;

    // Calculate derived stats based on the formulas
    // TODO: Define formulas for HP/MP/Stamina
    public void Recalculate(int level)
    {
        MaxHP = (Vitality * 10) + (level * 5);
        MaxMP = (Intelligence * 8) + (level * 2);
        MaxStamina = (Dexterity * 0.8f) + (Strength * 0.2f) + 50.0f;
    }
}
