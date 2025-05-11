using Godot;

namespace ShipOfTheseus2025.Managers;

public partial class ItemDragManager : Node3D
{
  private Viewport _viewport;
  private Camera3D _camera;
  private bool _dragging;
  private Node3D _item;
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

    node.Reparent(this);
    _item = node;
  }
  public void EndDragItem()
  {
    _dragging = false;
  }
  public override void _PhysicsProcess(double delta)
  {
    if (_dragging)
    {
      var pos = _camera.ProjectPosition(_viewport.GetMousePosition(), 6f);
      GD.Print(pos);
      _item.GlobalPosition = pos;
    }
  }
}