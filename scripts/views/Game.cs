using Godot;
using JamTemplate.Util;

public partial class Game : Node3D
{
  private AnimationPlayer _animationPlayer;
  [FromServices]
  public void Inject()
  {

  }
  public override void _EnterTree()
  {
    _animationPlayer = GetNode<AnimationPlayer>("%AnimationPlayer");
    _animationPlayer.CurrentAnimation = "rocking";
  }

}
