using System;
using System.Collections.Generic;
using Godot;
using ShipOfTheseus2025.Components.Game;

namespace ShipOfTheseus2025.Managers;

public partial class InventoryManager : IInventoryManager
{

    public event Action<List<InventoryItem>> InventoryUpdated;

    [Export]
    private int _inventorySize = 6;
    [Export]
    // Texture2D blankIcon;
    private IStatsManager _statsManager;

    private List<InventoryItem> _items;

    public InventoryManager(IStatsManager statsManager)
    {
        _items = new();
        _statsManager = statsManager;
        InventoryUpdated?.Invoke(_items);
    }

    public void AddInventoryItem(InventoryItem item)
    {
        if (_items.Count < _inventorySize)
        {
            _items.Add(item);
            _statsManager.ChangeStat(new()
            {
                Stat = Enum.Stat.Buoyancy,
                Mode = Enum.StatChangeMode.Relative,
                Amount = item.Weight
            });
            foreach (var effect in item.Effects)
            {
                if (effect.ItemStored is not null)
                    effect.ItemStored();
            }
            InventoryUpdated?.Invoke(_items);
        }

    }

    public void RemoveInventoryItem(int index)
    {
        if (index < 0 || index >= _items.Count) return;

        InventoryItem item = _items[index];
        _items.RemoveAt(index);
        _statsManager.ChangeStat(new()
        {
            Stat = Enum.Stat.Buoyancy,
            Mode = Enum.StatChangeMode.Relative,
            Amount = item.Weight * -1
        });
        InventoryUpdated?.Invoke(_items);
    }

    public void RemoveInventoryItem(InventoryItem item)
    {
        if (_items.Remove(item))
        {
            _statsManager.ChangeStat(new()
            {
                Stat = Enum.Stat.Buoyancy,
                Mode = Enum.StatChangeMode.Relative,
                Amount = item.Weight * -1
            });
            InventoryUpdated?.Invoke(_items);
        }
    }

    public InventoryItem GetInventoryItem(int index)
    {
        if (index < 0 || index >= _items.Count) return null;
        return _items[index];
    }

    public void SetInventory(List<InventoryItem> items)
    {
        _items = items;
        InventoryUpdated?.Invoke(_items);

    }

    public List<InventoryItem> GetInventory()
    {
        return _items;
    }
}