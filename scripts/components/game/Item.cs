using Godot;
using ShipOfTheseus2025.Components.Game;

public partial class Item : Node3D
{
  public InventoryItem InventoryItem { get; set; }
  public IItemMover ItemMover { get; set; }
  public IDragHandler DragHandler { get; set; }
  public IHoverHandler HoverHandler { get; set; }
  public Area3D Area { get; private set; }
  public Node3D Visual { get; private set; }
  public override void _EnterTree()
  {
    Visual = GetNode<Node3D>("Visual");
    var area = Visual.GetNode<Area3D>("%Area3D");
    if (area == null)
    {
      GD.PrintErr("Area3D node not found in Item");
      QueueFree();
    }
    Area = area;
  }
  public void SetAttached(AttachSlotType attachSlotType) { }
  public void SetDetached() { }
}
