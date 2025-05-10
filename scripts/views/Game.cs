using Godot;
using JamTemplate.Managers;
using JamTemplate.Util;

namespace JamTemplate.Views;

public partial class Game : Node3D
{
  private AnimationPlayer _animationPlayer;
  private SceneManager _sceneManager;
  private StatsManager _statsManager;
  private GameEventManager _eventManager;
  [FromServices]
  public void Inject(SceneManager sceneManager, StatsManager statsManager, GameEventManager eventManager)
  {
    _sceneManager = sceneManager;
    _statsManager = statsManager;
    _eventManager = eventManager;
    AddChild(_eventManager);
  }
  public override void _EnterTree()
  {
    _animationPlayer = GetNode<AnimationPlayer>("%AnimationPlayer");
    _animationPlayer.CurrentAnimation = "rocking";
  }
  public override void _Ready()
  {
    _eventManager.Start();
  }
}