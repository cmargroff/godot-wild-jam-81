using System;
using Godot;
using ShipOfTheseus2025.Components.Game;
using ShipOfTheseus2025.Enum;
using ShipOfTheseus2025.Managers;
using ShipOfTheseus2025.Models;
using ShipOfTheseus2025.Util;

namespace ShipOfTheseus2025.Views;

public partial class Game : Node3D
{
  private SceneManager _sceneManager;
  private StatsManager _statsManager;
  private GameEventManager _eventManager;
  private ItemDragManager _dragManager;
  private PauseManager _pauseManager;
  private GameManager _gameManager;

      /// <summary>
    /// The expected time for the game to finish at normal speed, in seconds.
    /// </summary>
    public float RunTimeAt1X { get; set; } = 600f;

    /// <summary>
    /// The time remaining
    /// </summary>
    public float RemainingTime { get; set; } = 600f;

    public float InitialKnots { get; set; } = 7f;
   [Export]
    public float SpeedScale { get; set; } = 1f;

  [FromServices]
  public void Inject(SceneManager sceneManager, StatsManager statsManager, GameEventManager eventManager,
      ItemDragManager dragManager, PauseManager pauseManager, GameManager gameManager, HoverPanelManager hoverPanelManager)
  {
    _sceneManager = sceneManager;
    _statsManager = statsManager;
    _eventManager = eventManager;
    _dragManager = dragManager;
    AddChild(_eventManager);
    _pauseManager = pauseManager;
    AddChild(_pauseManager);
    AddChild(_dragManager);
    _gameManager = gameManager;
  }

    public override void _EnterTree()
  {
    //used when the Game scene is loaded directly, otherwise this will be skipped
    if (_sceneManager is null)
    {
        Globals.InjectAttributedMethods(this, Globals.ServiceProvider);
    }
    if (_gameManager.EnabledItems is null || _gameManager.EnabledItems.Count == 0)
        _gameManager.LoadConfig();
#if DEBUG
    if (_gameManager.EnabledItems is not null && _gameManager.EnabledItems.Count > 0)
        _gameManager.LoadItemsDirectly();
#endif
  }

  public override void _Ready()
  {
    _dragManager.SetCamera(GetNode<Camera3D>("Camera"));
    _eventManager.Start();
    _sceneManager.GetChild<Control>(0).Visible = false; //hides loading screen without crashing when running the game scene directly
  }

    public override void _PhysicsProcess(double delta)
    {
        RemainingTime = Math.Max(0, RemainingTime - (float)(delta * SpeedScale));
        _statsManager.ChangeStat(new StatChange{Stat = Stat.Progress, Mode = StatChangeMode.Absolute, Amount = (1 - RemainingTime / RunTimeAt1X) * 100f});
    }

}