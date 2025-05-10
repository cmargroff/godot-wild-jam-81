using Godot;
using System;

namespace JamTemplate.Managers;
public partial class InventoryManager : ItemList
{
    

    [Export]
    private int _inventorySize = 6;
    [Export]
    Texture2D blankIcon;

    private Item[] _items;

    public override void _Ready()
    {
        ItemClicked += OnInventoryItemClicked;
        
        
        _items = new Item[_inventorySize];

        
        Item newItem = new Item();
        newItem.Name = "some item";
        newItem.Icon = GD.Load<Texture2D>("res://test.png");
        AddInventoryItem(newItem);
        LoadInventory();
    }

    public void AddInventoryItem(Item item)
    {
        for (int i = 0; i < _inventorySize; i++)
        {
            if (_items[i] != null) continue;
            
            _items[i] = item;
            
            // SetItemIcon(i, item.Icon);
            
            break;
        }
        GD.Print("item added");
        UpdateUI();
    }

    public void RemoveInventoryItem(int index)
    {
        if (index < 0 || index >= _inventorySize) return;

        _items[index] = null;
        // SetItemIcon(index, blankIcon);
        // SetItemText(index, " ");
        UpdateUI();
    }

    public Item GetInventoryItem(int index)
    {
        if (index < 0 || index >= _inventorySize) return null;
        return _items[index];
    }

    private void OnInventoryItemClicked(long index, Vector2 pos, long mouseButtonIndex)
    {
        
        if (mouseButtonIndex == 2)
        {
            Item item = GetInventoryItem((int)index);
            if (item == null) 
            {
                GD.Print("no item");
                return;
            }
            RemoveInventoryItem((int)index);
        }
        else if (mouseButtonIndex == 1)
        {
            Item item = GetInventoryItem((int)index);
            if (item == null)
            {
                GD.Print("no item");
                return;
            }
            GD.Print(item.Name);
            //use item
        }
    }

    public void LoadInventory()
    {
        for (int i = 0; i < _inventorySize; i++)
        {
            if (_items[i] == null)
            {
                
                AddItem(" ", blankIcon);
            }
            else
            {
                AddItem(" ", _items[i].Icon);
            }
           
        }
    }

    public void UpdateUI()
    {
        for (int i = 0; i < _inventorySize; i++)
        {
            if (_items[i] != null)
            {
                SetItemIcon(i, _items[i].Icon);
            }
            else 
            {
                SetItemIcon(i, blankIcon);
            }
        }
    }

}

public class Item
{
    public string Name;
    public Texture2D Icon; //?
}