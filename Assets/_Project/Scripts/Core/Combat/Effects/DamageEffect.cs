using UnityEngine;

[CreateAssetMenu(fileName = "NewDamageSkill", menuName = "Mark's Dungeon/Skills/Effects/Damage")]
public class DamageEffect : SkillEffect
{
    public override void Apply(GameObject user, GameObject target)
    {
        if (target.TryGetComponent(out IDamagable damagable))
        {
            // TODO: Create formula for skill damage
            damagable.Damage(BaseValue);
        }
    }
}
