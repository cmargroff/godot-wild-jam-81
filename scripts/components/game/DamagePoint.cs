using Godot;
using Microsoft.Extensions.DependencyInjection;
using ShipOfTheseus2025;
using ShipOfTheseus2025.Managers;

public partial class DamagePoint : Area3D
{
  private ItemDragManager _dragManager;
  public override void _EnterTree()
  {
    _dragManager = Globals.ServiceProvider.GetRequiredService<ItemDragManager>();
  }
  public override void _MouseEnter()
  {
    GD.Print("snap point");
    if (_dragManager.Dragging)
    {
      _dragManager.SnapPoint(this);
      GD.Print("snapped");

    }
  }
  public override void _MouseExit()
  {
    if (_dragManager.Dragging)
    {
      _dragManager.Unsnap();
      GD.Print("unsnapped");
    }
  }
}
