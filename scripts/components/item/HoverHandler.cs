using Godot;
using ShipOfTheseus2025.DependencyInjection;
using ShipOfTheseus2025.Enum;

public partial class HoverHandler : Node, IHoverHandler
{
  private Item _item;
  private IHoverPanelManager _hoverManager;
  private Callable _onEnterCallable;
  private Callable _onExitCallable;
  [FromServices]
  public void Inject(IHoverPanelManager hoverManager)
  {
    _hoverManager = hoverManager;
  }
  public override void _EnterTree()
  {
    _item = GetParent<Item>();
    _item.HoverHandler = this;
    _onEnterCallable = Callable.From(OnMouseEntered);
    _onExitCallable = Callable.From(OnMouseExited);
    Enable();
  }
  public void Enable()
  {
    _item.Area.Connect(Area3D.SignalName.MouseEntered,  _onEnterCallable);
    _item.Area.Connect(Area3D.SignalName.MouseExited,  _onExitCallable);
  }
  public void Disable()
  {
    _item.Area.Disconnect(Area3D.SignalName.MouseEntered,  _onEnterCallable);
    _item.Area.Disconnect(Area3D.SignalName.MouseExited,  _onExitCallable);
  }
  private void OnMouseEntered()
  {
    _hoverManager.ShowItem(_item.InventoryItem, HoverType.Item);
  }
  private void OnMouseExited()
  {
    // this can potentially be called because another item is hovered instead, even if it never activated its hover
    // potential fix would be to latch on this object so it can only be exited if it was hovered
    // another potential fix would be to just disable hide page from doing anything in the hover panel manager until the current item is dropped
    _hoverManager.HidePage();
  }
}
