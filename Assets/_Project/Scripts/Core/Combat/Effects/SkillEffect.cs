using UnityEngine;

public enum EffectAlignment { Positive, Negative, Neutral }

public abstract class SkillEffect : ScriptableObject
{
    public EffectAlignment Alignment;
    public float BaseValue;
    public abstract void Apply(GameObject user, GameObject target);
}
