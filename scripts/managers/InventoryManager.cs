using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

namespace JamTemplate.Managers;
public partial class InventoryManager 
{
    
    public event Action<List<Item>> InventoryUpdated;

    [Export]
    private int _inventorySize = 6;
    [Export]
    Texture2D blankIcon;

    private List<Item> _items;

    public InventoryManager()
    {
        _items = new();

    }

    public void AddInventoryItem(Item item)
    {
        for (int i = 0; i < _inventorySize; i++)
        {
            if (_items[i] != null) continue;
            
            _items[i] = item;
            
            InventoryUpdated?.Invoke(_items);
            
            return;
        }
       
    }

    public void RemoveInventoryItem(int index)
    {
        if (index < 0 || index >= _inventorySize) return;

        _items[index] = null;
        
        InventoryUpdated?.Invoke(_items);
    }

    public Item GetInventoryItem(int index)
    {
        if (index < 0 || index >= _inventorySize) return null;
        return _items[index];
    }

    public void GetStartingInventory(List<Item> items)
    {
        _items = items;
    }

}

public class Item
{
    public string Name;
    public int ID;
}