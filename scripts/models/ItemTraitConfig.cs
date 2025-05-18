using System;
using ShipOfTheseus2025.Managers;

public struct ItemTraitConfig
{
  public string Label;
  public float Min;
  public float Max;
  public Action<StatsManager, float> AttachCallback;
}