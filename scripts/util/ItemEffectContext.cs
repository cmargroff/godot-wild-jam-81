public class ItemEffectContext
{
  public IInventoryManager InventoryManager { get; set; }
  public IItemDragManager DragManager { get; set; }
  public IStatsManager StatsManager { get; set; }
}