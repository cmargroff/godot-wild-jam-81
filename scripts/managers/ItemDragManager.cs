using System;
using Godot;

namespace ShipOfTheseus2025.Managers;

public partial class ItemDragManager : Node3D
{
  const float ITEM_GRABBED_SCALE = 0.5f;
  const float ITEM_SCALE_SMOOTHING = 0.5f;
  const float ITEM_SNAP_SMOOTHING = 0.2f;
  private Viewport _viewport;
  private Camera3D _camera;
  private bool _dragging;
  private Node3D _item;
  private float _scale = 1f;
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
    _dragging = true;

    if (_camera == null) return;

    _scale = 1f;
    _item = new Node3D();
    _item.GlobalPosition = node.GlobalPosition;
    node.Reparent(_item);
    node.Position = Vector3.Zero;
    GetTree().Root.AddChild(_item);
  }
  public void EndDragItem()
  {
    _dragging = false;
  }
  public override void _Process(double delta)
  {
    if (_dragging)
    {
      var scaleDelta = _scale - ITEM_GRABBED_SCALE;
      if (scaleDelta > Mathf.Epsilon)
      {
        var targetScale = _scale - (scaleDelta * ITEM_SCALE_SMOOTHING);
        _item.GetChild<Node3D>(0).Scale = Vector3.One * targetScale;
        _scale = targetScale;
      }
      var dest = _camera.ProjectPosition(_viewport.GetMousePosition(), 3f);
      var distance = _item.GlobalPosition.DistanceTo(dest);
      if (distance > Mathf.Epsilon)
      {
        var dir = _item.Position.DirectionTo(dest);
        _item.GlobalPosition += dir * (distance * ITEM_SNAP_SMOOTHING);
      }
    }
  }
}