using Godot;
using ShipOfTheseus2025.DependencyInjection;

public partial class DragHandler : Node, IDragHandler
{
  private Item _item;
  private IItemDragManager _dragManager;
  [FromServices]
  public void Inject(IItemDragManager dragManager)
  {
    _dragManager = dragManager;
  }

  public override void _EnterTree()
  {
    _item = GetParent<Item>();
    _item.DragHandler = this;
    Enable();
  }

  public void Enable()
  {
    // _item.Area.Connect(Area3D.SignalName.InputEvent, Callable.From<Node, InputEvent, int>(OnInputEvent));
    _item.Area.InputEvent += OnInputEvent;
  }

  public void Disable()
  {
    _item.Area.InputEvent -= OnInputEvent;
  }

  public override void _ExitTree()
  {
    Disable();
  }
  private void OnInputEvent(Node camera, InputEvent @event, Vector3 eventPosition, Vector3 normal, long shapeIdx)
  {
    if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
    {
      OnMouseDown(mouseEvent);
    }
  }
  public void OnMouseDown(InputEventMouseButton mouseEvent)
  {
    if (mouseEvent.ButtonIndex == MouseButton.Left)
    {
      _dragManager.StartDragItem(_item);
      _item.Area.InputRayPickable = false;
      _item.HoverHandler.Disable();
    }
  }
}
