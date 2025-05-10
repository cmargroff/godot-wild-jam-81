using System.Collections.Generic;
using Godot;
using JamTemplate.Enum;
using JamTemplate.Models;

namespace JamTemplate.Managers;

public partial class StatsManager : Node
{
  [Signal]
  public delegate void StatChangedEventHandler(Stat stat, float amount);

  private Dictionary<Stat, float> _stats;
  public override void _EnterTree()
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
    EmitSignal(SignalName.StatChanged, Variant.From(statChange.Stat), Variant.From(_stats[statChange.Stat]));
  }
}