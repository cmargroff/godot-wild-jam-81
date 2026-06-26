using System;
using Godot;
using ShipOfTheseus2025.DependencyInjection;
using ShipOfTheseus2025.Enum;
using ShipOfTheseus2025.Models;

namespace ShipOfTheseus2025.Views;

public partial class Game : Node3D
{
  private ISceneManager _sceneManager;
  private IStatsManager _statsManager;
  private IGameEventManager _eventManager;
  private IItemDragManager _dragManager;
  private IPauseManager _pauseManager;
  private IGameManager _gameManager;
  private IAudioManager _audioManager;
  private IItemSpawnManager _itemSpawnManager;

  private GameOver _gameOverScreen;
  private Timer _gameOverTimer;
  private Port _port;

  /// <summary>
  /// The expected time for the game to finish at normal speed, in seconds.
  /// </summary>
  public float RunTimeAt1X { get; set; } = 300f;

  /// <summary>
  /// The time remaining
  /// </summary>
  public float RemainingTime { get; set; } = 300f;

  public float InitialKnots { get; set; } = 7f;
  [Export]
  public float SpeedScale { get; set; } = 1f;

  [FromServices]
  public void Inject(ISceneManager sceneManager, IStatsManager statsManager, IGameEventManager eventManager,
      IItemDragManager dragManager, IPauseManager pauseManager, IGameManager gameManager, IHoverPanelManager hoverPanelManager, IAudioManager audioManager, IItemSpawnManager itemSpawnManager)
  {
    _sceneManager = sceneManager;
    _statsManager = statsManager;
    _eventManager = eventManager;
    _dragManager = dragManager;
    _pauseManager = pauseManager;
    _gameManager = gameManager;
    _audioManager = audioManager;
    _itemSpawnManager = itemSpawnManager;
  }

  public override void _EnterTree()
  {
    _gameOverScreen = GetNode<GameOver>("GameOver");
    _gameOverTimer = GetNode<Timer>("GameOverTimer");
    //used when the Game scene is loaded directly, otherwise this will be skipped
    if (_sceneManager is null)
    {
      // Globals.Instance.InjectAttributedMethods(this, Globals.Instance.ServiceProvider);
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
    // _sceneManager.GetChild<Control>(0).Visible = false; //hides loading screen without crashing when running the game scene directly
    _audioManager.PlayGlobalAudioOnRepeat(_sceneManager.PreloadedResources["AudioRandomizers"]["waves_audio_stream_randomizer.tres"] as AudioStreamRandomizer,
        "SFX", this, new(0, 2f), true, (AudioStreamPlayer player) => player.VolumeDb = -6f, null);
    _audioManager.PlayGlobalAudioOnRepeat(_sceneManager.PreloadedResources["AudioRandomizers"]["ship_creaking_audio_stream_randomizer.tres"] as AudioStreamRandomizer,
        "SFX", this, new(2, 5f), false, null, null);

    if (_itemSpawnManager is Node3D itemSpawnManagerNode)
    {
      AddChild(itemSpawnManagerNode);
    }
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
      // _gameOverTimer.Start();
      // InstantiatePort();

    }
  }
  // public void InstantiatePort()
  // {
  //   var scene = ResourceLoader.Load<PackedScene>("res://components/game/Port.tscn");
  //   _port = scene.Instantiate<Port>();
  //   _port.Visible = true;
  //   AddChild(_port);
  //   _port.GlobalPosition = new Vector3(62, 7, -35);
  //   _port.Rotation = new Vector3(0, 90, 0);
  // }

  // public void GameOver()
  // {
  //   GD.Print("game over");
  //   _gameOverScreen.ShowScreen(true);
  //   _pauseManager.Pause();

  // }

}