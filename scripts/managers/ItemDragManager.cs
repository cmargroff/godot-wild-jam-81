using System;
using Godot;

namespace ShipOfTheseus2025.Managers;

public partial class ItemDragManager : Node3D
{
  public bool Dragging { get; private set; }
  const float ITEM_GRABBED_SCALE = 0.2f;
  const float ITEM_SCALE_SMOOTHING = 0.5f;
  const float ITEM_SNAP_SMOOTHING = 0.2f;
  private Viewport _viewport;
  private Camera3D _camera;
  private Node3D _item;
  private float _scale = 1f;
  private bool _snapped;
  private Node3D _snapPoint;

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

  public void StartDragItem(Node3D node)
  {
    Dragging = true;

    if (_camera == null) return;

    _scale = 1f;
    _item = new Node3D();
    _item.GlobalPosition = node.GlobalPosition;
    node.Reparent(_item);
    node.Position = Vector3.Zero;
    GetTree().Root.AddChild(_item);
    PickupAudioStreamPlayer.GlobalPosition = node.GlobalPosition;
    PickupAudioStreamPlayer.Play();
  }
  public void EndDragItem()
  {
    Dragging = false;
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
      var dest = _snapped ? _snapPoint.GlobalPosition : _camera.ProjectPosition(_viewport.GetMousePosition(), 3f);
      var distance = _item.GlobalPosition.DistanceTo(dest);
      if (distance > Mathf.Epsilon)
      {
        var dir = _item.Position.DirectionTo(dest);
        _item.GlobalPosition += dir * (distance * ITEM_SNAP_SMOOTHING);
      }
    }
  }
  public void SnapPoint(Node3D node)
  {
    _snapped = true;
    _snapPoint = node;
  }
  public void Unsnap()
  {
    _snapped = false;
    _snapPoint = null;
  }
}