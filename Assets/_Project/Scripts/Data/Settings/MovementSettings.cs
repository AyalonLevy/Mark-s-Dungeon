using UnityEngine;

[CreateAssetMenu(fileName = "MovementSettings", menuName = "Mark's Dungeon/Data/Movement Settings")]
public class MovementSettings : ScriptableObject
{
    [Header("Threshold")]
    [Tooltip("Minimum input magnitude to trigger movement")]
    public float MovementThreshold = 0.1f;

    [Tooltip("Minimum velocity magnitude to be considered 'Moving' for animations")]
    public float VelocityThreshold = 0.1f;

    [Header("Physics")]
    public float RotationSpeed = 15.0f;
    public float SprintMultiplier = 1.6f;
    public float StaminaCostPerSecond = 15.0f;
}
