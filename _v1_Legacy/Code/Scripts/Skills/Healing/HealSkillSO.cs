using UnityEngine;

[CreateAssetMenu(fileName = "NewHealSkill", menuName = "Data/Skills/Heal")]
public class HealSkillSO : SkillData
{
    public float healAmount;
    

    public override void Execute(Transform caster, IDamagable target = null)
    {
        target.CurrentHealth += healAmount;
    }
}
