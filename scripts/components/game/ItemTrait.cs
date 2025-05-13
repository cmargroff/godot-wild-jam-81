using ShipOfTheseus2025.Managers;
using ShipOfTheseus2025.Services;
using System;
using System.Diagnostics.CodeAnalysis;

public sealed class ItemTrait
{
    public required string Description { get; init; }
    public required Action<StatsManager, float> ApplyToShip { get; init; }

    public required float FixedValue { get; init; }

    public required float MinValue { get; init; }
    public required float MaxValue { get; init; }

    //TODO: 
    [SetsRequiredMembers]
    public ItemTrait(RandomNumberGeneratorService rng, string description, float minValue, float maxValue, Action<StatsManager, float> applyToShip)
    {
        
        MinValue = minValue;
        MaxValue = maxValue;
        FixedValue = rng.GetFloatRange(minValue, maxValue);
        Description = description.Replace("[%placeholder%]", FixedValue.ToString());

        ApplyToShip = applyToShip;
    }
}
