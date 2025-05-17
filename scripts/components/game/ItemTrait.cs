using System;
using System.Diagnostics.CodeAnalysis;
using ShipOfTheseus2025.Managers;
using ShipOfTheseus2025.Services;

public sealed class ItemTrait
{
    public required string Description { get; init; }
    public required Action<StatsManager, float> ApplyToShip { get; init; }
    public required Action<StatsManager, float> RemoveFromShip { get; init; }

    public required float FixedValue { get; init; }

    public required float MinValue { get; init; }
    public required float MaxValue { get; init; }
    public bool ReverseColor { get; init; }

    //TODO: 

    [SetsRequiredMembers]
    public ItemTrait(RandomNumberGeneratorService rng, string description, float minValue, float maxValue, bool reverseColor,
        Action<StatsManager, float> applyToShip,

        Action<StatsManager, float> removeFromShip)
    {

        MinValue = minValue;
        MaxValue = maxValue;
        FixedValue = rng.GetFloatRange(minValue, maxValue);
        Description = string.Format(description, FixedValue);

        ApplyToShip = applyToShip;
        ReverseColor = reverseColor;
        RemoveFromShip = removeFromShip;
    }

    public void Apply(StatsManager statsManager) => ApplyToShip(statsManager, FixedValue);
    public void Remove(StatsManager statsManager) => RemoveFromShip(statsManager, FixedValue);
}
