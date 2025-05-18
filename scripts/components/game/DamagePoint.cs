using System;
using Godot;
using Microsoft.Extensions.DependencyInjection;
using ShipOfTheseus2025;
using ShipOfTheseus2025.Components.Game;
using ShipOfTheseus2025.Enum;
using ShipOfTheseus2025.Interfaces;
using ShipOfTheseus2025.Managers;
using ShipOfTheseus2025.Util;


public partial class DamagePoint : Area3D, ISnapPoint
{
  private ItemDragManager _dragManager;
  private StatsManager _statsManager;
  public DamagePointState State;
  private MeshInstance3D _damage;
  private ItemPickUp _item;
  public event Action LeakingChanged;
  public bool Leaking
  {
    get
    {
      return State == DamagePointState.SnapEnable;
    }
  }
  public enum DamagePointState
  {
    SnapEnable,
    SnapDisable
  }

  [FromServices]
  public void Inject(StatsManager statsManager)
  {
    _statsManager = statsManager;
  }

  public override void _EnterTree()
  {
    State = DamagePointState.SnapDisable;
    _dragManager = Globals.ServiceProvider.GetRequiredService<ItemDragManager>();
    _damage = GetNode<MeshInstance3D>("damage");


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
    _item = item;
    item.Reparent(this);
    item.Attach();
    item.GlobalPosition = GlobalPosition;
    State = DamagePointState.SnapDisable;
    _statsManager.ChangeStat(new()
    {
      Stat = Stat.Buoyancy,
      Amount = item.InventoryItem.Weight,
      Mode = StatChangeMode.Relative
    });
    
    _dragManager.Unsnap();
    _dragManager.EndDragItem();
    LeakingChanged?.Invoke();
  }

  public void Enable()
  {
    if (_item is null)
    {
      _damage.Visible = true;
    }
    else

    {
      _item.Reparent(GetTree().Root);
      foreach (ItemTrait trait in _item.InventoryItem.Traits)
      {
        trait.Remove(_statsManager);
      }
      _item.Drop();
    }
    State = DamagePointState.SnapEnable;
    LeakingChanged?.Invoke();
  }
}
