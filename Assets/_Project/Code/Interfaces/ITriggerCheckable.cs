using UnityEngine;

public interface ITriggerCheckable
{
    bool IsAggroed { get; set; }
    bool IsWithinStrikingDistance { get; set; }

    void SetAggroState(bool isAggroed);
    void SetStrikingDistanceBool(bool isWithinStrikingDistance);
}
