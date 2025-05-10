using Godot;
using JamTemplate.Managers;
using JamTemplate.Util;

namespace JamTemplate.Views;

public partial class Game : Node3D
{
  private AnimationPlayer _animationPlayer;
  private SceneManager _sceneManager;
  private StatsManager _statsManager;
  [FromServices]
  public void Inject(SceneManager sceneManager, StatsManager statsManager)
  {
    _sceneManager = sceneManager;
    _statsManager = statsManager;
  }
  public override void _EnterTree()
  {
    _animationPlayer = GetNode<AnimationPlayer>("%AnimationPlayer");
    _animationPlayer.CurrentAnimation = "rocking";
  }
}