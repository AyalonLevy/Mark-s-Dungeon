using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHandler : MonoBehaviour
{
    private ActorStats _stats;
    private Dictionary<SkillData, float> _cooldownTimer = new();

    private void Awake() => _stats = GetComponent<ActorStats>();

    public void UseSkill(SkillData skill, GameObject target = null)
    {
        if (!CanAfford(skill) || IsOnCooldown(skill))
        {
            return;
        }

        foreach (var cost in skill.Costs)
        {
            PayCost(cost);
        }

        _cooldownTimer[skill] = Time.time + skill.Cooldown;

        ExecuteEffects(skill, target);
    }

    private void ExecuteEffects(SkillData skill, GameObject target)
    {
        if (skill.TargetType == SkillTargetType.Self)
        {
            ApplyToTarget(gameObject, gameObject, skill);
        }
        else if (skill.TargetType == SkillTargetType.AreaOfEffect)
        {
            CombatHandler handler = GetComponent<CombatHandler>();
            Collider[] hits = Physics.OverlapSphere(transform.position, skill.Range, handler.TargetLayer);

            foreach (var hit in hits)
            {
                ApplyToTarget(gameObject, hit.gameObject, skill);
            }
        }
        else
        {
            if (target != null)
            {
                ApplyToTarget(gameObject, target, skill);
            }
        }
    }

    private void ApplyToTarget(GameObject user, GameObject target, SkillData skill)
    {
        CombatHandler userCombatHandler = user.GetComponent<CombatHandler>();

        foreach (var effect in skill.Effects)
        {
            bool isEnemy = ((1 << target.layer) & userCombatHandler.TargetLayer) != 0;


            if (effect.Alignment == EffectAlignment.Negative && isEnemy)
            {
                effect.Apply(user, target);
            }
            else if (effect.Alignment == EffectAlignment.Positive && !isEnemy)
            {
                effect.Apply(user, target);
            }
            else if (effect.Alignment == EffectAlignment.Neutral)
            {
                effect.Apply(user, target); // Affects everyone
            }
        }
    }

    private bool CanAfford(SkillData skill)
    {
        foreach (var cost in skill.Costs)
        {
            float current = cost.Resource switch
            {
                ResourceType.HP => _stats.CurrentHP,
                ResourceType.MP => _stats.CurrentMP,
                ResourceType.Stamina => _stats.CurrentStamina,
                _ => 0
            };

            if (current < cost.Amount)
            {
                return false;
            }
        }
        return true;
    }

    private void PayCost(SkillCost cost)
    {
        switch (cost.Resource)
        {
            case ResourceType.HP:
                _stats.CurrentHP -= cost.Amount;
                break;
            case ResourceType.MP:
                _stats.CurrentMP -= cost.Amount;
                break;
            case ResourceType.Stamina:
                _stats.CurrentStamina -= cost.Amount;
                break;
        }
    }

    private bool IsOnCooldown(SkillData skill) => _cooldownTimer.ContainsKey(skill) && Time.time < _cooldownTimer[skill];
}
