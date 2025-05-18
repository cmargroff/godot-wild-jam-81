using Godot;
using Godot.Collections;
using ShipOfTheseus2025.Components.Game;
using System;
using System.Collections.Generic;

namespace ShipOfTheseus2025.Managers;
public partial class InventoryManager 
{
    
    public event Action<List<InventoryItem>> InventoryUpdated;

    [Export]
    private int _inventorySize = 6;
    [Export]
    Texture2D blankIcon;

    private List<InventoryItem> _items;

    public InventoryManager()
    {
        _items = new();

        InventoryUpdated?.Invoke(_items);
        
    }

    public void AddInventoryItem(InventoryItem item)
    {
        if (_items.Count < _inventorySize)
        {
            _items.Add(item);
            InventoryUpdated?.Invoke(_items);
        }
       
    }

    public void RemoveInventoryItem(int index)
    {
        if (index < 0 || index >= _items.Count) return;

        _items.RemoveAt(index);
        InventoryUpdated?.Invoke(_items);
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

    public List<InventoryItem> GetInventory(){
        return _items;
    }


}