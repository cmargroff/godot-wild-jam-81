using System;
using System.Collections.Generic;
using Godot;
using ShipOfTheseus2025.Enum;
using ShipOfTheseus2025.Models;

namespace ShipOfTheseus2025.Managers;

public partial class StatsManager
{
  public event Action<Stat, float> StatChanged;

  private Dictionary<Stat, float> _stats;
  public StatsManager()
  {
    _stats = new();
    foreach (Stat id in System.Enum.GetValues(typeof(Stat)))
    {
      _stats.Add(id, 0);
    }
  }
  public void ChangeStat(StatChange statChange)
  {
    // TODO: maybe change this to a switch
    if (statChange.Mode == StatChangeMode.Absolute)
    {
      _stats[statChange.Stat] = statChange.Amount;
    }
    else
    {
      _stats[statChange.Stat] += statChange.Amount;
    }
    // some logic to limit the individual stats like cap water level at 100;
    if (statChange.Stat == Stat.Speed)
    {
      GD.Print($"stats changed {statChange.Stat}");
    
    }
    StatChanged?.Invoke(statChange.Stat, _stats[statChange.Stat]);
  }

  public float GetStats(Stat stat)
  {
    return _stats[stat];
  }
}