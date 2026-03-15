using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public enum ModifierSource { Equipment, Buff }

[Serializable]
public class StatModifier
{
    public float Value;
    public ModifierSource Source;
    public object Connection;  // The Item os StatusEffectData that caused the change
}

[Serializable]
public class Stat
{
    [SerializeField] private float _baseValue;
    public float BaseValue { get => _baseValue; set => _baseValue = value; }

    private List<StatModifier> _modifiers = new();

    public float Value => _baseValue + _modifiers.Sum(m => m.Value);

    public void AddModifier(StatModifier mod) => _modifiers.Add(mod);

    public void RemoveModifier(object connection) => _modifiers.RemoveAll(m => m.Connection == connection);
}
