using Godot;

namespace ShipOfTheseus2025.Views;

public partial class LoadingScene : Control
{
  public override void _EnterTree()
  {
    GetNode<AnimationPlayer>("AnimationPlayer").Play("spin");
  }
}
