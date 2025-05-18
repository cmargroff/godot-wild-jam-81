using Godot;
using ShipOfTheseus2025;
using ShipOfTheseus2025.Managers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using ShipOfTheseus2025.Components.Game;

public partial class InventoryUi : Control
{
    private InventoryManager _inventoryManager;
    private ItemList _itemList;

    private Dictionary<string, Texture2D> _itemIcons;
    private Texture2D _blankIcon;
    
    public override void _EnterTree()
    {
        PreloadIcons();
        _inventoryManager = Globals.ServiceProvider.GetRequiredService<InventoryManager>();
        _itemList = GetNode<ItemList>("ItemList");
        GD.Print(_itemList);
        _inventoryManager.InventoryUpdated += RenderItems;
        _itemList.Connect(ItemList.SignalName.ItemClicked, Callable.From<long, Vector2, long>(ItemList_ItemClicked));
        RenderItems(_inventoryManager.GetInventory());
    }

    private void PreloadIcons()
    {
        _itemIcons = new();
        var tex = ResourceLoader.Load<Texture2D>("res://assets/textures/items/test.png");
        _itemIcons.Add("test", tex);
    }
    private void RenderItems(List<InventoryItem> items)
    {
        
        _itemList.Clear();
        
        foreach (var item in items)
        {
            _itemList.AddItem(item.Name, _itemIcons[item.Name]);
        }
        
        int itemsCount = items.Count;
        if (itemsCount < 6)
        {
            for (int i = itemsCount; i < 6; i++)
            {
                _itemList.AddItem(" ", _blankIcon);
            }
        }
    }

    private void ItemList_ItemClicked(long index, Vector2 pos, long mouseButtonIndex)
    {
        
    
        if (mouseButtonIndex == 2)
        {
            _inventoryManager.RemoveInventoryItem((int)index);
        }
        else if (mouseButtonIndex == 1)
        {
            var item = _inventoryManager.GetInventoryItem((int)index);
            if (item == null)
            {
                GD.Print("no item");
            }
            else{
                //use item
            }
           
        }
    }
    
}
