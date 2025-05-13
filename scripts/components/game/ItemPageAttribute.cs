using System;
using Godot;

public partial class ItemPageAttribute : Control
{
  private Label _label;
  public override void _EnterTree()
  {
    _label = GetNode<Label>("Label");
  }
  public void SetAttribute(object itemAttribute)
  {
    // _label = itemAttribute.GetName();
  }
}
