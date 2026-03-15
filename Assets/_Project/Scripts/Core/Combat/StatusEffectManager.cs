using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{

    private class ActiveStatus
    {
        public StatusEffectData Data;
        public float RemainingTime;
        public float NextTickTime;

        public ActiveStatus(StatusEffectData data)
        {
            Data = data;
            RemainingTime = data.Duration;
            NextTickTime = data.TickInterval;
        }
    }

    private ActorStats _stats;
    private List<ActiveStatus> _activeStatuses = new();

    private void Awake() => _stats = GetComponent<ActorStats>();

    private void ApplyStatus(StatusEffectData data)
    {
        var existing = _activeStatuses.Find(s => s.Data == data);
        if (existing != null)
        {
            existing.RemainingTime = data.Duration;
            return;
        }

        ActiveStatus newStatus = new ActiveStatus(data);
        _activeStatuses.Add(newStatus);

        StartCoroutine(ProcessStatus(newStatus));
    }

    private IEnumerator ProcessStatus(ActiveStatus status)
    {
        if (status.Data.Type != StatusType.DoT)
        {
            ApplyStatChange(status.Data, true);
        }

        while (status.RemainingTime > 0)
        {
            float deltaTime = Time.deltaTime;
            status.RemainingTime -= deltaTime;
            status.NextTickTime -= deltaTime;

            if (status.Data.Type == StatusType.DoT && status.NextTickTime <= 0)
            {
                ApplyResourceTick(status.Data.ResourceToTick, status.Data.ValuePerTick);

                status.NextTickTime = status.Data.TickInterval;
            }

            yield return null;
        }

        if (status.Data.Type != StatusType.DoT)
        {
            ApplyStatChange(status.Data, false);

            // TODO: Example for healing
            _stats.CurrentHP = Mathf.Min(_stats.CurrentHP, _stats.MaxHP);
            _stats.CurrentMP = Mathf.Min(_stats.CurrentMP, _stats.MaxMP);
            _stats.CurrentStamina = Mathf.Min(_stats.CurrentStamina, _stats.MaxStamina);
        }

        _activeStatuses.Remove(status);
    }

    private void ApplyResourceTick(ResourceType resource, float amount)
    {
        switch (resource)
        {
            case ResourceType.HP:
                if (amount < 0)
                {
                    _stats.Damage(Mathf.Abs(amount));
                }
                else
                {
                    _stats.Heal(amount);
                }
                break;
            case ResourceType.MP:
                _stats.CurrentMP = Mathf.Clamp(_stats.CurrentMP + amount, 0.0f, _stats.MaxMP);
                break;
            case ResourceType.Stamina:
                _stats.CurrentStamina = Mathf.Clamp(_stats.CurrentStamina + amount, 0.0f, _stats.MaxStamina);
                break;
        }
    }

    private void ApplyStatChange(StatusEffectData data, bool isApplying)
    {
        Stat targetStat = data.StatToTarget switch
        {
            AffectedStat.Strength => _stats.Strength,
            AffectedStat.Dexterity => _stats.Dexterity,
            AffectedStat.Intelligence => _stats.Intelligence,
            AffectedStat.Vitality => _stats.Vitality,
            _ => null
        };

        if (targetStat == null)
        {
            return;
        }

        if (isApplying)
        {
            var mod = new StatModifier
            {
                Value = data.StatChangeAmount,
                Source = ModifierSource.Buff,
                Connection = data
            };
            
            targetStat.AddModifier(mod);
            Debug.Log($"Applied {data.EffectName}: {data.StatToTarget} +{data.StatChangeAmount}");
        }
        else
        {
            targetStat.RemoveModifier(data);
            Debug.Log($"Removed {data.EffectName}: {data.StatToTarget} expired");
        }
    }
}
