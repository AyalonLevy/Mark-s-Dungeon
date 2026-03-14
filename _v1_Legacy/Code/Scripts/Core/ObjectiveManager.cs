using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private ObjectiveData _currentObjective;
    private List<Entity> _targets = new List<Entity>();

    [Header("References")]
    [SerializeField] private ForceFieldController _forceField;

    private int _targetEliminated = 0;
    private float _progress = 0.0f;


    private void Start()
    {
        if (_currentObjective.Type == ObjectiveType.Elimination)
        {
            SetupElimination();
        }

        if (_currentObjective.Type == ObjectiveType.Assassination)
        {
            SetupAssassination();
        }
    }

    private void OnEnable() => Entity.OnAnyEntityDeath += HandleGlobalDeath;
    private void OnDisable() => Entity.OnAnyEntityDeath -= HandleGlobalDeath;

    private void SetupElimination()
    {
        Entity[] potentialTargets = Object.FindObjectsByType<Entity>(FindObjectsSortMode.None);
        
        foreach (var target in potentialTargets)
        {
            if (target.CompareTag("Enemy"))
            {
                _targets.Add(target);
            }
        }
    }

    private void SetupAssassination()
    {
        Entity[] potentialTargets = Object.FindObjectsByType<Entity>(FindObjectsSortMode.None);

        foreach (var target in potentialTargets)
        {
            if (target.CompareTag("Enemy") && target.isMarked)
            {
                _targets.Add(target);
            }
        }
    }

    private void HandleGlobalDeath(Entity victim)
    {
        if (victim.CompareTag("Enemy"))
        {
            // For Elimination type, every death is counting for the progress. For assassination, only marked targets counts
            if (_currentObjective.Type == ObjectiveType.Assassination && !victim.isMarked)
            {
                return;
            }

            UpdateObjectiveProgress(); 
        }
    }

    private void UpdateObjectiveProgress()
    {
        _targetEliminated++;
        _progress = (float)_targetEliminated / _currentObjective.TargetValue;

        if (_targetEliminated >= _currentObjective.TargetValue)
        {
            CompleteObjective();
        }
    }

    private void CompleteObjective()
    {
        Debug.Log("Objective Complete! proceed to the exit");

        if (_forceField != null)
        {
            _forceField.DeactivateForceField();
        }
    }
}
