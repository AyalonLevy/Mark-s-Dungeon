using UnityEngine;

[CreateAssetMenu(fileName = "NewRace", menuName = "Mark's Dungeon/Data/Race")]
public class RaceData : ScriptableObject
{
    public string RaceName;
    public GameObject VisualPrefab;

    [Header("Base Attributes")]
    public int BaseSTR = 10;
    public int BaseDEX = 10;
    public int BaseINT = 10;
    public int BaseVIT = 10;

    [Header("Base Vitality")]
    public float BaseHP = 100.0f;
    public float BaseMP = 50.0f;
    public float BaseStamina = 100.0f;

    [Header("Sense")]
    public float DetectionRange = 5.0f;
}
