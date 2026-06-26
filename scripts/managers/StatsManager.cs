using System;
using System.Collections.Generic;
using ShipOfTheseus2025.Enum;
using ShipOfTheseus2025.Models;

namespace ShipOfTheseus2025.Managers;

public class StatsManager : IStatsManager
{
  public event Action<Stat, float> StatChanged;
  public ObservableStat this[Stat stat] => _stats[stat];

  private readonly Dictionary<Stat, ObservableStat> _stats;
  public StatsManager(ConfigManager configManager)
  {
    _stats = new();
    _stats[Stat.WaterLevel] = new ObservableStat(Stat.WaterLevel, 0.5f); //50% of the ship's height (not including the mast) is below the surface of the water
    _stats[Stat.Speed] = new ObservableStat(Stat.Speed, configManager.GetValue("shipstats", "INITIAL_SPEED").As<float>());
    _stats[Stat.Buoyancy] = new ObservableStat(Stat.Buoyancy, configManager.GetValue("shipstats", "INITIAL_WEIGHT_TONS").As<float>() * 2000);
    _stats[Stat.Progress] = new ObservableStat(Stat.Progress, 0f);
    _stats[Stat.WaterNoiseTime] = new ObservableStat(Stat.WaterNoiseTime, 0f);
  }

  public void ChangeStat(StatChange statChange)
  {
    // TODO: maybe change this to a switch
    if (statChange.Mode == StatChangeMode.Absolute)
    {
      _stats[statChange.Stat].Value = statChange.Amount;
    }
    else
    {
      _stats[statChange.Stat].Value = _stats[statChange.Stat].Value + statChange.Amount;
    }
    // some logic to limit the individual stats like cap water level at 100;
    StatChanged?.Invoke(statChange.Stat, _stats[statChange.Stat].Value);
  }

  public float GetStat(Stat stat)
  {
    return _stats[stat].Value;
  }
}