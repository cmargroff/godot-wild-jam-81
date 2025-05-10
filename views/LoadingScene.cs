using Godot;

namespace ShipOfTheseus2025.Views;

public partial class LoadingScene : Control
{
  public override void _EnterTree()
  {
    (FindChild("AnimationPlayer") as AnimationPlayer).Play("spin");
  }
}
