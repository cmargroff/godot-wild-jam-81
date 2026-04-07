using System;
using ShipOfTheseus2025.Enum;
using ShipOfTheseus2025.Models;

public interface IStatsManager
{
  public event Action<Stat, float> StatChanged;
  public void ChangeStat(StatChange statChange);
  public float GetStats(Stat stat);
}