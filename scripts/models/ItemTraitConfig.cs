using System;
using ShipOfTheseus2025.Managers;
using ShipOfTheseus2025.Services;

public struct ItemTraitConfig
{
  public RandomNumberGeneratorService Rng;
  public string Label;
  public float Min;
  public float Max;
  public Action<StatsManager, float> AttachCallback;
  public bool ReverseColor;
}