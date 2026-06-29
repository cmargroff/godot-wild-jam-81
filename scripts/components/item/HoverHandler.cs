using Godot;
using ShipOfTheseus2025.DependencyInjection;
using ShipOfTheseus2025.Enum;

public partial class HoverHandler : Node, IHoverHandler
{
  private Item _item;
  private IHoverPanelManager _hoverManager;
  [FromServices]
  public void Inject(IHoverPanelManager hoverManager)
  {
    _hoverManager = hoverManager;
  }
  public override void _EnterTree()
  {
    _item = GetParent<Item>();
    _item.HoverHandler = this;
    Enable();
  }
  public void Enable()
  {
    _item.Area.MouseEntered += OnMouseEntered;
    _item.Area.MouseExited += OnMouseExited;
  }

  public void Disable()
  {
    _item.Area.MouseEntered -= OnMouseEntered;
    _item.Area.MouseExited -= OnMouseExited;
  }
  public override void _ExitTree()
  {
    Disable();
  }
  private void OnMouseEntered()
  {
    _hoverManager.ShowItem(_item.InventoryItem, HoverType.Item);
  }
  private void OnMouseExited()
  {
    _hoverManager.HidePage();
  }
}
