using System;
using ShipOfTheseus2025.Components.Game;

public class ItemEffectConfig
{
  public string Description { get; set; }
  public Action<InventoryItem, ItemEffectContext> ItemStored { get; set; }
  public Action<InventoryItem, ItemEffectContext> ItemRemovedFromStorage { get; set; }
  public Action<InventoryItem, ItemEffectContext> ItemAttached { get; set; }
  public Action<InventoryItem, ItemEffectContext> ItemDetached { get; set; }
  public Action<InventoryItem, ItemEffectContext> ItemDropped { get; set; }
  public Action<InventoryItem, ItemEffectContext> ItemPickedUp { get; set; }
  public Action<InventoryItem, ItemEffectContext> EventStarted { get; set; }
  public Action<InventoryItem, ItemEffectContext> EnemyAttacking { get; set; }
}