using Godot;
using ShipOfTheseus2025.Components.Game;

public partial class Item : Node3D
{
  public InventoryItem InventoryItem { get; set; }
  public IItemMover ItemMover { get; set; }
  public IDragHandler DragHandler { get; set; }
  public IHoverHandler HoverHandler { get; set; }
  public Area3D Area { get; private set; }
  public override void _EnterTree()
  {
    var area = GetNode<Area3D>("Display/%Area3D");
    if (area == null)
    {
      GD.PrintErr("Area3D node not found in Item");
      QueueFree();
    }
    Area = area;
  }
}
