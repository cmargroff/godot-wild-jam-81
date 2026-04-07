using System;
using System.Diagnostics.CodeAnalysis;
using ShipOfTheseus2025.Services;

public sealed class ItemTrait
{
    public required string Description { get; init; }
    public required Action<IStatsManager, float> ApplyToShip { get; init; }
    public required Action<IStatsManager, float> RemoveFromShip { get; init; }

    public required float FixedValue { get; init; }

    public required float MinValue { get; init; }
    public required float MaxValue { get; init; }
    public bool ReverseColor { get; init; }

    [SetsRequiredMembers]
    public ItemTrait(RandomNumberGeneratorService rng, string description, float minValue, float maxValue, bool reverseColor,
        Action<IStatsManager, float> applyToShip,

        Action<IStatsManager, float> removeFromShip)
    {

        MinValue = minValue;
        MaxValue = maxValue;
        FixedValue = rng.GetFloatRange(minValue, maxValue);
        Description = string.Format(description, FixedValue);

        ApplyToShip = applyToShip;
        ReverseColor = reverseColor;
        RemoveFromShip = removeFromShip;
    }

    public void Apply(IStatsManager statsManager) => ApplyToShip(statsManager, FixedValue);
    public void Remove(IStatsManager statsManager) => RemoveFromShip(statsManager, FixedValue);
}
