using Godot;
using Microsoft.Extensions.DependencyInjection;
using ShipOfTheseus2025;
using ShipOfTheseus2025.Managers;
using ShipOfTheseus2025.Interfaces;
using ShipOfTheseus2025.Components.Game;
using ShipOfTheseus2025.Util;
using ShipOfTheseus2025.Models;
using ShipOfTheseus2025.Enum;


public partial class DamagePoint : Area3D, ISnapPoint
{
  private ItemDragManager _dragManager;
  private StatsManager _statsManager;
  private ScoreManager _scoreManager;
  public DamagePointState State;
  private MeshInstance3D _damage;
  private ItemPickUp _item;
  public InventoryItem InventoryItem { get; set; }

  public enum DamagePointState
  {
    SnapEnable,
    SnapDisable
  }
  [FromServices]
  public void Inject(ItemDragManager dragManager, StatsManager statsManager, ScoreManager scoreManager)
  {
    _dragManager = dragManager;
    _statsManager = statsManager;
    _scoreManager = scoreManager;
  }
  public override void _EnterTree()
  {
    State = DamagePointState.SnapDisable;
    _damage = GetNode<MeshInstance3D>("damage");

  }

  public override void _PhysicsProcess(double delta)
  {
    if (State == DamagePointState.SnapEnable)
    {
    _statsManager.ChangeStat(new StatChange { Stat = Stat.WaterLevel, Mode = StatChangeMode.Relative, Amount = 0.01f });

    }
      
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
    _dragManager.Unsnap();
    _dragManager.EndDragItem();
    // InventoryItem = item.InventoryItem;
    // var gold = InventoryItem.GoldValue;
    // _scoreManager.RemoveGold(gold);
  }

  public void Enable()
  {
    if (_item is null)
    {
      _damage.Visible = true;
    }
    else 
    {
      // _item.Reparent(GetTree().Root, true);
      _item.Drop();
    }
    State = DamagePointState.SnapEnable;
    
  }
}
