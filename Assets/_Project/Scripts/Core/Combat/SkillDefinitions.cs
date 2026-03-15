using System;

public enum ResourceType { HP, MP, Stamina }
public enum SkillTargetType { Self, SingleTarget, AreaOfEffect }

[Serializable]
public struct SkillCost
{
    public ResourceType Resource;
    public float Amount;
}
