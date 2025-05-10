using System;
using System.Collections.Generic;
using JamTemplate.Enum;
using JamTemplate.Models;

namespace JamTemplate.Managers;

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
    StatChanged?.Invoke(statChange.Stat, _stats[statChange.Stat]);
  }
}