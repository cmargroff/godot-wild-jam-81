using Godot;
using Microsoft.Extensions.DependencyInjection;
using ShipOfTheseus2025;

public partial class Inventory : Control
{
  [Export]
  public PackedScene SlotTemplate;
  public int SlotsAvailable = 7;
  private int _rowsAvailable = 1;
  private Vector2 _origin;
  private VFlowContainer _slotsWrap;
  public override void _EnterTree()
  {
    _slotsWrap = GetNode<VFlowContainer>("%SlotsWrap");
    _rowsAvailable = Mathf.CeilToInt(SlotsAvailable / 7);
    _origin = Position * 1;

    for (var i = 0; i < SlotsAvailable; i++)
    {
      var slot = Globals.Instance.ServiceProvider.GetRequiredService<InventoryItemSlot>();
      _slotsWrap.AddChild(slot);
    }

    Connect(SignalName.MouseEntered, Callable.From(_MouseEntered));
    Connect(SignalName.MouseExited, Callable.From(_MouseExited));
  }
  private void _MouseEntered()
  {
    var tween = CreateTween();
    tween.TweenProperty(this, "position", _origin + new Vector2(_rowsAvailable * 75, 0), 0.15);
    tween.Play();
  }
  private void _MouseExited()
  {
    var tween = CreateTween();
    tween.TweenProperty(this, "position", _origin, 0.15);
    tween.Play();
  }
}
