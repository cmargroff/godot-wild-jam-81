using Godot;

public partial class Ship : Node3D
{
  private AnimationPlayer _animationPlayer;
  private MeshInstance3D _model;
  public override void _EnterTree()
  {
    // _model = GetNode<MeshInstance3D>("Model");
    // _animationPlayer = GetNode<AnimationPlayer>("%AnimationPlayer");
    // _animationPlayer.CurrentAnimation = "rocking";
  }
}
