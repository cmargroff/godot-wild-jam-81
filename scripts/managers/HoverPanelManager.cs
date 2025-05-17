using Godot;
using ShipOfTheseus2025.Enum;

namespace ShipOfTheseus2025.Components.Game;

public partial class HoverPanelManager : Control
{
  public HoverPage _page;
  private Marker2D _slotMarker;
  private Marker2D _hoverMarker;
  public override void _EnterTree()
  {
    Name = GetType().Name;
    InstantiatePage();
    InstantiateMarkers();
  }
  public void InstantiatePage()
  {
    var scene = ResourceLoader.Load<PackedScene>("res://components/game/HoverPage.tscn");
    _page = scene.Instantiate<HoverPage>();
    _page.Visible = false;
    AddChild(_page);
  }

  public void InstantiateMarkers()
  {
    var scene = ResourceLoader.Load<PackedScene>("res://components/game/ItemDragLayout.tscn");
    var layout = scene.Instantiate<Control>();

    _slotMarker = layout.GetNode<Marker2D>("%HoverSlotMarker");
    _hoverMarker = layout.GetNode<Marker2D>("%HoverItemMarker");

    AddChild(layout);
  }

  public void ShowItem(InventoryItem item, HoverType hoverType)
  {
    _page.Show(item);
    if (hoverType == HoverType.Item)
    {
      _page.Position = _hoverMarker.Position;
    }
    else
    {
      _page.Position = _slotMarker.Position;

    }
  }
  public void HidePage()
  {
    _page.Hide();
  }
}
