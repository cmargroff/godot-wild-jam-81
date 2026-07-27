using System;
using Godot;
using ShipOfTheseus2025.Components.Game;
using ShipOfTheseus2025.DependencyInjection;

public partial class InventoryItemSlot : TextureRect, IDroppable
{
  private IInventoryManager _inventoryManager;
  private IItemDragManager _dragManager;
  private IHoverPanelManager _hoverPanelManager;
  private TextureRect _icon;
  private Area2D _dropArea;
  private bool _pickingUp;
  public InventoryItem InventoryItem { get; set; }

  [FromServices]
  public void Inject(
    IInventoryManager inventoryManager,
    IItemDragManager dragManager,
    IHoverPanelManager hoverPanelManager
  )
  {
    _inventoryManager = inventoryManager;
    _hoverPanelManager = hoverPanelManager;
    _dragManager = dragManager;
  }

  public override void _EnterTree()
  {
    _icon = GetNode<TextureRect>("%Icon");
    _dropArea = GetNode<Area2D>("%DropArea");
    _dragManager.Register(this);
  }

  public void OnDragOver(IDraggable draggable)
  {
    var item = draggable.GetItem();
  }

  public void OnDragOut(IDraggable draggable)
  {
    GD.Print("Drag out");
  }

  public void OnDragDrop(IDraggable draggable)
  {
  }

  public bool CanDrop(IDraggable draggable)
  {
    return true;
  }


  public Vector3 GetDropPosition()
  {
    return GetTree().Root.GetCamera3D().ProjectPosition(_dropArea.GlobalPosition, 24f);
  }

  public Area2D GetDropArea()
  {
    return _dropArea;
  }
  public override void _GuiInput(InputEvent @event)
  {
    if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left)
    {
    }
  }
}
