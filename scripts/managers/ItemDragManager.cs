using System.Collections.Generic;
using System.Linq;
using Godot;
using ShipOfTheseus2025.DependencyInjection;

namespace ShipOfTheseus2025.Managers;

public partial class ItemDragManager : Node3D, IItemDragManager
{
  const float ITEM_GRABBED_SCALE = 0.8f;
  const float ITEM_SCALE_SMOOTHING = 0.5f;
  const float ITEM_SNAP_SMOOTHING = 0.3f;
  const int DRAG_LAYER = 1;
  const int DROP_LAYER = 2;
  public bool Dragging { get; private set; }
  private Viewport _viewport;
  private Camera3D _camera;
  private Node3D _draggedNode;
  private IDraggable _draggedItem;
  private Area2D _dragArea;
  private IDroppable _currentDroppable
  {
    get
    {
      if (_droppables.Count == 0) return null;
      return _droppables.Peek();
    }
  }
  private Stack<IDroppable> _droppables = new Stack<IDroppable>();

  public AudioStreamPlayer PickupAudioStreamPlayer { get; set; }
  private IHoverPanelManager _hoverPanelManager;

  [FromServices]
  public void Inject(IHoverPanelManager hoverPanelManager)
  {
    _hoverPanelManager = hoverPanelManager;
  }

  public override void _EnterTree()
  {
    _viewport = GetViewport();

    Name = "ItemDragManager";
    GD.Print("ItemDragManager entered");
    _camera = GetTree().Root.GetCamera3D();
    _viewport.GetWindow().WindowInput += Window_WindowInput;
  }

  public void SetCamera(Camera3D camera)
  {
    _camera = camera;
  }
  private void CreateDragArea(IDraggable draggable)
  {
    var area = new Area2D
    {
      Name = "DragArea",
      CollisionLayer = DRAG_LAYER | DROP_LAYER,
      CollisionMask = DROP_LAYER,
      Monitoring = false,
      Monitorable = true
    };
    var collisionShape = draggable.GetDragShape();
    area.AddChild(collisionShape);
    AddChild(area);
    _dragArea = area;
  }

  public bool CanPickup() => !Dragging;

  public void StartDragItem(IDraggable draggable)
  {
    if (_camera == null) return;

    CreateDragArea(draggable);

    // mode ownership of dragged item to root to simplify dragging
    var node = draggable.GetItem();
    node.Reparent(GetTree().Root, true);
    _draggedNode = node;
    _draggedItem = draggable;

    Dragging = true;
    _hoverPanelManager.SetEnabled(false);
  }
  public void EndDragItem()
  {
    _draggedNode = null;
    _dragArea.QueueFree();
    Dragging = false;
    _hoverPanelManager.SetEnabled(true);
  }
  public override void _Process(double delta)
  {
    if (Dragging)
    {
      var mousePos = _viewport.GetMousePosition();
      _dragArea.GlobalPosition = mousePos;
      _draggedNode.GlobalPosition = _camera.ProjectPosition(mousePos, 24f);
    }
  }
  public void Register(IDroppable droppable)
  {
    BindDropAreaEvents(droppable);
  }
  private void BindDropAreaEvents(IDroppable droppable)
  {
    var area = droppable.GetDropArea();
    area.AreaEntered += (body) => HandleBodyEntered(droppable);
    area.AreaExited += (body) => HandleBodyExited(droppable);
  }
  private void HandleBodyEntered(IDroppable droppable)
  {
    if (droppable.CanDrop(_draggedItem))
    {
      _droppables.Push(droppable);
      _currentDroppable.OnDragOver(_draggedItem);
    }
  }
  private void HandleBodyExited(IDroppable droppable)
  {
    if (_droppables.Count > 0)
    {
      droppable.OnDragOut(_draggedItem);
      _droppables = new Stack<IDroppable>(_droppables.Where(item => item != droppable).ToList());
      _currentDroppable?.OnDragOver(_draggedItem);
    }
  }
  private void Window_WindowInput(InputEvent @event)
  {
    if (Dragging && @event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
    {
      if (mouseEvent.ButtonIndex == MouseButton.Left && _currentDroppable != null)
      {
        _currentDroppable.OnDragDrop(_draggedItem);
        _hoverPanelManager.SetEnabled(true);
        _hoverPanelManager.HidePage();
        EndDragItem();
      }
      if (mouseEvent.ButtonIndex == MouseButton.Right)
      {
        // TODO: drop item in water
        EndDragItem();
      }
    }
  }
}