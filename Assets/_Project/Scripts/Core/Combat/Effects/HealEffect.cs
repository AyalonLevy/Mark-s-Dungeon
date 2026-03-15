using UnityEngine;

[CreateAssetMenu(fileName = "NewHealSkill", menuName = "Mark's Dungeon/Skills/Effects/Heal")]
public class HealEffect : SkillEffect
{
    public override void Apply(GameObject user, GameObject target)
    {
        if (target.TryGetComponent(out ActorStats stats))
        {
            // TODO: Create formula for skill damage
            stats.CurrentHP = Mathf.Min(stats.MaxHealth, stats.CurrentHP + BaseValue);
            Debug.Log($"{target.name} healed for {BaseValue}");
        }
    }
}