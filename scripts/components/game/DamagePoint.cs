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
    if (_dragManager.Dragging)
    {
      GD.Print(Name + " Entered");
    }
  }
  public override void _MouseExit()
  {
    if (_dragManager.Dragging)
    {
      GD.Print(Name + " Exited");
    }
  }
}
