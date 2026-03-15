using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "NewSkill", menuName = "Mark's Dungeon/Data/Skill")]
public class SkillData : ScriptableObject
{
    public string SkillName;
    public SkillTargetType TargetType;

    [Header("Requirements")]
    public float Range = 3.0f;
    public float Cooldown = 1.0f;
    public List<SkillCost> Costs;

    [Header("Effects")]
    public List<SkillEffect> Effects;
}
