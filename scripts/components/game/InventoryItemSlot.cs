using Godot;
using Microsoft.Extensions.DependencyInjection;
using ShipOfTheseus2025;
using ShipOfTheseus2025.Components.Game;
using ShipOfTheseus2025.Interfaces;
using ShipOfTheseus2025.Managers;

public partial class InventoryItemSlot : TextureRect, ISnapPoint
{
  private ItemDragManager _dragManager;
  private TextureRect _icon;
  private ItemPickUp _item;
  public override void _EnterTree()
  {
    _dragManager = Globals.ServiceProvider.GetRequiredService<ItemDragManager>();
    _icon = GetNode<TextureRect>("%Icon");
    Connect(SignalName.MouseEntered, Callable.From(_MouseEntered));
    Connect(SignalName.MouseExited, Callable.From(_MouseExited));
  }

  public void Snap(InventoryItem item)
  {
    _icon.Texture = item.IconTexture;
  }

  private void _MouseEntered()
  {
    _dragManager.SnapPoint(this, false);
  }
  private void _MouseExited()
  {
    _dragManager.Unsnap();
  }

  public void AttachItem(ItemPickUp item)
  {
    _item = item;
    _item.Visible = false;
    _icon.Texture = item.InventoryItem.IconTexture;
    item.Attach();
    _dragManager.Unsnap();
  }

}
