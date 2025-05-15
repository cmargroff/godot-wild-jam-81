using Godot;

namespace ShipOfTheseus2025.Components.Game;

public partial class HoverPanelManager : Control
{
  public HoverPage _page;
  public override void _EnterTree()
  {
    var scene = ResourceLoader.Load<PackedScene>("res://components/game/HoverPage.tscn");
    _page = scene.Instantiate<HoverPage>();
    _page.Visible = false;
    AddChild(_page);
  }

  public void ShowItem(InventoryItem item)
  {
    _page.Show(item);
  }
  public void HidePage()
  {
    _page.Hide();
  }
}
