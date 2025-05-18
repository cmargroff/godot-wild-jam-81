using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using ShipOfTheseus2025.Components.Game;
using ShipOfTheseus2025.Enum;
using ShipOfTheseus2025.Managers;
using ShipOfTheseus2025.Resources;
using ShipOfTheseus2025.Services;

public class ItemFactoryService
{
    private System.Collections.Generic.Dictionary<string, List<ItemTrait>> ItemTraitLookup;

    private RandomNumberGeneratorService rng;
    private readonly StatsManager statsManager;

    public ItemFactoryService(RandomNumberGeneratorService rng, StatsManager statsManager)
    {
        this.rng = rng;
        this.statsManager = statsManager;
        SetupItemTraitLookup();
    }

    public void SetupItemTraitLookup()
    {
        ItemTraitLookup = new()
        {
            {"Fancy Portrait", [
                new(
                    rng,
                    "Attached speed bonus of {0:N2}",
                    -0.01f, -0.05f, false,
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue }
                    ),
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue * -1}
                    )
                )
            ] },
            {"Palm Leaf", [
                new(
                    rng,
                    "Attached speed bonus of {0:N2}",
                    0.7f, 1.5f, false,
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue }
                    ),
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue * -1 }
                    )
                ),
                new(
                    rng,
                    "Attached bailing bonus of {0:N2}",
                    -0.2f, -0.5f, true,
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.WaterLevel, Mode = StatChangeMode.Relative, Amount = fixedValue }
                    ),
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.WaterLevel, Mode = StatChangeMode.Relative, Amount = fixedValue * -1 }
                    )
                )
            ] },
            {"Coconut", [
                new(
                    rng,
                    "Attached buoyancy bonus of {0:N2}",
                    0.02f, 0.05f, false,
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Buoyancy, Mode = StatChangeMode.Relative, Amount = fixedValue }
                    ),
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Buoyancy, Mode = StatChangeMode.Relative, Amount = fixedValue* -1 }
                    )
                ),
                new(
                    rng,
                    "Attached speed bonus of {0:N2}",
                    -0.2f, -0.5f, false,
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue }
                    ),
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue * -1}
                    )
                ),
            ] },
            {"Shark", [
                new(
                    rng,
                    "Attached speed bonus of {0:N2}",
                    0.2f, 0.5f, false,
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue }
                    ),
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue * -1}
                    )
                ),
                new(
                    rng,
                    "Attached bailing bonus of {0:N2}",
                    -0.7f, -0.9f, true,
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.WaterLevel, Mode = StatChangeMode.Relative, Amount = fixedValue }
                    ),
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.WaterLevel, Mode = StatChangeMode.Relative, Amount = 0 }
                    )
                )
            ] },
            {"Orange", [
                new(
                    rng,
                    "Attached speed bonus of {0:N2}",
                    -0.1f, -0.05f, false,
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue }
                    ),
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue * -1 }
                    )
                ),
                new(
                    rng,
                    "Attached bailing bonus of {0:N2}",
                    -0.2f, -0.5f, true,
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.WaterLevel, Mode = StatChangeMode.Relative, Amount = fixedValue }
                    ),
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.WaterLevel, Mode = StatChangeMode.Relative, Amount = 0 }
                    )
                )
            ] }
        };
    }

    public ShipOfTheseus2025.Components.Game.InventoryItem GenerateItem(ItemResource itemResource)
    {
        ShipOfTheseus2025.Components.Game.InventoryItem item = new()
        {
            Name = itemResource.ItemName,
            Description = itemResource.Description,
            GoldValue = GetGoldValue(itemResource),
            Weight = rng.GetFloatRange(itemResource.MinWeight, itemResource.MaxWeight),
            IconTexture = itemResource.IconTexture,
            ItemScene = itemResource.ItemScene?.Instantiate<Node3D>()// ?? new Node3D()
        };
        AddItemTraits(item);
        return item;
    }

    private void AddItemTraits(ShipOfTheseus2025.Components.Game.InventoryItem item)
    {
        ItemTraitLookup.TryGetValue(item.Name, out var value);
        if (value is not null)
            item.Traits = ItemTraitLookup[item.Name];
    }

    private int GetGoldValue(ItemResource itemResource)
    {
        return itemResource.GoldValueDistribution switch
        {
            GoldValueDistribution.Extremes => rng.GetFloat() > 0.5f ? itemResource.MaxGoldValue : itemResource.MinGoldValue,
            GoldValueDistribution.FullRange => (int)rng.GetFloatRange(itemResource.MinGoldValue, itemResource.MaxGoldValue),
            GoldValueDistribution.Normal => (int)rng.NextInNormalDistribution((itemResource.MinGoldValue + itemResource.MaxGoldValue) / 2f, (itemResource.MinGoldValue + itemResource.MaxGoldValue) / 10f),
            _ => itemResource.MinGoldValue
        };
    }

}
