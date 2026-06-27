using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using ShipOfTheseus2025.Components.Game;
using ShipOfTheseus2025.Enum;
using ShipOfTheseus2025.Resources;
using ShipOfTheseus2025.Services;

public class ItemFactoryService
{
    private Dictionary<string, List<ItemTrait>> ItemTraitLookup;
    private Dictionary<string, List<ItemEffectConfig>> ItemEffectLookup;

    private RandomNumberGeneratorService rng;
    private readonly IStatsManager statsManager;
    private readonly IInventoryManager inventoryManager;
    private readonly IItemDragManager dragManager;
    private ItemEffectContext _itemEffectContext;

    public ItemFactoryService(RandomNumberGeneratorService rng, IStatsManager statsManager, IInventoryManager inventoryManager, IItemDragManager dragManager)
    {
        this.rng = rng;
        this.statsManager = statsManager;
        this.inventoryManager = inventoryManager;
        this.dragManager = dragManager;
        _itemEffectContext = new ItemEffectContext
        {
            InventoryManager = inventoryManager,
            DragManager = dragManager,
            StatsManager = statsManager
        };
        SetupItemTraitLookup();
        SetupItemEffectLookup();
    }

    public void SetupItemTraitLookup()
    {
        ItemTraitLookup = new()
        {
            {"Fancy Portrait", [
                new(
                    rng,
                    "Attached speed bonus of {0:N2}",
                    Stat.Speed,
                    -0.01f, -0.05f
                )
            ] },
            {"Palm Leaf", [
                new(
                    rng,
                    "Attached speed bonus of {0:N2}",
                    Stat.Speed,
                    0.7f, 1.5f
                ),
                new(
                    rng,
                    "Attached bailing bonus of {0:N2}",
                    Stat.Bailing,
                    -0.2f, -0.5f, StatChangeMode.Relative, true
                )
            ] },
            {"Coconut", [
                new(
                    rng,
                    "Attached buoyancy bonus of {0:N2}",
                    Stat.Buoyancy,
                    0.02f, 0.05f
                ),
                new(
                    rng,
                    "Attached speed bonus of {0:N2}",
                    Stat.Speed,
                    -0.2f, -0.5f
                ),
            ] },
            {"Shark", [
                new(
                    rng,
                    "Attached speed bonus of {0:N2}",
                    Stat.Speed,
                    0.2f, 0.5f
                ),
                new(
                    rng,
                    "Attached bailing bonus of {0:N2}",
                    Stat.Bailing,
                    -0.7f, -0.9f, StatChangeMode.Relative, true
                )
            ] },
            {"Orange", [
                new(
                    rng,
                    "Attached speed bonus of {0:N2}",
                    Stat.Speed,
                    -0.1f, -0.05f
                ),
                new(
                    rng,
                    "Attached bailing bonus of {0:N2}",
                    Stat.Bailing,
                    -0.2f, -0.5f
                )
            ] }
        };
    }

    public void SetupItemEffectLookup()
    {
        ItemEffectLookup = new()
        {
            {
                "Barrel", [
                    new ItemEffectConfig{
                        Description = "Test effect that does all the things",
                        ItemStored = (thisItem, context) =>
                        {
                            GD.Print($"Item {thisItem.Name} stored in inventory, there are now {context.InventoryManager.GetInventory().Count} items in the inventory");
                        },
                        ItemRemovedFromStorage = (thisItem, context) =>
                        {
                            GD.Print($"Item {thisItem.Name} removed from inventory");
                        },
                        ItemAttached = (thisItem, context) =>
                        {
                            GD.Print($"Item {thisItem.Name} attached to ship");
                        },
                        ItemDetached = (thisItem, context) =>
                        {
                            GD.Print($"Item {thisItem.Name} detached from ship");
                        },
                        ItemDropped = (thisItem, context) =>
                        {
                            GD.Print($"Item {thisItem.Name} dropped");
                        },
                        ItemPickedUp = (thisItem, context) =>
                        {
                            GD.Print($"Item {thisItem.Name} picked up");
                        },
                        EventStarted = (thisItem, context) =>
                        {
                            GD.Print($"Item {thisItem.Name} saw an event start");
                        },
                        EnemyAttacking = (thisItem, context) =>
                        {
                            GD.Print($"Item {thisItem.Name} saw an enemy attacking");
                        }
                    }
                ]
            },
            {
                "Ruby Amulet", [
                    new ItemEffectConfig{
                        Description = "Increases the value of all items in your inventory by 50%",
                        ItemStored = (thisItem, context) =>
                        {
                            foreach (var item in context.InventoryManager.GetInventory())
                            {
                                if (item != thisItem)
                                item.GoldValueMultiplier += 1.5f;
                            }
                        },
                        ItemRemovedFromStorage = (thisItem, context) =>
                        {
                            foreach (var item in context.InventoryManager.GetInventory())
                            {
                                if (item != thisItem)
                                item.GoldValueMultiplier -= 1.5f;
                            }
                        }
                    }
                ]
            }
        };
    }

    public Action BindItemEffectContext(InventoryItem item, Action<InventoryItem, ItemEffectContext> cb)
    {
        return () => cb(item, _itemEffectContext);
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
        AddItemEffects(item);
        return item;
    }

    private void AddItemTraits(InventoryItem item)
    {
        ItemTraitLookup.TryGetValue(item.Name, out var value);
        if (value is not null)
            item.Traits = ItemTraitLookup[item.Name];
    }

    private void AddItemEffects(InventoryItem item)
    {
        ItemEffectLookup.TryGetValue(item.Name, out var value);
        if (value is not null)
        {
            item.Effects = value.Select(config => new ItemEffect
            {
                Description = config.Description,
                ItemStored = config.ItemStored is not null ? BindItemEffectContext(item, config.ItemStored) : null,
                ItemRemovedFromStorage = config.ItemRemovedFromStorage is not null ? BindItemEffectContext(item, config.ItemRemovedFromStorage) : null,
                ItemAttached = config.ItemAttached is not null ? BindItemEffectContext(item, config.ItemAttached) : null,
                ItemDetached = config.ItemDetached is not null ? BindItemEffectContext(item, config.ItemDetached) : null,
                ItemDropped = config.ItemDropped is not null ? BindItemEffectContext(item, config.ItemDropped) : null,
                ItemPickedUp = config.ItemPickedUp is not null ? BindItemEffectContext(item, config.ItemPickedUp) : null,
                EventStarted = config.EventStarted is not null ? BindItemEffectContext(item, config.EventStarted) : null,
                EnemyAttacking = config.EnemyAttacking is not null ? BindItemEffectContext(item, config.EnemyAttacking) : null
            }).ToList();
        }
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
