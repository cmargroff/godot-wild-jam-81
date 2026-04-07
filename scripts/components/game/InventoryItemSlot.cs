using Godot;
using Microsoft.Extensions.DependencyInjection;
using ShipOfTheseus2025;
using ShipOfTheseus2025.Components.Game;
using ShipOfTheseus2025.DependencyInjection;
using ShipOfTheseus2025.Enum;
using ShipOfTheseus2025.Interfaces;
using ShipOfTheseus2025.Managers;

public partial class InventoryItemSlot : TextureRect, ISnapPoint
{
  private InventoryManager _inventoryManager;
  private IItemDragManager _dragManager;
  private IHoverPanelManager _hoverManager;
  private TextureRect _icon;
  private ItemPickUp _item;
  public InventoryItem InventoryItem { get; set; }

  [FromServices]
  public void Inject()
  {
    GD.Print("Injecting InventoryItemSlot");
  }

  public override void _EnterTree()
  {
    _dragManager = Globals.Instance.ServiceProvider.GetRequiredService<IItemDragManager>();
    _hoverManager = Globals.Instance.ServiceProvider.GetRequiredService<IHoverPanelManager>();
    _inventoryManager = Globals.Instance.ServiceProvider.GetRequiredService<InventoryManager>();
    _icon = GetNode<TextureRect>("%Icon");
    Connect(SignalName.MouseEntered, Callable.From(_MouseEntered));
    Connect(SignalName.MouseExited, Callable.From(_MouseExited));
  }

  public void Snap()
  {
    var item = _dragManager.GetItem();
    if (_item is null && item is not null)
    {
      _icon.Texture = item.InventoryItem.IconTexture;
      _icon.Modulate = new Color(1, 1, 1, 0.5f);
      GD.Print("show icon");
    }
  }

  public void Unsnap()
  {
    _icon.Texture = null;
  }

  private void _MouseEntered()
  {
    _dragManager.SnapPoint(this, false);
    Snap();
    if (_dragManager.Dragging == false && _item is not null) _hoverManager.ShowItem(_item.InventoryItem, HoverType.Slot);

  }
  private void _MouseExited()
  {
    _dragManager.Unsnap();
    if (_item is null) Unsnap();
    if (_dragManager.Dragging == false) _hoverManager.HidePage();
  }

  public void AttachItem(ItemPickUp item)
  {
    if (_dragManager.Dragging == false && _item is not null)
    {
      GrabItem();
    }
    else if (_dragManager.Dragging && _item is null)
    {
      _item = item;
      item.Visible = false;
      _icon.Texture = item.InventoryItem.IconTexture;
      _icon.Modulate = new Color(1, 1, 1, 1);
      item.Attach();
      _inventoryManager.AddInventoryItem(item.InventoryItem);
      _dragManager.EndDragItem();
      _dragManager.Unsnap();
    }
    _MouseEntered();
  }

  public void GrabItem()
  {
    _icon.Texture = null;
    _item.Visible = true;
    _item.Grab();
    _inventoryManager.RemoveInventoryItem(_item.InventoryItem);
    _dragManager.StartDragItem(_item);
    _item = null;

  }


}
