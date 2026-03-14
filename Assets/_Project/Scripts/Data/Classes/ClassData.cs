using UnityEngine;

[CreateAssetMenu(fileName = "NewClass", menuName = "Mark's Dungeon/Data/Class")]
public class ClassData : ScriptableObject
{
    public string ClassName;

    [Header("Stat Growth per Level")]
    public float StrGrowth = 1.2f;
    public float DexGrowth = 1.0f;
    public float IntGrowth = 1.0f;
    public float VitGrowth = 1.5f;

    [Header("Starting Skills")]
    public string[] StartingSkillIDs;
}
