using Godot;
using Microsoft.Extensions.DependencyInjection;
using ShipOfTheseus2025;
using ShipOfTheseus2025.Components.Game;
using ShipOfTheseus2025.Managers;

public partial class InventoryItemSlot : TextureRect
{
  private ItemDragManager _dragManager;
  private TextureRect _icon;
  public override void _EnterTree()
  {
    _dragManager = Globals.ServiceProvider.GetRequiredService<ItemDragManager>();
    _icon = GetNode<TextureRect>("%Icon");
    Connect(SignalName.MouseEntered, Callable.From(_MouseEntered));
    Connect(SignalName.MouseEntered, Callable.From(_MouseExited));
  }

  public void Snap(InventoryItem item)
  {
    _icon.Texture = item.IconTexture;
  }

  private void _MouseEntered()
  {
  }
  private void _MouseExited()
  {
    _dragManager.Unsnap();
  }
}
