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
    private System.Collections.Generic.Dictionary<string, List<ItemTraitConfig>> ItemTraitLookup;

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
                new(){
                    Label = "Attached speed bonus of [%placeholder%]",
                    Min = -0.03f,
                    Max = -0.1f,
                    AttachCallback = IncreaseSpeedCallback
                }
            ] },
            { "Seagull", [
                new(){
                    Label = "Speed bonus of [%placeholder%]",
                    Min = 0.05f,
                    Max = 0.2f,
                    AttachCallback = IncreaseSpeedCallback
                }
            ] }
        };
    }

    private void IncreaseSpeedCallback(StatsManager statsManager, float fixedValue) => statsManager
        .ChangeStat(new() { Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = fixedValue });
    private void ReduceSpeedCallback(StatsManager statsManager, float fixedValue) => statsManager
        .ChangeStat(new() { Stat = Stat.Speed, Mode = StatChangeMode.Relative, Amount = -fixedValue });

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
        item.Traits = ItemTraitLookup[item.Name].Select(conf => ItemTrait.FromConfig(rng, conf)).ToList();
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
