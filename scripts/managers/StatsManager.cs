using System;
using System.Collections.Generic;
using Godot;
using ShipOfTheseus2025.Enum;
using ShipOfTheseus2025.Models;

namespace ShipOfTheseus2025.Managers;

public class StatsManager
{
  public event Action<Stat, float> StatChanged;

  private Dictionary<Stat, float> _stats;
  public StatsManager(ConfigManager configManager)
  {
    _stats = new();
    _stats[Stat.WaterLevel] = 0.5f; //50% of the ship's height (not including the mast) is below the surface of the water
    _stats[Stat.Speed] = configManager.GetValue("shipstats", "INITIAL_SPEED").As<float>();
    _stats[Stat.Buoyancy] = configManager.GetValue("shipstats", "INITIAL_WEIGHT_TONS").As<float>() * 2000;
    _stats[Stat.Progress] = 0;
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