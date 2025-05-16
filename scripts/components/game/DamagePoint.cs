using Godot;
using Microsoft.Extensions.DependencyInjection;
using ShipOfTheseus2025;
using ShipOfTheseus2025.Managers;
using ShipOfTheseus2025.Interfaces;
using ShipOfTheseus2025.Components.Game;


public partial class DamagePoint : Area3D, ISnapPoint
{
  private ItemDragManager _dragManager;
  public DamagePointState State;

  public enum DamagePointState 
  {
    SnapEnable,
    SnapDisable
  }

  public override void _EnterTree()
  {
    State = DamagePointState.SnapEnable;
    _dragManager = Globals.ServiceProvider.GetRequiredService<ItemDragManager>();
  }
  public override void _MouseEnter()
  {
    if (_dragManager.Dragging && State == DamagePointState.SnapEnable)
    {
      _dragManager.SnapPoint(this, true);

    }
  }
  public override void _MouseExit()
  {
    if (_dragManager.Dragging && State == DamagePointState.SnapEnable)
    {
      _dragManager.Unsnap();
    }
  }
  public void AttachItem(ItemPickUp item)
  {
    item.Reparent(this);
    item.Attach();
    item.GlobalPosition = GlobalPosition;
    State = DamagePointState.SnapDisable;
    _dragManager.Unsnap();
    _dragManager.EndDragItem();
  }
  // public void Snap(ItemPickUp item)
  // {
  //   GD.Print("");
  // }
}
