using Godot;
using ShipOfTheseus2025.Managers;
using ShipOfTheseus2025.Util;

namespace ShipOfTheseus2025.Views;

public partial class Game : Node3D
{
  private AnimationPlayer _animationPlayer;
  private SceneManager _sceneManager;
  private StatsManager _statsManager;
  private GameEventManager _eventManager;

  [FromServices]
  public void Inject(SceneManager sceneManager, StatsManager statsManager, GameEventManager eventManager, ItemDragManager dragManager)
  {
    _sceneManager = sceneManager;
    _statsManager = statsManager;
    _eventManager = eventManager;
    AddChild(_eventManager);
    AddChild(dragManager);
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