using ShipOfTheseus2025.Components.Game;
using ShipOfTheseus2025.Enum;

public interface IHoverPanelManager
{
  void ShowItem(InventoryItem item, HoverType hoverType);
  void HidePage();
  void SetEnabled(bool enabled);
}