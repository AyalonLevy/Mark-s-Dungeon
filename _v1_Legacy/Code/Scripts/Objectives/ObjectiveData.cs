using UnityEngine;

public enum ObjectiveType
{
    Elimination,
    Survival,
    Assassination,
    Fetch
}

[CreateAssetMenu(fileName = "NewObjectiveData", menuName = "Data/Objective")]
public class ObjectiveData : ScriptableObject
{
    public ObjectiveType Type;

    [Header("Display Info")]
    public string Title;
    [TextArea] public string Description;

    [Header("Requirements")]
    public float TargetValue;

    private void OnValidate()
    {
        switch (Type)
        {
            case ObjectiveType.Elimination:
                Title = "Clear the Area";
                Description = $"Defeat {TargetValue} enemies to unlock the path.";
                break;
            case ObjectiveType.Survival:
                Title = "Hold the line";
                Description = $"Survive for {TargetValue} seconds.";
                break;
            case ObjectiveType.Assassination:
                Title = "Kill Target";
                Description = "Kill the marked enemy to proceed.";
                break;
            case ObjectiveType.Fetch:
                Title = "The Key";
                Description = "Find the hidden object to unlock the stairs.";
                break;
        }
    }
}
