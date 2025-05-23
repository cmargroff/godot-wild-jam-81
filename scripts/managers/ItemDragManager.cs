using System;
using Godot;
using ShipOfTheseus2025.Components.Game;
using ShipOfTheseus2025.Enum;
using ShipOfTheseus2025.Interfaces;


namespace ShipOfTheseus2025.Managers;

public partial class ItemDragManager : Node3D
{
  public bool Dragging { get; private set; }
  const float ITEM_GRABBED_SCALE = 0.8f;
  const float ITEM_SCALE_SMOOTHING = 0.5f;
  const float ITEM_SNAP_SMOOTHING = 0.3f;
  private Viewport _viewport;
  private Camera3D _camera;
  private ItemPickUp _item;
  private float _scale = 1f;
  private bool _snapped;
  private ISnapPoint _snapPoint;

  public event Action<ItemPickUp> ItemSnapped;


  public AudioStreamPlayer3D PickupAudioStreamPlayer { get; set; }

  public override void _EnterTree()
  {
    _viewport = GetViewport();

    Name = "ItemDragManager";
    GD.Print("ItemDragManager entered");
  }

  public void SetCamera(Camera3D camera)
  {
    _camera = camera;
  }
  public void StartDragItem(ItemPickUp item)
  {
    Dragging = true;

    if (_camera == null) return;

    _scale = 1f;
    _item = item;
    item.Reparent(GetTree().Root, true);
    PickupAudioStreamPlayer.GlobalPosition = item.GlobalPosition;
    PickupAudioStreamPlayer.Play();
    ;
  }
  public void EndDragItem()
  {
    Dragging = false;
    _item = null;
  }
  public override void _Process(double delta)
  {

    if (Dragging)
    {
      var scaleDelta = _scale - (_snapped ? 1f : ITEM_GRABBED_SCALE);
      if (scaleDelta > Mathf.Epsilon || scaleDelta < 0)
      {
        var targetScale = _scale - (scaleDelta * ITEM_SCALE_SMOOTHING);
        _item.GetChild<Node3D>(0).Scale = Vector3.One * targetScale;
        _scale = targetScale;
      }

      var dest = _snapped ? ((Area3D)_snapPoint).GlobalPosition : _camera.ProjectPosition(_viewport.GetMousePosition(), 24f);

      var distance = _item.GlobalPosition.DistanceTo(dest);
      if (distance > Mathf.Epsilon)
      {
        var dir = _item.Position.DirectionTo(dest);
        _item.GlobalPosition += dir * (distance * ITEM_SNAP_SMOOTHING);
      }

    }

  }
  public override void _Input(InputEvent @event)
  {
    if (@event.IsPressed() && @event.IsAction("lmb"))
    {
      if (_snapPoint is not null)
      {
        Attach();
      }
    }
  }
  public void SnapPoint(ISnapPoint point, bool snap)
  {
    _snapped = snap;
    _snapPoint = point;

  }

  public void Attach()
  {
    _snapPoint.AttachItem(_item);

  }
  public void Unsnap()
  {
    _snapped = false;
    _snapPoint = null;
  }

  public ItemPickUp GetItem()
  {
    return _item;
  }

}