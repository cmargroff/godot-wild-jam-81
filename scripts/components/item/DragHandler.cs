using Godot;
using ShipOfTheseus2025.DependencyInjection;
using ShipOfTheseus2025.Enum;

public partial class DragHandler : Node, IDragHandler, IDraggable
{
  private Item _item;
  private IItemDragManager _dragManager;
  private IHoverPanelManager _hoverPanelManager;
  [FromServices]
  public void Inject(IItemDragManager dragManager, IHoverPanelManager hoverPanelManager)
  {
    _dragManager = dragManager;
    _hoverPanelManager = hoverPanelManager;
  }

  public override void _EnterTree()
  {
    _item = GetParent<Item>();
    _item.DragHandler = this;
    Enable();
  }

  public void Enable()
  {
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
      if (!_dragManager.CanPickup()) return;
      _item.HoverHandler.Disable();
      _dragManager.StartDragItem(this);
    }
  }
  public Item GetItem()
  {
    return _item;
  }
  public Node3D GetVisualComponent()
  {
    return _item.Visual;
  }
  public CollisionShape2D GetDragShape()
  {
    var shape = new CircleShape2D
    {
      Radius = 20f
    };
    var collisionShape = new CollisionShape2D
    {
      Shape = shape
    };
    return collisionShape;
  }
}
