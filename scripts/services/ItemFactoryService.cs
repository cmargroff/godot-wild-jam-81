using Godot;
using ShipOfTheseus2025.Components.Game;
using ShipOfTheseus2025.Enum;
using ShipOfTheseus2025.Resources;
using ShipOfTheseus2025.Services;
using System;

public class ItemFactoryService
{
    private RandomNumberGeneratorService rng;

    public ItemFactoryService(RandomNumberGeneratorService rng)
    {
        this.rng = rng;
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
            ItemScene = itemResource.ItemScene.Instantiate<Node3D>()
        };
        return item;
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
