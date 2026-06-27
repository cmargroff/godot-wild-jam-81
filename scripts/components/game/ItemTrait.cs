using System;
using System.Diagnostics.CodeAnalysis;
using ShipOfTheseus2025.Enum;
using ShipOfTheseus2025.Models;
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
    public ItemTrait(RandomNumberGeneratorService rng, string description, Stat stat, float minValue, float maxValue, StatChangeMode mode = StatChangeMode.Relative, bool reverseColor = false)
    {

        MinValue = minValue;
        MaxValue = maxValue;
        FixedValue = rng.GetFloatRange(minValue, maxValue);
        Description = string.Format(description, FixedValue);

        ApplyToShip = CreateApplyAction(stat, mode);
        ReverseColor = reverseColor;
        RemoveFromShip = CreateRemoveAction(stat, mode);
    }
    public static Action<IStatsManager, float> CreateApplyAction(Stat stat, StatChangeMode mode)
    {
        return (IStatsManager statsManager, float fixedValue) =>
        {
            statsManager.ChangeStat(new StatChange { Stat = stat, Mode = mode, Amount = fixedValue });
        };
    }
    public static Action<IStatsManager, float> CreateRemoveAction(Stat stat, StatChangeMode mode)
    {
        return (IStatsManager statsManager, float fixedValue) =>
        {
            statsManager.ChangeStat(new StatChange { Stat = stat, Mode = mode, Amount = fixedValue * -1 });
        };
    }

    public void Apply(IStatsManager statsManager) => ApplyToShip(statsManager, FixedValue);
    public void Remove(IStatsManager statsManager) => RemoveFromShip(statsManager, FixedValue);
}
