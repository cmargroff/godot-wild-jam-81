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
  private ItemDragManager _dragManager;
  private PauseManager _pauseManager;

  [FromServices]
  public void Inject(SceneManager sceneManager, StatsManager statsManager, GameEventManager eventManager, ItemDragManager dragManager, PauseManager pauseManager)
  {
    _sceneManager = sceneManager;
    _statsManager = statsManager;
    _eventManager = eventManager;
    _dragManager = dragManager;
    AddChild(_eventManager);
    _pauseManager = pauseManager;
    AddChild(_pauseManager);
    AddChild(_dragManager);
  }
  public override void _EnterTree()
  {
    _animationPlayer = GetNode<AnimationPlayer>("%AnimationPlayer");
    _animationPlayer.CurrentAnimation = "rocking";
  }
  public override void _Ready()
  {
    _dragManager.SetCamera(GetNode<Camera3D>("Camera"));
    _eventManager.Start();
  }
}