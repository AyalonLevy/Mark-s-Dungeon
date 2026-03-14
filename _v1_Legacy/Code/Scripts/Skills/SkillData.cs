using UnityEngine;

public abstract class SkillData : ScriptableObject
{
    public string SkillName;
    public Sprite Icon;
    public float Cooldown;
    public float ManaCost;

    public abstract void Execute(Transform caster, IDamagable target = null);
}
