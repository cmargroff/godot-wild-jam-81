using Godot;
using JamTemplate.Managers;
using JamTemplate.Util;

public partial class Game : Node3D
{
  private AnimationPlayer _animationPlayer;
  private SceneManager _sceneManager;
  [FromServices]
  public void Inject(SceneManager sceneManager)
  {
    _sceneManager = sceneManager;
  }
  public override void _EnterTree()
  {
    _animationPlayer = GetNode<AnimationPlayer>("%AnimationPlayer");
    _animationPlayer.CurrentAnimation = "rocking";
  }

}
