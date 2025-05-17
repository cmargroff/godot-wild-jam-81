using System;
using Godot;
using ShipOfTheseus2025.Enum;

public partial class ItemPageTrait : Control
{
  private Label _label;
  public override void _EnterTree()
  {
    _label = GetNode<Label>("Label");
  }
  public void SetTrait(ItemTrait itemTrait)
  {
    _label.Text = itemTrait.Description;
    if (itemTrait.FixedValue < 0)
    {
      _label.AddThemeColorOverride("font_color", new Color(0.9f, 0.15f, 0.15f, 1));
    }
    else
    {
      _label.AddThemeColorOverride("font_color", new Color(0, 1, 0, 1));
    }
  }

}
