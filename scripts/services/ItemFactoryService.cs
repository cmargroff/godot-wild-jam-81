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
                    "Attached speed bonus of [%placeholder%]",
                    -0.01f, -0.05f,
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue }
                    )
                )
            ] },
            // { "Seagull",
            // [
            //         new(rng, "Speed bonus of [%placeholder%]", 0.01f, 0.05f, (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue }))
            //     ]
            // }
            {"Palm Leaf", [
                new(
                    rng,
                    "Attached speed bonus of [%placeholder%]",
                    0.7f, 1.5f,
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue }
                    )
                )
            ] },
            {"Coconut", [
                new(
                    rng,
                    "Attached buoyancy bonus of [%placeholder%]",
                    0.02f, 0.05f,
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Buoyancy, Mode = StatChangeMode.Relative, Amount = fixedValue }
                    )
                ),
                new(
                    rng,
                    "Attached speed bonus of [%placeholder%]",
                    -0.2f, -0.5f,
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue }
                    )
                ),
            ] },
            {"Shark", [
                new(
                    rng,
                    "Attached speed bonus of [%placeholder%]",
                    0.02f, 0.05f,
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue }
                    )
                ),
                new(
                    rng,
                    "Attached bailing bonus of [%placeholder%]",
                    -0.2f, -0.5f,
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.WaterLevel, Mode = StatChangeMode.Relative, Amount = fixedValue }
                    )
                ),
            ] },
            {"Orange", [
                new(
                    rng,
                    "Attached speed bonus of [%placeholder%]",
                    -0.01f, -0.05f,
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue }
                    )
                )          
            ] }
        };
    }

    public InventoryItem GenerateItem(ItemResource itemResource)
    {
        InventoryItem item = new()
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

    private void AddItemTraits(InventoryItem item)
    {
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
