using Godot;
using ShipOfTheseus2025.Enum;

namespace ShipOfTheseus2025.Components.Game;

public partial class HoverPanelManager : Control, IHoverPanelManager
{
  public HoverPage _page;
  private Marker2D _slotMarker;
  private Marker2D _hoverMarker;
  private bool _enabled = true;
  public override void _EnterTree()
  {
    Name = GetType().Name;
    InstantiatePage();
    InstantiateMarkers();
  }
  private void InstantiatePage()
  {
    var scene = ResourceLoader.Load<PackedScene>("res://components/game/HoverPage.tscn");
    _page = scene.Instantiate<HoverPage>();
    _page.Visible = false;
    AddChild(_page);
  }

  private void InstantiateMarkers()
  {
    var scene = ResourceLoader.Load<PackedScene>("res://components/game/ItemDragLayout.tscn");
    var layout = scene.Instantiate<Control>();

    _slotMarker = layout.GetNode<Marker2D>("%HoverSlotMarker");
    _hoverMarker = layout.GetNode<Marker2D>("%HoverItemMarker");

    AddChild(layout);
  }

  public void ShowItem(InventoryItem item, HoverType hoverType)
  {
    if (!_enabled) return;
    _page.Show(item);
    _page.Position = _hoverMarker.Position;
    // if (hoverType == HoverType.Item)
    // {
    //   _page.Position = _hoverMarker.Position;
    // }
    // else
    // {
    //   _page.Position = _slotMarker.Position;

    // }
  }
  public void HidePage()
  {
    _page.Hide();
  }
  public void SetEnabled(bool enabled)
  {
    _enabled = enabled;
  }
}
