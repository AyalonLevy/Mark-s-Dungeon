using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "Mark's Dungeon/Data/Level Settings")]
public class LevelSettings : ScriptableObject
{
    // TODO: Define formula for XP required based on level
    public float GetXPRequiredForLevel(int level)
    {
        return level * 100.0f;
    }
}
