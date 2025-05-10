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

        InventoryUpdated?.Invoke(_items);
        
    }

    public void AddInventoryItem(Item item)
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

    public Item GetInventoryItem(int index)
    {
        if (index < 0 || index >= _items.Count) return null;
        return _items[index];
    }

    public void SetInventory(List<Item> items)
    {
        _items = items;
        InventoryUpdated?.Invoke(_items);

    }

    public List<Item> GetInventory(){
        return _items;
    }


}

public class Item
{
    public string Name;
    public int ID;
}