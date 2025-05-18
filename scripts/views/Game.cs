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
  private AudioManager _audioManager;

  private GameOver _gameOverScreen;

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
      ItemDragManager dragManager, PauseManager pauseManager, GameManager gameManager, HoverPanelManager hoverPanelManager, AudioManager audioManager)
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
    _audioManager = audioManager;
  }

  public override void _EnterTree()
  {
    _gameOverScreen = GetNode<GameOver>("GameOver");

    //used when the Game scene is loaded directly, otherwise this will be skipped
    if (_sceneManager is null)
    {
      Globals.InjectAttributedMethods(this, Globals.ServiceProvider);
    }
    if (_gameManager.EnabledItems is null || _gameManager.EnabledItems.Count == 0)
      _gameManager.LoadConfig();
#if DEBUG
    if (_gameManager.EnabledItems is not null && _gameManager.EnabledItems.Count > 0)
    {
      _gameManager.LoadItemsDirectly();
    }
#endif
  }

  public override void _Ready()
  {
    _dragManager.SetCamera(GetNode<Camera3D>("Camera"));
    _eventManager.Start();
    _sceneManager.GetChild<Control>(0).Visible = false; //hides loading screen without crashing when running the game scene directly
    _audioManager.PlayGlobalAudioOnRepeat(_sceneManager.PreloadedResources["AudioRandomizers"]["waves_audio_stream_randomizer.tres"] as AudioStreamRandomizer,
        "SFX", this, new(0, 2f), true, (AudioStreamPlayer player) => player.VolumeDb = -6f, null);
    _audioManager.PlayGlobalAudioOnRepeat(_sceneManager.PreloadedResources["AudioRandomizers"]["ship_creaking_audio_stream_randomizer.tres"] as AudioStreamRandomizer,
        "SFX", this, new(2, 5f), false, null, null);
  }

  public override void _PhysicsProcess(double delta)
  {
    RemainingTime = Math.Max(0, RemainingTime - (float)(delta * SpeedScale));
    _statsManager.ChangeStat(new StatChange { Stat = Stat.Progress, Mode = StatChangeMode.Absolute, Amount = (1 - RemainingTime / RunTimeAt1X) * 100f });
    if (_statsManager.GetStats(Stat.WaterLevel) >= 100)
    {
      _gameOverScreen.ShowScreen(false);
    }
    else if (_statsManager.GetStats(Stat.Progress) >= 100)
    {
      _gameOverScreen.ShowScreen(true);
    }
  }



}