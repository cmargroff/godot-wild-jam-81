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
  private IDraggable _storedItem;

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
    item.InventoryHover(this);
  }

  public void OnDragOut(IDraggable draggable)
  {
    var item = draggable.GetItem();
    item.InventoryHover(this, false);
  }

  public void OnDragDrop(IDraggable draggable)
  {
    var item = draggable.GetItem();
    _storedItem = draggable;
    _inventoryManager.AddInventoryItem(item.InventoryItem);
    _icon.Texture = item.InventoryItem.IconTexture;
    _icon.Modulate = new Color(1, 1, 1, 1);
  }

  public bool CanDrop(IDraggable draggable)
  {
    return _storedItem == null;
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
    if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
    {
      if (_storedItem != null)
      {
        _dragManager.StartDragItem(_storedItem);
        _storedItem.GetItem().Show();
        _storedItem = null;
        _icon.Texture = null;
      }
    }
  }
  public void ShowHoverImage(Texture2D tex)
  {
    _icon.Texture = tex;
    _icon.Modulate = new Color(1,1,1,0.75f);
  }
  public void RemoveHoverImage()
  {
    _icon.Texture = null;
    _icon.Modulate = new Color(1,1,1,1);
  }
}
