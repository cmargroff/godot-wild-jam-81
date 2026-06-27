using System;
using System.Collections.Generic;
using ShipOfTheseus2025.Components.Game;

public interface IInventoryManager
{
  public event Action<List<InventoryItem>> InventoryUpdated;
  public void AddInventoryItem(InventoryItem item);
  public void RemoveInventoryItem(int index);
  public void RemoveInventoryItem(InventoryItem item);
  public InventoryItem GetInventoryItem(int index);
  public void SetInventory(List<InventoryItem> items);
  public List<InventoryItem> GetInventory();
}