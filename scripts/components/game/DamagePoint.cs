using System;
using Godot;
using ShipOfTheseus2025.DependencyInjection;
using ShipOfTheseus2025.Enum;

public partial class DamagePoint : Area3D, IDroppable
{
  private IItemDragManager _dragManager;
  private IStatsManager _statsManager;
  public DamagePointState State;
  private MeshInstance3D _damage;
  private Item _item;
  public event Action LeakingChanged;
  private Area2D _dropArea;
  private Camera3D _camera;
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
  public void Inject(IStatsManager statsManager, IItemDragManager dragManager)
  {
    _statsManager = statsManager;
    _dragManager = dragManager;
  }

  public override void _EnterTree()
  {
    _camera = GetViewport().GetCamera3D();
    State = DamagePointState.SnapDisable;
    _damage = GetNode<MeshInstance3D>("damage");

    _dropArea = new Area2D
    {
      CollisionLayer = 2,
      CollisionMask = 1
    };
    var dropShape = new CollisionShape2D
    {
      Shape = new CircleShape2D
      {
        Radius = 20f
      }
    };
    _dropArea.AddChild(dropShape);
    AddChild(_dropArea);

    _dragManager.Register(this);
  }

  public override void _Process(double delta)
  {
    _dropArea.GlobalPosition = _camera.UnprojectPosition(GlobalPosition);
  }
  public void AttachItem(Item item)
  {
    _item = item;
    item.Reparent(this);
    item.GlobalPosition = GlobalPosition;
    State = DamagePointState.SnapDisable;
    _statsManager.ChangeStat(new()
    {
      Stat = Stat.Buoyancy,
      Amount = item.InventoryItem.Weight,
      Mode = StatChangeMode.Relative
    });
    foreach (ItemTrait trait in _item.InventoryItem.Traits)
    {
      trait.Apply(_statsManager);
    }
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
      // _item.Drop();
    }
    State = DamagePointState.SnapEnable;
    LeakingChanged?.Invoke();
  }

  public bool CanDrop(IDraggable draggable)
  {
    return State == DamagePointState.SnapEnable;
  }

  public Vector3 GetDropPosition()
  {
    return GlobalPosition;
  }

  public void OnDragOver(IDraggable draggable)
  {

  }

  public void OnDragOut(IDraggable draggable)
  {

  }

  public void OnDragDrop(IDraggable draggable)
  {

  }
  public Area2D GetDropArea()
  {
    return _dropArea;
  }
}
