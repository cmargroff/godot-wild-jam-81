using System.Collections.Generic;
using System.Linq;
using Godot;
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
                    -0.01f, -0.05f,
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue }
                    ),
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue * -1 }
                    ), false
                )
            ] },
            { "Seagull",
                [
                    new(rng, "Speed bonus of {0:N2}", 0.01f, 0.05f,
                        (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue }),
                        (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue * -1 }),
                        false)
                ]
            },
            {"Palm Leaf", [
                new(
                    rng,
                    "Attached speed bonus of {0:N2}",
                    1.5f, 2.5f,
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue }
                    ),
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue * -1 }
                    ), false
                ),
                new(
                    rng,
                    "Attached bailing bonus of {0:N2}",
                    0.3f, 0.5f,
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.WaterLevel, Mode = StatChangeMode.Relative, Amount = fixedValue }
                    ),
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.WaterLevel, Mode = StatChangeMode.Relative, Amount = fixedValue * -1 }
                    ), true
                )
            ] },
            {"Coconut", [
                new(
                    rng,
                    "Attached speed bonus of {0:N2}",
                    -0.03f, -0.07f,
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue }
                    ),
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue * -1 }
                    ), false
                )
            ] },
            {"Shark", [
                new(
                    rng,
                    "Attached speed bonus of {0:N2}",
                    0.4f, 0.8f,
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue }
                    ),
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue * -1 }
                    ), false
                ),
                new(
                    rng,
                    "Attached bailing bonus of {0:N2}",
                    -2f, -1.5f,
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.WaterLevel, Mode = StatChangeMode.Relative, Amount = fixedValue }
                    ),
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.WaterLevel, Mode = StatChangeMode.Relative, Amount = 0 }
                    ), true
                )
            ] },
            {"Orange", [
                new(
                    rng,
                    "Attached speed bonus of {0:N2}",
                    -0.03f, -0.07f,
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue }
                    ),
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue * -1 }
                    ), false
                ),
                new(
                    rng,
                    "Attached bailing bonus of {0:N2}",
                    -0.3f, -0.6f,
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.WaterLevel, Mode = StatChangeMode.Relative, Amount = fixedValue }
                    ),
                    (StatsManager statsManager, float fixedValue) => statsManager.ChangeStat(
                        new(){ Stat = Stat.WaterLevel, Mode = StatChangeMode.Relative, Amount = 0 }
                    ), true
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
        //item.Traits = ItemTraitLookup[item.Name].Select(conf => ItemTrait.FromConfig(rng, conf)).ToList();
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
