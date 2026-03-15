using UnityEngine;

public enum StatusType { DoT, Buff, Debuff }
public enum AffectedStat { Strength, Dexterity, Intelligence, Vitality }

[CreateAssetMenu(fileName = "NewStatus", menuName = "Mark's Dungeon/Skills/Status Effect")]
public class StatusEffectData : ScriptableObject
{
    public string EffectName;
    public StatusType Type;
    public float Duration = 5.0f;
    public float TickInterval = 1.0f;  // Only for damage ofver time (DoT)

    [Header("Instant/Tick Values (For DoT/HoT")]
    public ResourceType ResourceToTick;
    public float ValuePerTick;

    [Header("Stat Modification ( For Buffs/Debuffs)")]
    public AffectedStat StatToTarget;
    public float StatChangeAmount;
}
