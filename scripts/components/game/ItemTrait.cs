using System;
using System.Diagnostics.CodeAnalysis;
using Godot;
using ShipOfTheseus2025.Managers;
using ShipOfTheseus2025.Services;

public sealed class ItemTrait
{
    public required string Description { get; init; }
    public required Action<StatsManager, float> ApplyToShip { get; init; }

    public required float FixedValue { get; init; }

    public required float MinValue { get; init; }
    public required float MaxValue { get; init; }
    public bool ReverseColor { get; init; } 

    //TODO: 
    [SetsRequiredMembers]
    public ItemTrait(RandomNumberGeneratorService rng, string description, float minValue, float maxValue, Action<StatsManager, float> applyToShip, bool reverseColor)
    {
        MinValue = minValue;
        MaxValue = maxValue;
        FixedValue = rng.GetFloatRange(minValue, maxValue);
        Description = description.Replace("[%placeholder%]", (Mathf.RoundToInt(FixedValue * 100) / 100f).ToString());

        ApplyToShip = applyToShip;
        ReverseColor = reverseColor;
    }
    public static ItemTrait FromConfig(RandomNumberGeneratorService rng, ItemTraitConfig config)
    {
        return new ItemTrait(rng, config.Label, config.Min, config.Max, config.AttachCallback);
    }
}
